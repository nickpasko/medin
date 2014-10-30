using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Libs;
using MedIn.OziCms.Controllers;

namespace MedIn.OziCms.Mvc
{
	public class OziModelBinder : DefaultModelBinder
	{
		private ModelBinderDictionary _binders;

		private ModelBinderDictionary OziBinders
		{
			get { return _binders ?? (_binders = ModelBinders.Binders); }
		}

		internal void BindComplexElementalModel(ControllerContext controllerContext, ModelBindingContext bindingContext, object model)
		{
			// need to replace the property filter + model object and create an inner binding context
			ModelBindingContext newBindingContext = CreateComplexElementalModelBindingContext(controllerContext, bindingContext, model);

			// validation
			if (OnModelUpdating(controllerContext, newBindingContext))
			{
				BindProperties(controllerContext, newBindingContext);
				OnModelUpdated(controllerContext, newBindingContext);
			}
		}

		internal object BindComplexModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var model = bindingContext.Model;
			var modelType = bindingContext.ModelType;

			// if we're being asked to create an array, create a list instead, then coerce to an array after the list is created
			if (model == null && modelType.IsArray)
			{
				var elementType = modelType.GetElementType();
				var listType = typeof(List<>).MakeGenericType(elementType);
				var collection = CreateModel(controllerContext, bindingContext, listType);

				var arrayBindingContext = new ModelBindingContext
				{
					ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => collection, listType),
					ModelName = bindingContext.ModelName,
					ModelState = bindingContext.ModelState,
					PropertyFilter = bindingContext.PropertyFilter,
					ValueProvider = bindingContext.ValueProvider
				};
				var list = (IList)UpdateCollection(controllerContext, arrayBindingContext, elementType);

				if (list == null)
				{
					return null;
				}

				var array = Array.CreateInstance(elementType, list.Count);
				list.CopyTo(array, 0);
				return array;
			}

			if (model == null)
			{
				model = CreateModel(controllerContext, bindingContext, modelType);
			}

			// special-case IDictionary<,> and ICollection<>
			var dictionaryType = TypeHelpers.ExtractGenericInterface(modelType, typeof(IDictionary<,>));
			if (dictionaryType != null)
			{
				var genericArguments = dictionaryType.GetGenericArguments();
				var keyType = genericArguments[0];
				var valueType = genericArguments[1];

				var dictionaryBindingContext = new ModelBindingContext
				{
					ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, modelType),
					ModelName = bindingContext.ModelName,
					ModelState = bindingContext.ModelState,
					PropertyFilter = bindingContext.PropertyFilter,
					ValueProvider = bindingContext.ValueProvider
				};
				var dictionary = UpdateDictionary(controllerContext, dictionaryBindingContext, keyType, valueType);
				return dictionary;
			}

			var enumerableType = TypeHelpers.ExtractGenericInterface(modelType, typeof(IEnumerable<>));
			if (enumerableType != null)
			{
				var elementType = enumerableType.GetGenericArguments()[0];

				var collectionType = typeof(ICollection<>).MakeGenericType(elementType);
				if (collectionType.IsInstanceOfType(model))
				{
					var collectionBindingContext = new ModelBindingContext
					{
						ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, modelType),
						ModelName = bindingContext.ModelName,
						ModelState = bindingContext.ModelState,
						PropertyFilter = bindingContext.PropertyFilter,
						ValueProvider = bindingContext.ValueProvider
					};
					var collection = UpdateCollection(controllerContext, collectionBindingContext, elementType);
					return collection;
				}
			}

			// otherwise, just update the properties on the complex type
			BindComplexElementalModel(controllerContext, bindingContext, model);
			return model;
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			EnsureStackHelper.EnsureStack();

			if (bindingContext == null)
			{
				throw new ArgumentNullException("bindingContext");
			}

			var performedFallback = false;

			if (!String.IsNullOrEmpty(bindingContext.ModelName) && !bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName) && !(typeof(IEnumerable).IsAssignableFrom(bindingContext.ModelType)))
			{
				// We couldn't find any entry that began with the prefix. If this is the top-level element, fall back
				// to the empty prefix.
				if (bindingContext.FallbackToEmptyPrefix)
				{
					bindingContext = new ModelBindingContext
					{
						ModelMetadata = bindingContext.ModelMetadata,
						ModelState = bindingContext.ModelState,
						PropertyFilter = bindingContext.PropertyFilter,
						ValueProvider = bindingContext.ValueProvider
					};
					performedFallback = true;
				}
				else
				{
					return null;
				}
			}

			// Simple model = int, string, etc.; determined by calling TypeConverter.CanConvertFrom(typeof(string))
			// or by seeing if a value in the request exactly matches the name of the model we're binding.
			// Complex type = everything else.
			if (!performedFallback)
			{
				var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
				if (valueProviderResult != null)
				{
					return BindSimpleModel(controllerContext, bindingContext, valueProviderResult);
				}
			}
			if (!bindingContext.ModelMetadata.IsComplexType)
			{
				return null;
			}

			return BindComplexModel(controllerContext, bindingContext);
		}

		private void BindProperties(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			IEnumerable<PropertyDescriptor> properties = OziGetFilteredModelProperties(controllerContext, bindingContext);
			foreach (PropertyDescriptor property in properties)
			{
				BindProperty(controllerContext, bindingContext, property);
			}
		}

		protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
		{
			// need to skip properties that aren't part of the request, else we might hit a StackOverflowException
			var fullPropertyKey = OziCreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);
			if (!bindingContext.ValueProvider.ContainsPrefix(fullPropertyKey) && (!(typeof(IEnumerable).IsAssignableFrom(propertyDescriptor.PropertyType)) || typeof(string) == propertyDescriptor.PropertyType))
			{
				return;
			}

			// call into the property's model binder
			//IModelBinder propertyBinder = OziBinders.GetBinder(propertyDescriptor.PropertyType);
			//if (propertyBinder.GetType() == typeof (DefaultModelBinder))
			//{
			//	propertyBinder = this;
			//}
			var originalPropertyValue = propertyDescriptor.GetValue(bindingContext.Model);
			var propertyMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
			propertyMetadata.Model = originalPropertyValue;
			var innerBindingContext = new ModelBindingContext
			{
				ModelMetadata = propertyMetadata,
				ModelName = fullPropertyKey,
				ModelState = bindingContext.ModelState,
				ValueProvider = bindingContext.ValueProvider
			};
			var newPropertyValue = GetPropertyValue(controllerContext, innerBindingContext, propertyDescriptor, this);
			//object newPropertyValue = GetPropertyValue(controllerContext, innerBindingContext, propertyDescriptor, propertyBinder);
			propertyMetadata.Model = newPropertyValue;

			// validation
			var modelState = bindingContext.ModelState[fullPropertyKey];
			if (modelState == null || modelState.Errors.Count == 0)
			{
				if (originalPropertyValue != newPropertyValue && OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, newPropertyValue))
				{
					SetProperty(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);
					OnPropertyValidated(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);
				}
			}
			else
			{
				SetProperty(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);

				// Convert FormatExceptions (type conversion failures) into InvalidValue messages
				foreach (var error in modelState.Errors.Where(err => String.IsNullOrEmpty(err.ErrorMessage) && err.Exception != null).ToList())
				{
					for (var exception = error.Exception; exception != null; exception = exception.InnerException)
					{
						if (exception is FormatException)
						{
							break;
						}
					}
				}
			}
		}

		internal object BindSimpleModel(ControllerContext controllerContext, ModelBindingContext bindingContext, ValueProviderResult valueProviderResult)
		{
			bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

			// if the value provider returns an instance of the requested data type, we can just short-circuit
			// the evaluation and return that instance
			if (bindingContext.ModelType.IsInstanceOfType(valueProviderResult.RawValue))
			{
				return valueProviderResult.RawValue;
			}

			// since a string is an IEnumerable<char>, we want it to skip the two checks immediately following
			if (bindingContext.ModelType != typeof(string))
			{
				// conversion results in 3 cases, as below
				if (bindingContext.ModelType.IsArray)
				{
					// case 1: user asked for an array
					// ValueProviderResult.ConvertTo() understands array types, so pass in the array type directly
					object modelArray = ConvertProviderResult(bindingContext.ModelState, bindingContext.ModelName, valueProviderResult, bindingContext.ModelType);
					return modelArray;
				}

				var enumerableType = TypeHelpers.ExtractGenericInterface(bindingContext.ModelType, typeof(IEnumerable<>));
				if (enumerableType != null)
				{
					// case 2: user asked for a collection rather than an array
					// need to call ConvertTo() on the array type, then copy the array to the collection
					var modelCollection = CreateModel(controllerContext, bindingContext, bindingContext.ModelType);
					var elementType = enumerableType.GetGenericArguments()[0];
					var arrayType = elementType.MakeArrayType();
					var modelArray = ConvertProviderResult(bindingContext.ModelState, bindingContext.ModelName, valueProviderResult, arrayType);

					var collectionType = typeof(ICollection<>).MakeGenericType(elementType);
					if (collectionType.IsInstanceOfType(modelCollection))
					{
						CollectionHelpers.ReplaceCollection(elementType, modelCollection, modelArray);
					}
					return modelCollection;
				}
			}

			// case 3: user asked for an individual element
			var model = ConvertProviderResult(bindingContext.ModelState, bindingContext.ModelName, valueProviderResult, bindingContext.ModelType);
			return model;
		}

		private static bool CanUpdateReadonlyTypedReference(Type type)
		{
			// value types aren't strictly immutable, but because they have copy-by-value semantics
			// we can't update a value type that is marked readonly
			if (type.IsValueType)
			{
				return false;
			}

			// arrays are mutable, but because we can't change their length we shouldn't try
			// to update an array that is referenced readonly
			if (type.IsArray)
			{
				return false;
			}

			// special-case known common immutable types
			if (type == typeof(string))
			{
				return false;
			}

			return true;
		}

		private static object ConvertProviderResult(ModelStateDictionary modelState, string modelStateKey, ValueProviderResult valueProviderResult, Type destinationType)
		{
			try
			{
				var convertedValue = valueProviderResult.ConvertTo(destinationType);
				return convertedValue;
			}
			catch (Exception ex)
			{
				modelState.AddModelError(modelStateKey, ex);
				return null;
			}
		}

		internal ModelBindingContext CreateComplexElementalModelBindingContext(ControllerContext controllerContext, ModelBindingContext bindingContext, object model)
		{
			var bindAttr = (BindAttribute)GetTypeDescriptor(controllerContext, bindingContext).GetAttributes()[typeof(BindAttribute)];
			var newPropertyFilter = (bindAttr != null) ? propertyName => bindAttr.IsPropertyAllowed(propertyName) && bindingContext.PropertyFilter(propertyName) : bindingContext.PropertyFilter;
			var newBindingContext = new ModelBindingContext
			{
				ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, bindingContext.ModelType),
				ModelName = bindingContext.ModelName,
				ModelState = bindingContext.ModelState,
				PropertyFilter = newPropertyFilter,
				ValueProvider = bindingContext.ValueProvider
			};
			return newBindingContext;
		}

		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			var typeToCreate = modelType;

			// we can understand some collection interfaces, e.g. IList<>, IDictionary<,>
			if (modelType.IsGenericType)
			{
				var genericTypeDefinition = modelType.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(IDictionary<,>))
				{
					typeToCreate = typeof(Dictionary<,>).MakeGenericType(modelType.GetGenericArguments());
				}
				else if (genericTypeDefinition == typeof(IEnumerable<>) || genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IList<>))
				{
					typeToCreate = typeof(List<>).MakeGenericType(modelType.GetGenericArguments());
				}
			}
			// fallback to the type's default constructor
			object result = null;
			var isEntityCollection = typeof (IEntity).IsAssignableFrom(typeToCreate);
			var db = ((OziController)controllerContext.Controller).DataModelContext;
			if (isEntityCollection)
			{
				// надо вот именно здесь проверять существует ли уже такое значение в базе (по ключу и, если надо, имени)
				// именно тут надо добавлять элемент в базу. Итак:
				var id = bindingContext.ValueProvider.GetValue(OziCreateSubPropertyName(bindingContext.ModelName, "Id"));
				db.TryGetObjectByKey(new EntityKey(TypeHelpers.GetEntitySetName(typeToCreate, db), "Id", id.ConvertTo(typeof(int))), out result);
				if (result == null)
				{
					var added = db.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
					var name = bindingContext.ValueProvider.GetValue(OziCreateSubPropertyName(bindingContext.ModelName, "Name"));
					result = added.FirstOrDefault(entry => TypeHelpers.GetPropertyValue(entry.Entity, "Name", null) == name);
					if (result != null)
					{
						result = ((ObjectStateEntry)result).Entity;
					}
				}
			}
			if (result == null)
			{
				result = Activator.CreateInstance(typeToCreate);
				if (isEntityCollection)
				{
					db.AddObject(TypeHelpers.GetEntitySetName(typeToCreate, db), result);
				}
			}
			return result;
		}

		protected static string OziCreateSubIndexName(string prefix, int index)
		{
			return String.Format(CultureInfo.InvariantCulture, "{0}[{1}]", prefix, index);
		}

		protected static string OziCreateSubIndexName(string prefix, string index)
		{
			return String.Format(CultureInfo.InvariantCulture, "{0}[{1}]", prefix, index);
		}

		protected static string OziCreateSubPropertyName(string prefix, string propertyName)
		{
			if (String.IsNullOrEmpty(prefix))
			{
				return propertyName;
			}
			if (String.IsNullOrEmpty(propertyName))
			{
				return prefix;
			}
			return prefix + "." + propertyName;
		}

		protected IEnumerable<PropertyDescriptor> OziGetFilteredModelProperties(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var properties = GetModelProperties(controllerContext, bindingContext);
			var propertyFilter = bindingContext.PropertyFilter;

			return from PropertyDescriptor property in properties
				   where ShouldUpdateProperty(property, propertyFilter)
				   select property;
		}

		private static void GetIndexes(ModelBindingContext bindingContext, out bool stopOnIndexNotFound, out IEnumerable<string> indexes)
		{
			var indexKey = OziCreateSubPropertyName(bindingContext.ModelName, "index");
			var valueProviderResult = bindingContext.ValueProvider.GetValue(indexKey);

			if (valueProviderResult != null)
			{
				var indexesArray = valueProviderResult.ConvertTo(typeof(string[])) as string[];
				if (indexesArray != null)
				{
					stopOnIndexNotFound = false;
					indexes = indexesArray;
					return;
				}
			}

			// just use a simple zero-based system
			stopOnIndexNotFound = true;
			indexes = GetZeroBasedIndexes();
		}

		protected override PropertyDescriptorCollection GetModelProperties(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			return GetTypeDescriptor(controllerContext, bindingContext).GetProperties();
		}

		protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
		{
			var value = propertyBinder.BindModel(controllerContext, bindingContext);

			if (bindingContext.ModelMetadata.ConvertEmptyStringToNull && Equals(value, String.Empty))
			{
				return null;
			}

			return value;
		}

		// If the user specified a ResourceClassKey try to load the resource they specified.
		// If the class key is invalid, an exception will be thrown.
		// If the class key is valid but the resource is not found, it returns null, in which
		// case it will fall back to the MVC default error message.
		//private static string GetUserResourceString(ControllerContext controllerContext, string resourceName)
		//{
		//	string result = null;

		//	if (!String.IsNullOrEmpty(ResourceClassKey) && (controllerContext != null) && (controllerContext.HttpContext != null))
		//	{
		//		result = controllerContext.HttpContext.GetGlobalResourceObject(ResourceClassKey, resourceName, CultureInfo.CurrentUICulture) as string;
		//	}

		//	return result;
		//}

		private static IEnumerable<string> GetZeroBasedIndexes()
		{
			var i = 0;
			while (true)
			{
				yield return i.ToString(CultureInfo.InvariantCulture);
				i++;
			}
		}

		protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var startedValid = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

			foreach (var validationResult in ModelValidator.GetModelValidator(bindingContext.ModelMetadata, controllerContext).Validate(null))
			{
				var subPropertyName = OziCreateSubPropertyName(bindingContext.ModelName, validationResult.MemberName);

				if (!startedValid.ContainsKey(subPropertyName))
				{
					startedValid[subPropertyName] = bindingContext.ModelState.IsValidField(subPropertyName);
				}

				if (startedValid[subPropertyName])
				{
					bindingContext.ModelState.AddModelError(subPropertyName, validationResult.Message);
				}
			}
		}

		protected override bool OnModelUpdating(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			// default implementation does nothing
			return true;
		}

		protected override void OnPropertyValidated(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
		{
			// default implementation does nothing
		}

		protected override bool OnPropertyValidating(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
		{
			// default implementation does nothing
			return true;
		}

		protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
		{
			var propertyMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
			propertyMetadata.Model = value;
			var modelStateKey = OziCreateSubPropertyName(bindingContext.ModelName, propertyMetadata.PropertyName);

			// If the value is null, and the validation system can find a Required validator for
			// us, we'd prefer to run it before we attempt to set the value; otherwise, property
			// setters which throw on null (f.e., Entity Framework properties which are backed by
			// non-nullable strings in the DB) will get their error message in ahead of us.
			//
			// We are effectively using the special validator -- Required -- as a helper to the
			// binding system, which is why this code is here instead of in the Validating/Validated
			// methods, which are really the old-school validation hooks.
			if (value == null && bindingContext.ModelState.IsValidField(modelStateKey))
			{
				var requiredValidator = ModelValidatorProviders.Providers.GetValidators(propertyMetadata, controllerContext).FirstOrDefault(v => v.IsRequired);
				if (requiredValidator != null)
				{
					foreach (var validationResult in requiredValidator.Validate(bindingContext.Model))
					{
						bindingContext.ModelState.AddModelError(modelStateKey, validationResult.Message);
					}
				}
			}

			var isNullValueOnNonNullableType = value == null && !TypeHelpers.TypeAllowsNullValue(propertyDescriptor.PropertyType);

			// Try to set a value into the property unless we know it will fail (read-only
			// properties and null values with non-nullable types)
			if (!propertyDescriptor.IsReadOnly && !isNullValueOnNonNullableType)
			{
				try
				{
					Debug.Assert(value != null);
					propertyDescriptor.SetValue(bindingContext.Model, value);
				}
				catch (Exception ex)
				{
					// Only add if we're not already invalid
					if (bindingContext.ModelState.IsValidField(modelStateKey))
					{
						bindingContext.ModelState.AddModelError(modelStateKey, ex);
					}
				}
			}
		}

		private static bool ShouldUpdateProperty(PropertyDescriptor property, Predicate<string> propertyFilter)
		{
			if (property.IsReadOnly && !CanUpdateReadonlyTypedReference(property.PropertyType))
			{
				return false;
			}

			// if this property is rejected by the filter, move on
			if (!propertyFilter(property.Name))
			{
				return false;
			}

			// otherwise, allow
			return true;
		}

		internal object UpdateCollection(ControllerContext controllerContext, ModelBindingContext bindingContext, Type elementType)
		{
			bool stopOnIndexNotFound;
			IEnumerable<string> indexes;
			GetIndexes(bindingContext, out stopOnIndexNotFound, out indexes);
			//var elementBinder = OziBinders.GetBinder(elementType);
			//if (elementBinder.GetType() == typeof(DefaultModelBinder))
			//{
			//	elementBinder = this;
			//}

			// build up a list of items from the request
			var modelList = new List<object>();
			var isEntityCollection = typeof (IEntity).IsAssignableFrom(elementType);
			foreach (string currentIndex in indexes)
			{
				var subIndexKey = OziCreateSubIndexName(bindingContext.ModelName, currentIndex);
				if (!bindingContext.ValueProvider.ContainsPrefix(subIndexKey))
				{
					if (stopOnIndexNotFound)
					{
						// we ran out of elements to pull
						break;
					}
					continue;
				}

				// ищем модель в исходном массиве, необходимо конечно проверить что у нас коллекции IEntity и тп
				object model = null;

				if (isEntityCollection)
				{
					var idVal = bindingContext.ValueProvider.GetValue(subIndexKey + ".Id");
					var id = idVal.ConvertTo(typeof(int));
					if (id != null)
					{
						model = ((IListSource) bindingContext.Model).GetList().Cast<IEntity>().FirstOrDefault<IEntity>(e => e.Id == (int)id);
					}
				}


				var innerContext = new ModelBindingContext
				{
					ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, elementType),
					ModelName = subIndexKey,
					ModelState = bindingContext.ModelState,
					PropertyFilter = bindingContext.PropertyFilter,
					ValueProvider = bindingContext.ValueProvider
				};
				//object thisElement = elementBinder.BindModel(controllerContext, innerContext);
				var thisElement = BindModel(controllerContext, innerContext);

				// we need to merge model errors up
				modelList.Add(thisElement);
			}

			// modelList содержит в себе все элементы которые должны быть в коллекции, поэтому надо обновить исходную коллекцию
			// в частности надо проверить какие элементы добавились (сверять по айдишникам и, если он нулевой, то и по имени)
			// и какие удалились, их нет в исходной коллекции

			object collection = bindingContext.Model;

			if (isEntityCollection)
			{
				// удаляем
				var sList = ((IListSource) collection).GetList();
				var srcList = sList.Cast<IEntity>().ToList();
				var listToRemove = new List<IEntity>();

				foreach (var o in srcList)
				{
					var m = (IEntity)modelList.FirstOrDefault(n => ((IEntity)n).Id == o.Id);
					if (m == null)
					{
						listToRemove.Add(o);
					}
				}
				listToRemove.ForEach(sList.Remove);

				// добавляем

				foreach (var m in modelList)
				{
					var name = TypeHelpers.GetPropertyValue(m, "Name", null);
					var o = srcList.FirstOrDefault(e => e.Id == ((IEntity) m).Id || (((IEntity)m).Id == 0 && (name == null || (name == TypeHelpers.GetPropertyValue(e, "Name", null)))));
					if (o == null)
					{
						sList.Add(m);
					}
				}

			}
			else // обычное поведение
			{
				// if there weren't any elements at all in the request, just return
				if (modelList.Count == 0)
				{
					return null;
				}

				// replace the original collection

				// оригинальную коллекцию переписывать не будем, вместо этого будем проверять существующие объекты, и подменять уже их свойства
				CollectionHelpers.ReplaceCollection(elementType, collection, modelList);
				
			}
			return collection;
		}

		internal object UpdateDictionary(ControllerContext controllerContext, ModelBindingContext bindingContext, Type keyType, Type valueType)
		{
			bool stopOnIndexNotFound;
			IEnumerable<string> indexes;
			GetIndexes(bindingContext, out stopOnIndexNotFound, out indexes);

			//IModelBinder keyBinder = OziBinders.GetBinder(keyType);
			//if (keyBinder.GetType() == typeof(DefaultModelBinder))
			//{
			//	keyBinder = this;
			//}
			//IModelBinder valueBinder = OziBinders.GetBinder(valueType);
			//if (valueBinder.GetType() == typeof(DefaultModelBinder))
			//{
			//	valueBinder = this;
			//}

			// build up a list of items from the request
			var modelList = new List<KeyValuePair<object, object>>();
			foreach (string currentIndex in indexes)
			{
				var subIndexKey = OziCreateSubIndexName(bindingContext.ModelName, currentIndex);
				var keyFieldKey = OziCreateSubPropertyName(subIndexKey, "key");
				var valueFieldKey = OziCreateSubPropertyName(subIndexKey, "value");

				if (!(bindingContext.ValueProvider.ContainsPrefix(keyFieldKey) && bindingContext.ValueProvider.ContainsPrefix(valueFieldKey)))
				{
					if (stopOnIndexNotFound)
					{
						// we ran out of elements to pull
						break;
					}
					continue;
				}

				// bind the key
				var keyBindingContext = new ModelBindingContext
				{
					ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, keyType),
					ModelName = keyFieldKey,
					ModelState = bindingContext.ModelState,
					ValueProvider = bindingContext.ValueProvider
				};
				object thisKey = BindModel(controllerContext, keyBindingContext);

				// we need to merge model errors up
				if (!keyType.IsInstanceOfType(thisKey))
				{
					// we can't add an invalid key, so just move on
					continue;
				}

				// bind the value
				modelList.Add(CreateEntryForModel(controllerContext, bindingContext, valueType, this, valueFieldKey, thisKey));
			}

			// Let's try another method
			if (modelList.Count == 0)
			{
				var enumerableValueProvider = bindingContext.ValueProvider as IEnumerableValueProvider;
				if (enumerableValueProvider != null)
				{
					var keys = enumerableValueProvider.GetKeysFromPrefix(bindingContext.ModelName);
					foreach (var thisKey in keys)
					{
						modelList.Add(CreateEntryForModel(controllerContext, bindingContext, valueType, this, thisKey.Value, thisKey.Key));
					}
				}
			}

			// if there weren't any elements at all in the request, just return
			if (modelList.Count == 0)
			{
				return null;
			}

			// replace the original collection
			var dictionary = bindingContext.Model;
			CollectionHelpers.ReplaceDictionary(keyType, valueType, dictionary, modelList);
			return dictionary;
		}

		private static KeyValuePair<object, object> CreateEntryForModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type valueType, IModelBinder valueBinder, string modelName, object modelKey)
		{
			var valueBindingContext = new ModelBindingContext
			{
				ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, valueType),
				ModelName = modelName,
				ModelState = bindingContext.ModelState,
				PropertyFilter = bindingContext.PropertyFilter,
				ValueProvider = bindingContext.ValueProvider
			};
			var thisValue = valueBinder.BindModel(controllerContext, valueBindingContext);
			return new KeyValuePair<object, object>(modelKey, thisValue);
		}

		// This helper type is used because we're working with strongly-typed collections, but we don't know the Ts
		// ahead of time. By using the generic methods below, we can consolidate the collection-specific code in a
		// single helper type rather than having reflection-based calls spread throughout the DefaultModelBinder type.
		// There is a single point of entry to each of the methods below, so they're fairly simple to maintain.

		private static class CollectionHelpers
		{
			private static readonly MethodInfo ReplaceCollectionMethod = typeof(CollectionHelpers).GetMethod("ReplaceCollectionImpl", BindingFlags.Static | BindingFlags.NonPublic);
			private static readonly MethodInfo ReplaceDictionaryMethod = typeof(CollectionHelpers).GetMethod("ReplaceDictionaryImpl", BindingFlags.Static | BindingFlags.NonPublic);

			[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
			public static void ReplaceCollection(Type collectionType, object collection, object newContents)
			{
				MethodInfo targetMethod = ReplaceCollectionMethod.MakeGenericMethod(collectionType);
				targetMethod.Invoke(null, new [] { collection, newContents });
			}

			private static void ReplaceCollectionImpl<T>(ICollection<T> collection, IEnumerable newContents)
			{
				collection.Clear();
				if (newContents != null)
				{
					foreach (object item in newContents)
					{
						// if the item was not a T, some conversion failed. the error message will be propagated,
						// but in the meanwhile we need to make a placeholder element in the array.
						var castItem = (item is T) ? (T)item : default(T);
						collection.Add(castItem);
					}
				}
			}

			[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
			public static void ReplaceDictionary(Type keyType, Type valueType, object dictionary, object newContents)
			{
				MethodInfo targetMethod = ReplaceDictionaryMethod.MakeGenericMethod(keyType, valueType);
				targetMethod.Invoke(null, new [] { dictionary, newContents });
			}

			private static void ReplaceDictionaryImpl<TKey, TValue>(IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<object, object>> newContents)
			{
				dictionary.Clear();
				foreach (KeyValuePair<object, object> item in newContents)
				{
					// if the item was not a T, some conversion failed. the error message will be propagated,
					// but in the meanwhile we need to make a placeholder element in the dictionary.
					var castKey = (TKey)item.Key; // this cast shouldn't fail
					var castValue = (item.Value is TValue) ? (TValue)item.Value : default(TValue);
					dictionary[castKey] = castValue;
				}
			}
		}
	}

	internal class EnsureStackHelper
	{
		private static readonly Action EnsureStackAction = InitializeEnsureStackDelegate();

		internal static void EnsureStack()
		{
			if (EnsureStackAction != null)
			{
				EnsureStackAction();
			}
		}

		private static Action InitializeEnsureStackDelegate()
		{
			try
			{
				// The following method will do a link demand, and because RuntimeHelpers.EnsureSufficientExecutionStack is marked 
				// SecurityCritical in 4.0 and moved to SecuritySafeCritical in 4.5. The following method will only fail in 4.0 partial trust
				var ensureStackAction = (Action)Delegate.CreateDelegate(typeof(Action), typeof(RuntimeHelpers), "EnsureSufficientExecutionStack");

				// Invoke the method just to be sure
				ensureStackAction();

				return ensureStackAction;
			}
			catch
			{
				return null;
			}
		}
	}

	}
