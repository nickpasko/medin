using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Db.Infrastructure;
using MedIn.Libs;
using MedIn.Libs.Services;
using MedIn.OziCms.Helpers;
using MedIn.OziCms.Infrastructure;
using MedIn.OziCms.Memberships;
using MedIn.OziCms.Mvc;
using MedIn.OziCms.PagesSettings;
using MedIn.OziCms.PagesSettings.Forms;
using MedIn.OziCms.PagesSettings.Lists;
using MedIn.OziCms.Repositories;
using MedIn.OziCms.Services;
using MedIn.OziCms.ViewModels;
using CurrencySettings = MedIn.OziCms.PagesSettings.Forms.CurrencySettings;

namespace MedIn.OziCms.Controllers
{
	[ValidateInput(false)]
	[OziAuthorize]
	public abstract partial class GenericController<TEntity, TDbContext> : OziController
		where TEntity : class, IEntity, new()
		where TDbContext : ObjectContext, new()
	{

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			WebContext.GetSettings(GetType());
			WebContext.ModelType = typeof(TEntity);
		}

		public AdminWebContext WebContext
		{
			get { return DependencyResolver.Current.GetService<AdminWebContext>(); }
		}

		#region properties

		protected Settings Settings
		{
			get { return WebContext.GetSettings(); }
		}
		
		private IRepository<TEntity, TDbContext> _repository;
		public IRepository<TEntity, TDbContext> Repository
		{
			get { return _repository ?? (_repository = new GenericRepository<TEntity, TDbContext>()); }
		}

		public override ObjectContext DataModelContext
		{
			get { return Repository.DataContext; }
		}

		#endregion

		#region protected properties

		protected virtual string IndexViewName
		{
			get
			{
				return Constants.IndexView;
			}
		}

		protected virtual string DetailsViewName
		{
			get
			{
				return Constants.DetailsView;
			}
		}

		protected virtual string EditViewName
		{
			get
			{
				return Constants.EditView;
			}
		}

		protected virtual string CreateViewName
		{
			get
			{
				return Constants.CreateView;
			}
		}

		#endregion

		#region CRUD Actions

		public virtual ActionResult Index(int? page, string order, string desc)
		{
			var indexList = GetIndexEntities().ToList();

			indexList = InitFiltersAndFilterList(indexList);
			indexList = OrderList(indexList, order, desc);
			var itemsCount = indexList.Count;
			indexList = BrakeIntoPagesList(page, itemsCount, indexList);
			if (Settings.ListSettings.Levels)
			{
				indexList = PlainTree.GetTree(indexList);
			}
			var filterParameters = new FilterParameters
			{
				ItemsCount = itemsCount,
				Page = page,
				PageSize = Settings.ListSettings.PageSize
			};

			return View(IndexViewName, GetIndexViewModel(indexList, filterParameters));
		}

		public virtual ActionResult Details(int id)
		{
			var entity = Repository.GetByPrimaryKey(id);
			return View(DetailsViewName, GetDetailsViewModel(entity));
		}

		public virtual ActionResult Create()
		{
			WebContext.IsCreate = true;
			var entity = new TEntity();
			Repository.AddObject(entity);
			PrepareEntity(entity);
			return View(CreateViewName, entity);
		}

		public virtual ActionResult Edit(int id)
		{
			var entity = Repository.GetByPrimaryKey(id);
			if (entity == null)
			{
				return ToIndex();
			}
			WebContext.Model = entity;

			var list = GetIndexEntities().ToList();
			if (Settings.ListSettings.Levels)
			{
				list = PlainTree.GetTree(list);
			}
			var prev = list.TakeWhile(e => e.Id != id).LastOrDefault();
			WebContext.PrevId = prev == null ? null : (int?) prev.Id;
			var next = list.SkipWhile(e => e.Id != id).Skip(1).Take(1).FirstOrDefault();
			WebContext.NextId = next == null ? null : (int?)next.Id;

			PrepareEntity(entity);
			return View(EditViewName, entity);
		}

		[NonAction]
		protected virtual TEntity CloneEntity(int from, int to)
		{
			var fromEntity = Repository.GetByPrimaryKey(from);
			if (fromEntity == null)
				return null;
			TEntity toEntity;
			if (to == 0)
			{
				toEntity = new TEntity();
				Repository.AddObject(toEntity);
				PopulateProperties(fromEntity, toEntity);
			}
			else
			{
				toEntity = Repository.GetByPrimaryKey(to);
				if (toEntity != null)
				{
					PopulateProperties(fromEntity, toEntity);
				}
			}
			return toEntity;
		}

		protected virtual void PopulateProperties(TEntity from, TEntity to)
		{
			Repository.Save();
		}

		public virtual ActionResult Delete(int id)
		{
			var entity = Repository.GetByPrimaryKey(id);
			try
			{
				// удаляем файлы
				foreach (var field in Settings.FormSettings.Fields.Where(settings => settings is UploadFileSettings))
				{
					var value = TypeHelpers.GetPropertyValue(entity, field.Name);
					if (value is IEnumerable)
					{
						var list = ((IListSource) value).GetList();
						foreach (var file in list.Cast<IFileEntity>().ToList())
						{
							DefaultFileService.DeleteFile(file, HttpContext);
							Repository.DataContext.DeleteObject(file);
						}
						list.Clear();
					}
					else if (value != null)
					{
						DefaultFileService.DeleteFile((IFileEntity)value, HttpContext);
						Repository.DataContext.DeleteObject(value);
					}
				}
				// удаляем элемент
				Repository.Delete(entity);
			}
			catch (Exception e)
			{
				Logger.Instance.LogException(e);
			}

			return ToIndex();
		}

		private bool IsClone(FormCollection collection, TEntity to)
		{
			if (collection.GetValue(FieldsNames.CloneButton) != null)
			{
				try
				{
					var from = (int) collection.GetValue(FieldsNames.PrototypeSelect).ConvertTo(typeof (int));
					var result = CloneEntity(from, to.Id);
					if (result != null)
						to.Id = result.Id;
				}
				catch
				{
				}
				return true;
			}
			return false;
		}

		[HttpPost]
		public virtual ActionResult Create(FormCollection collection)
		{
			var entity = new TEntity();
			if (!IsClone(collection, entity))
			{
				Repository.AddObject(entity);
				return Edit(entity, collection, CreateViewName);
			}
			return Edit(entity.Id);
		}

		[HttpPost]
		public virtual ActionResult Edit(int id, FormCollection collection)
		{
			var entity = Repository.GetByPrimaryKey(id);
			if (!IsClone(collection, entity))
			{
				WebContext.Model = entity;
				return Edit(entity, collection, EditViewName);
			}
			return Back();
		}

		[NonAction]
		public virtual ActionResult Edit(TEntity entity, FormCollection collection, string editViewName)
		{
			OnEntityEditing(entity, collection);
			SaveFormPrerequisites(collection, entity);

			UpdatePartialLocalization(entity, collection);

			UpdateCurrencies(entity, collection);

			//if (Binders.GetBinder(typeof (TEntity)).GetType() == typeof(DefaultModelBinder))
			//{
			//	Binders.Add(typeof(TEntity), new OziModelBinder());
			//}
		    
                TryUpdateModel(entity, collection);
			//var bindingContext = new ModelBindingContext
			//{
			//	ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => entity, typeof(TEntity)),
			//	ModelName = null,
			//	ModelState = ModelState,
			//	PropertyFilter = null,
			//	ValueProvider = ValueProvider
			//};
			//new OziModelBinder().BindModel(ControllerContext, bindingContext);

			UpdateAlias(entity, collection);
			UpdateCollections(entity, collection);

			ActionResult result;

			if (ModelState.IsValid)
			{
				OnEntityEdited(entity, collection);

				Repository.Save();

				OnEditComplete(entity);
				result = collection.GetValue(FieldsNames.SaveButton) != null ?
							RedirectToAction(IndexViewName) :
							RedirectToAction(EditViewName, new { id = entity.Id });
			}
			else
			{
				WebContext.EditViewName = editViewName;
				PrepareEntity(entity);
				return View(editViewName, entity);
			}

			var returnUrl = WebContext.ReturnUrl;
			if (string.IsNullOrEmpty(returnUrl))
			{
				return result;
			}
			if (collection.GetValue(FieldsNames.SaveButton) != null)
				return Redirect(returnUrl);

			return RedirectToAction(EditViewName, new { id = entity.Id, returnUrl = returnUrl });
		}

		private void UpdateCurrencies(TEntity entity, FormCollection collection)
		{
			foreach (var field in Settings.FormSettings.Fields.Where(settings => settings is CurrencySettings).Cast<CurrencySettings>())
			{
				var value = collection[field.Name];
				if (!string.IsNullOrEmpty(value))
				{
					var d = decimal.Parse(value, NumberStyles.Currency);
					TypeHelpers.SetPropertyValue(entity, field.Name, d);
					collection.Remove(field.Name);
				}
			}
		}

		#endregion

		#region List actions

		public virtual ActionResult SetVisibility(int id, bool visibilityToSet)
		{
			var entity = (IVisibleEntity)Repository.GetByPrimaryKey(id);
			entity.Visibility = visibilityToSet;
			Repository.Save();
			return Back();
		}

		[HttpPost]
		public virtual ActionResult SortList(int? page, FormCollection collection)
		{
			var sortList = new IndexSortData { SortListItems = new List<SortListItem>() };
			TryUpdateModel(sortList.SortListItems, FieldsNames.SortListPrefix);

			var list = Repository.All();

			var originalList = list.ToList().Cast<ISortableEntity>().ToList();
			foreach (var entity in originalList)
			{
				var elem = sortList.SortListItems.Find(item => item.Id == entity.Id);
				if (elem == null) continue;
				var sort = elem.Sort;
				if (sort.HasValue)
				{
					entity.Sort = sort.Value;
				}
			}

			//вложенный список
			if (Settings.ListSettings.Levels)
			{
				ApplyNestedSort(originalList.Cast<INestedEntity<TEntity>>().ToList());
			}
			//обычный, не вложенный список
			else
			{
				ApplyPlainSort(originalList);
			}
			Repository.Save();
			return RedirectToAction(IndexViewName, new { page });
		}

		private static void ApplyPlainSort(IEnumerable<ISortableEntity> source)
		{
			var sortedList = source.OrderBy(entity => entity.Sort).ToList();
			var sort = 10;
			foreach (var entity in sortedList)
			{
				entity.Sort = sort;
				sort += 10;
			}
		}

		private static void ApplyNestedSort(IEnumerable<INestedEntity<TEntity>> source, int? parentId = null)
		{
			var entities = source as IList<INestedEntity<TEntity>> ?? source.ToList();
			var children = entities.Where(entity => entity.ParentId == parentId).Cast<ISortableEntity>().OrderBy(entity => entity.Sort).ToList();
			var sort = 10;
			foreach (var child in children)
			{
				child.Sort = sort;
				ApplyNestedSort(entities, child.Id);
				sort += 10;
			}
		}

		#endregion

		#region default actions

		[NonAction]
		protected override ActionResult ToIndex()
		{
			return RedirectToAction(IndexViewName);
		}

		#endregion

		#region events

		protected virtual void OnEntityEditing(TEntity entity, FormCollection collection) { }

		protected virtual void OnEntityEdited(TEntity entity, FormCollection collection) { }

		protected virtual void OnEditComplete(TEntity entity)
		{
		}

		#endregion

		#region protected & virtual logic

		protected virtual dynamic GetIndexViewModel(IEnumerable<TEntity> objectsList, FilterParameters filterParameters)
		{
			return new ListViewModel
			{
				Settings = Settings,
				ListData = objectsList,
				FilterParameters = filterParameters
			};
		}

		protected virtual dynamic GetDetailsViewModel(TEntity entity)
		{
			return new EditViewModel
			{
				Settings = Settings,
				FormData = entity
			};
		}

		//protected virtual dynamic GetEditViewModel(TEntity entity)
		//{
		//	return new EditViewModel
		//	{
		//		Settings = Settings,
		//		FormData = entity
		//	};
		//}

		protected virtual IEnumerable<TEntity> GetIndexEntities()
		{
			IEnumerable<TEntity> result = Repository.All();
			return result;
		}

		protected virtual void PrepareEntity(TEntity entity)
		{
		}

		#endregion

		#region logic


		private void UpdatePartialLocalization(TEntity entity, FormCollection collection)
		{
			var l = DependencyResolver.Current.GetService<ILocalizationDbHelper>();
			var lang = DependencyResolver.Current.GetService<ILocalizationProvider>();
			var fields = Settings.FormSettings.Fields;
			foreach (var f in fields.Where(settings => settings.Localizable))
			{
				try
				{
					var value = collection[f.Name];
					var key = (Guid?)TypeHelpers.GetPropertyValue(entity, f.Name);
					if (key == null || key.Value == Guid.Empty)
					{
						if (string.IsNullOrEmpty(f.Name))
							continue;
						key = Guid.NewGuid();
						TypeHelpers.SetPropertyValue(entity, f.Name, key);
					}
					l.Set(lang.GetLanguageName(), key.Value, value);
					lang.Reset();
					collection.Remove(f.Name);
				}
				catch
				{
				}
			}
			var unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();
			unitOfWork.Commit();
		}

		private void SaveFormPrerequisites(FormCollection collection, TEntity entity)
		{
			foreach (var fieldSettings in Settings.FormSettings.Fields.Where(fieldSettings => fieldSettings.PreValue))
			{
				Session[string.Format("prevalue_{0}_{1}", fieldSettings.Name, entity.GetType().BaseType.Name)] = collection[fieldSettings.Name];
			}
		}

		//private void UpdateFiles(TEntity entity)
		//{
		//	var files = new DefaultFileService();
		//	foreach (var fieldSettings in Settings.FormSettings.Fields.Where(f => f.GetType() == typeof(PagesSettings.Forms.UploadFileSettings)).Cast<PagesSettings.Forms.UploadFileSettings>())
		//	{
		//		files.SaveFiles(Repository.DataContext, ControllerContext, entity, fieldSettings);
		//	}
		//}

		//private void UpdateImages(TEntity entity)
		//{
		//	var images = DependencyResolver.Current.GetService<IEntityImageService>();
		//	foreach (var fieldSettings in Settings.FormSettings.Fields.Where(f => f.GetType() == typeof(PagesSettings.Forms.ImageSettings)).Cast<PagesSettings.Forms.ImageSettings>())
		//	{
		//		images.SaveImage(Repository.DataContext, ControllerContext, entity, fieldSettings);
		//	}
		//}

		private List<TEntity> InitFiltersAndFilterList(List<TEntity> indexList)
		{
			foreach (var column in Settings.ListSettings.Cols.Where(settings => settings is FilterSettings).Cast<FilterSettings>().ToList())
			{
				var filterValue = Request.QueryString[column.FilterField];
				if (!string.IsNullOrEmpty(filterValue))
				{
					indexList = indexList.Where(delegate(TEntity i)
					{
						var value = TypeHelpers.GetPropertyValue(i, column.FilterField);
						return (value != null) && value.ToString().Equals(filterValue);
					}).ToList();
				}
				column.FilterValues = Repository.All().ToList().Select(e => (IEntity)TypeHelpers.GetPropertyValue(e, column.Name)).Distinct().ToList();
			}

			foreach (var column in Settings.ListSettings.Cols.Where(settings => settings.GetFilterField() != null))
			{
				var filterField = column.GetFilterField();
				var filterValue = Request.QueryString[filterField];
				if (!string.IsNullOrEmpty(filterValue))
				{
					indexList = indexList.Where(delegate(TEntity i)
					{
						var value = TypeHelpers.GetPropertyValue(i, filterField);
						return (value != null) && value.ToString().ToLowerInvariant().Contains(filterValue.ToLowerInvariant());
					}).ToList();
				}
			}

			return indexList;
		}

		private List<TEntity> BrakeIntoPagesList(int? page, int itemsCount, List<TEntity> indexList)
		{
			if (Settings.ListSettings.PageSize.HasValue)
			{
				var p = page.HasValue ? page.Value - 1 : 0;
				var pageCount = itemsCount / Settings.ListSettings.PageSize.Value;
				var skip = Math.Min(p, pageCount) * Settings.ListSettings.PageSize.Value;
				indexList = indexList.Skip(skip).Take(Settings.ListSettings.PageSize.Value).ToList();
			}
			return indexList;
		}

		private List<TEntity> OrderList(List<TEntity> indexList, string order, string desc)
		{
			var orderField = order;

			Func<TEntity, object> globalSortAsc = null;
			Func<TEntity, object> globalSortDesc = null;

			var hasAsc = Settings.ListSettings.OrderAsc != null && Settings.ListSettings.OrderAsc != orderField;
			var hasDesc = Settings.ListSettings.OrderDesc != null && Settings.ListSettings.OrderDesc != orderField;

			if (hasAsc || hasDesc)
			{
				if (Settings.ListSettings.OrderAsc != null)
				{
					globalSortAsc = (item => TypeHelpers.GetPropertyValue(item, Settings.ListSettings.OrderAsc));
				}
				else if (Settings.ListSettings.OrderDesc != null)
				{
					globalSortDesc = (item => TypeHelpers.GetPropertyValue(item, Settings.ListSettings.OrderDesc));
				}
			}

			if (!string.IsNullOrEmpty(orderField))
			{
				var list = desc != null ?
							   indexList.OrderByDescending(entity => TypeHelpers.GetPropertyValue(entity, orderField)) :
							   indexList.OrderBy(entity => TypeHelpers.GetPropertyValue(entity, orderField));
				if (globalSortAsc != null)
				{
					list = list.ThenBy(globalSortAsc);
				}
				else if (globalSortDesc != null)
				{
					list = list.ThenByDescending(globalSortDesc);
				}
				indexList = list.ToList();
			}
			else
			{
				if (globalSortAsc != null)
				{
					indexList = indexList.OrderBy(globalSortAsc).ToList();
				}
				else if (globalSortDesc != null)
				{
					indexList = indexList.OrderByDescending(globalSortDesc).ToList();
				}
			}
			return indexList;
		}

		private static void UpdateAlias(IHaveAliasEntity entity)
		{
			if (string.IsNullOrEmpty(entity.Alias))
			{
				entity.Alias = entity.Name.ToLower().Transliterate();
			}
		}

		private void UpdateAlias(TEntity entity, FormCollection collection)
		{
			if (!(entity is IHaveAliasEntity))
				return;
			var aliasable = entity as IHaveAliasEntity;
			var name = aliasable.Name;
			Guid key;
			if (Guid.TryParse(name, out key))
			{
				var l = DependencyResolver.Current.GetService<ILocalizationDbHelper>();
				var lang = DependencyResolver.Current.GetService<ILocalizationProvider>();
				name = l.Get(lang.GetLanguageName(), key);
			}
			if (string.IsNullOrWhiteSpace(collection["Alias"]) && !string.IsNullOrWhiteSpace(name))
			{
				var path = name.ToLower().Transliterate();
				aliasable.Alias = path;
			}
			var result = aliasable.Alias.Transliterate();
			//var index = 1;
			//var list = Repository.All().ToList().Cast<IHaveAliasEntity>().ToList();
			//var other = list.FirstOrDefault(e => e.Alias == result);
			/*while (other != null && other.Id != entity.Id)
			{
				result = string.Format("{0}_{1}", collection[FieldsNames.Alias], index++);
				other = list.FirstOrDefault(e => e.Alias == result);
			}*/
			aliasable.Alias = result.ToLower();
			ModelState.Remove(FieldsNames.Alias);
		}

		#endregion

		#region nested types

		public class IndexSortData
		{
			public string Field
			{
				get;
				set;
			}
			public List<SortListItem> SortListItems
			{
				get;
				set;
			}
		}

		public class SortListItem
		{
			public int Id
			{
				get;
				set;
			}
			public int? Sort
			{
				get;
				set;
			}
		}

		#endregion
	}
}


