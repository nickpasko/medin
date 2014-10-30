using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MedIn.Libs;
using MedIn.OziCms.PagesSettings.Forms;
using MedIn.OziCms.PagesSettings.Lists;
using CheckboxSettings = MedIn.OziCms.PagesSettings.Lists.CheckboxSettings;
using CurrencySettings = MedIn.OziCms.PagesSettings.Lists.CurrencySettings;
using DateSettings = MedIn.OziCms.PagesSettings.Lists.DateSettings;
using ImageSettings = MedIn.OziCms.PagesSettings.Lists.ImageSettings;
using StringSettings = MedIn.OziCms.PagesSettings.Lists.StringSettings;

namespace MedIn.OziCms.PagesSettings
{
	public class Settings
	{
		private const string SettingsFolderPath = "~/Areas/Admin/Settings";

		private XDocument XDoc
		{
			get;
			set;
		}

		private ListSettings ListSettingsInternal
		{
			get;
			set;
		}

		public ListSettings ListSettings
		{
			get
			{
				if (ListSettingsInternal == null)
				{
					var settings = XDoc.Element("settings".ToXName());
					Debug.Assert(settings != null);
					ListSettingsInternal = (from listSettings in settings.Elements("list".ToXName())
											select new ListSettings
											{
												Levels = listSettings.GetBoolean("levels"),

												OrderAsc = listSettings.GetString("orderAsc"),
												OrderDesc = listSettings.GetString("orderDesc"),
												PageSize = listSettings.GetInt("pageSize"),
												SelectRowProperty = listSettings.GetString("selectRowProperty"),

												GlobalActions = ParseGlobalActions(listSettings),
												Cols = ParseColumnsSettings(listSettings),
												ListActions = ParseListActions(listSettings)

											}).Single();
				}
				return ListSettingsInternal;
			}
		}

		private FormSettings FormSettingsInternal
		{
			get;
			set;
		}

		public FormSettings FormSettings
		{
			get
			{
				if (FormSettingsInternal == null)
				{
					var settings = XDoc.Element("settings".ToXName());
					Debug.Assert(settings != null);
					FormSettingsInternal = settings.Elements("form".ToXName()).Select(formSettings => new FormSettings
					{
						Localizable = formSettings.GetNullableBoolean("localizable"),
						Tabs = formSettings.Elements("tab".ToXName()).Select(ParseFormTabs).ToList()
					}).Single();
				}
				return FormSettingsInternal;
			}
		}

		public Settings(string entityTypeName)
		{
			string path = System.Web.HttpContext.Current.Server.MapPath(Path.Combine(SettingsFolderPath, entityTypeName + ".xml"));
			//TODO отловить ошибки
			XDoc = XDocument.Load(path);
			XDoc.Root.Attributes("xmlns").Remove();
		}

		#region form fields parsers

		private static readonly Dictionary<XName, Func<XElement, FieldSettings>> FormFieldsParsers;

		static Settings()
		{
			FormFieldsParsers = new Dictionary<XName, Func<XElement, FieldSettings>>
				{
					{"collection", ParseFormCollection},
					{"image", ParseFormImage}, 
					{"static", ParseFormStatic}, 
					{"file", ParseFormFile}, 
					{"select", ParseFormSelect}, 
					{"wysiwyg", ParseFormWysiwyg}, 
					{"textarea", ParseFormTextarea}, 
					{"date", ParseFormDate}, 
					{"custom", ParseFormCustom}, 
					{"string", ParseFormString}, 
					{"currency", ParseFormCurrency}, 
					{"hidden", ParseFormHidden},
					{"checkbox", ParseFormCheckbox},
					{"code", ParseFormCode},
					{"hr", ParseFormHr}
				};
		}

		private static TabsSettings ParseFormTabs(XElement tab)
		{
			return new TabsSettings
			{
				Name = tab.GetString("name"),
				Sort = tab.GetInt("sort", 0),
				Fields = tab.Elements().Select(ParseFormField).ToList()
			};
		}

		private static FieldSettings ParseFormHr(XElement element)
		{
			return new FieldSettings { Control = "hr" };
		}

		private static FieldSettings ParseFormCheckbox(XElement element)
		{
			//TODO вообще-то есть шаблон boolean
			return new Forms.CheckboxSettings();
		}

		private static FieldSettings ParseFormHidden(XElement element)
		{
			return new HiddenSettings();
		}

		private static FieldSettings ParseFormCurrency(XElement element)
		{
			return new Forms.CurrencySettings { Format = element.GetString("format") };
		}

		private static FieldSettings ParseFormCode(XElement element)
		{
			return new CodeSettings { Type = element.GetString("type") };
		}

		private static FieldSettings ParseFormString(XElement element)
		{
			return new Forms.StringSettings
			{
				//TODO Не реализовано
				Maxlength = element.GetInt("maxlength", 0)
			};
		}

		private static FieldSettings ParseFormStatic(XElement element)
		{
			return new StaticSettings();
		}

		private static FieldSettings ParseFormCustom(XElement element)
		{
			return new FieldSettings { Control = element.Attribute("control").Value };
		}

		private static FieldSettings ParseFormDate(XElement element)
		{
			//TODO Format, а зачем?
			return new Forms.DateSettings();
		}

		private static FieldSettings ParseFormTextarea(XElement element)
		{
			return new TextareaSettings { Rows = element.GetInt("rows", 0) };
		}

		private static FieldSettings ParseFormWysiwyg(XElement element)
		{
			return new WysiwygSettings { Rows = element.GetInt("rows", 0) };
		}

		private static FieldSettings ParseFormSelect(XElement element)
		{
			return new SelectSettings
			{
				Data = element.GetString("data"),
				OptionTitle = element.GetString("optionTitle", "Name"),
				OptionValue = element.GetString("optionValue", "Id"),
				Multiple = element.GetBoolean("multiple"),
				Height = element.GetInt("height", 0),
				Options = element.Elements("option".ToXName()).Select(option => new OptionSettings
				{
					Value = option.GetString("value"),
					Title = option.Value
				}).ToList(),
				Levels = element.GetBoolean("levels"),
				Editable = element.GetBoolean("editable"),
				Reference = element.GetString("reference"),
				DependsOn = element.GetString("dependsOn"),
				TypeName = element.GetString("type"),
				PropertyName = element.GetString("property"),
				MethodName = element.GetString("method"),
			};
		}

		private static FieldSettings ParseFormCollection(XElement element)
		{
			var result = new CollectionSettings
			{
				Sortable = element.GetBoolean("sortable"),
				ShowDelete = element.GetBoolean("showDelete", true),
				ShowAdd = element.GetBoolean("showAdd", true),
				ItemTitle = element.GetString("itemTitle"),
				Fields = element.Elements().Select(ParseFormField).ToList()
			};
			result.Fields.ForEach(settings => settings.Parent = result); // присваиваем парента
			return result;
		}

		private static FieldSettings ParseFormFile(XElement element)
		{
			return new UploadFileSettings
			{
				PathToSave = element.GetString("path"),
				IsImage = element.GetBoolean("isImage", true),
				ShowTitle = element.GetBoolean("showTitle"),
				ShowSourceName = element.GetBoolean("showSourceName"),
				Sortable = element.GetBoolean("sortable"),
				Visibility = element.GetBoolean("visibility"),
				ShowDescription = element.GetBoolean("showDescription"),
				DisplayType = element.GetString("display"),
				ShowDate = element.GetBoolean("showDate"),
				Size = element.GetInt("size", 0),
				Watermarks = element.Elements("watermark".ToXName()).Select(e => new WatermarkSettings
				{
					Left = e.GetInt("left", 0),
					Top = e.GetInt("top", 0),
					Width = e.GetInt("width", 0),
					Height = e.GetInt("height", 0),
					ReduceWidth = e.GetInt("reduceWidth"),
					ReduceHeight = e.GetInt("reduceHeight"),
					ReduceFactor = e.GetDouble("reduceFactor", 1.0),
					Margins = e.GetMargins("margins"),
					//Opacity = e.GetDouble("opacity", 1.0),
					FillType = (WatermarkFillType)Enum.Parse(typeof(WatermarkFillType), e.GetString("filltype")),
					RelativePath = e.GetString("path")
				}).ToList()
			};
		}

		private static FieldSettings ParseFormImage(XElement element)
		{
			return new Forms.ImageSettings
			{
				Width = element.GetInt("width", 0),
				Height = element.GetInt("height", 0),
				Proportional = element.GetBoolean("proportional", true),
				Alt = element.GetString("alt"),
				Reference = element.GetString("reference"),
				SimpleForm = element.GetBoolean("simple")
			};
		}

		private static FieldSettings ParseFormField(XElement element)
		{
			var func = FormFieldsParsers[element.Name.LocalName];
			if (func == null)
				return null;
			return ParseBaseFormFields(element, func(element));
		}

		private static FieldSettings ParseBaseFormFields(XElement element, FieldSettings field)
		{
			field.RoleName = element.GetString("rolename");
			field.Name = element.GetString("name");
			field.Title = element.GetString("title");
			field.Info = element.GetString("info");
			field.Condition = element.GetString("condition");
			field.Disabled = element.GetBoolean("disabled");
			field.SkipTitle = element.GetBoolean("skipTitle");
			field.Readonly = element.GetBoolean("readonly");
			field.PreValue = element.GetBoolean("prevalue");
			field.Localizable = element.GetBoolean("localizable");
			return field;
		}

		#endregion

		#region target parsers

		private static List<ListActionSettings> ParseListActions(XElement element)
		{
			Debug.Assert(element != null);
			var listActions = element.Element("actions".ToXName());
			Debug.Assert(listActions != null);
			return listActions.Elements().Select(listAction =>
			{
				switch (listAction.Name.LocalName)
				{
					case "sort":
						return new SortActionSettings();
					case "visibility":
						return new VisibilityActionSettings();
					case "edit":
						return new ListActionSettings { Control = "edit" };
					case "delete":
						return new ListActionSettings { Control = "delete" };
				}
				return null;

			}).ToList();
		}

		private static List<ColSettings> ParseColumnsSettings(XElement element)
		{
			Debug.Assert(element != null);
			var cols = element.Element("cols".ToXName());
			Debug.Assert(cols != null);
			return cols.Elements().Select(field =>
			{

				ColSettings column;

				switch (field.Name.LocalName)
				{
					case "custom":
						column = new CustomSettings { Control = field.GetString("control") };
						break;
					case "date":
						column = new DateSettings { Format = field.GetString("format") };
						break;
					case "string":
						column = new StringSettings { Maxlength = field.GetInt("maxlength", 0) };
						break;
					case "currency":
						column = new CurrencySettings { Format = field.GetString("format") };
						break;
					case "image":
						column = new ImageSettings { ImageWidth = field.GetString("imageWidth") };
						break;
					case "checkbox":
						column = new CheckboxSettings();
						break;
					case "filter":
						column = new FilterSettings { FilterField = field.GetString("field") };
						break;
					default:
						return null;
				}

				column.RoleName = field.GetString("rolename");
				column.Name = field.GetString("name");
				column.Title = field.GetString("title");
				column.Width = field.GetInt("width", 0);
				column.Levels = field.GetBoolean("levels");
				column.Localizable = field.GetBoolean("localizable");
				column.Order = field.GetString("order");
				column.Filter = field.GetString("filter");
				column.HeaderControl = "ozi_Header";
				return column;
			}).ToList();
		}

		private static List<GlobalActionSettings> ParseGlobalActions(XElement element)
		{
			Debug.Assert(element != null);
			var globalActions = element.Element("global".ToXName());
			Debug.Assert(globalActions != null);
			return globalActions.Elements().Select(field =>
			{
				if (field.Name.LocalName == "create")
				{
					return new CreateGlobalActionSettings();
				}
				if (field.Name.LocalName == "localize")
				{
					return new LocalizeGlobalActionSettingss();
				}
				if (field.Name.LocalName == "custom")
				{
					return new GlobalActionSettings { Control = field.GetString("control"), Action = field.GetString("action"), Text = field.GetString("text") };
				}
				return null;
			}).ToList();
		}

		#endregion

		public FieldSettings GetFormSettingsItem(string propName)
		{
			if (propName.Contains("."))
			{
				var props = propName.Split(new[] { '.' });
				return GetFormSettingsItemRecursive(props, 0, FormSettings.Fields);
			}
			return FormSettings.Fields.FirstOrDefault(settings => settings.Name == propName);
		}

		private FieldSettings GetFormSettingsItemRecursive(string[] props, int index, IEnumerable<FieldSettings> fields)
		{
			var field = fields.FirstOrDefault(f => f.Name == props[index]);
			if (field is CollectionSettings && props.Length > index + 1)
			{
				var result = GetFormSettingsItemRecursive(props, index + 1, ((CollectionSettings)field).Fields);
				return result;
			}
			return field;
		}
	}
}