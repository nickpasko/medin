using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Db.Entities.Mocks;
using MedIn.Libs;
using MedIn.OziCms.PagesSettings.Forms;
using MedIn.OziCms.Services;
using MedIn.OziCms.ViewModels;
using Newtonsoft.Json.Linq;

namespace MedIn.OziCms.Controllers
{
	partial class GenericController<TEntity, TDbContext>
	{
		protected virtual UploadFileSettings GetFileSettings(string propName)
		{
			return (UploadFileSettings) Settings.FormSettings.Fields.FirstOrDefault(s => s.Name == propName);
		}

		[HttpPost]
		public virtual FineUploaderResult UploadFile(FineUpload upload, int id, string propName)
		{
			var settings = GetFileSettings(propName);
			if (upload.InputStream != null && upload.InputStream.Length != 0 && settings != null)
			{
				try
				{
					var entity = Repository.GetByPrimaryKey(id);
					var value = TypeHelpers.GetPropertyValue(entity, propName);

					Func<IFileEntity> saveImage = () =>
					{
						// надо создать новое value и добавить его в базу
						var t = TypeHelpers.GetPropertyType(entity, propName);
						var file = (IFileEntity)Activator.CreateInstance(t);

						// формируем пути
						var defaultPath = AppConfig.GetValue(AppConfig.BasePathForImages);
						var relativePath = settings.PathToSave ?? defaultPath;
						if (string.IsNullOrEmpty(relativePath))
						{
							throw new Exception("Не указан путь сохранения файла");
						}
						var path = Server.MapPath(relativePath);
						var filename = string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(upload.Filename));
						var fullname = Path.Combine(path, filename);

						// сохраняем файл
						if (settings.IsImage)
						{
							DefaultFileService.ResaveImage(upload, fullname, settings);
						}
						else
						{
							upload.SaveAs(fullname);
						}

						file.Name = Path.Combine(relativePath, filename);
						file.SourceName = upload.Filename;
						file.Date = DateTime.Now;
						file.Visibility = true;
						file.Size = new FileInfo(fullname).Length;
						var entitySetName = TypeHelpers.GetEntitySetName(file, Repository.DataContext);
						Repository.DataContext.AddObject(entitySetName, file);
						return file;
					};

					IFileEntity result;
					if (value is IEnumerable)
					{
						result = saveImage();
						var list = ((IListSource)value).GetList();
						var sortList = list.Cast<IFileEntity>().ToList();
						if (sortList.Any())
						{
							result.Sort = sortList.Max(fileEntity => fileEntity.Sort) + 1;
						}
						list.Add(result);
					}
					else
					{
						result = (IFileEntity)value;
						if (result != null)
						{
							DefaultFileService.DeleteFile(result, ControllerContext.HttpContext);
							Repository.DataContext.DeleteObject(result);
						}
						result = saveImage();
						TypeHelpers.SetPropertyValue(entity, propName, result);
					}
					Repository.Save();
					return new FineUploaderResult(true, new
					{
						result.Id,
						Url = Url.Content(result.Name),
						result.Alt,
						result.Title,
						result.Description,
						result.Visibility,
						result.SourceName
					});
				}
				catch (Exception exception)
				{
					return new FineUploaderResult(false, null, exception.Message);
				}
			}
			return new FineUploaderResult(false, null, "файл не был передан");
		}

		[HttpPost]
		public virtual ActionResult DeleteFile(CustomFileEntity model)
		{
			try
			{
				var db = Repository.DataContext;
				var entity = Repository.GetByPrimaryKey(model.ObjId);
				var type = TypeHelpers.GetPropertyType(entity, model.PropName);
				var entitySetName = TypeHelpers.GetEntitySetName(type, db);
				var key = new EntityKey(entitySetName, "Id", model.Id);
				var file = (IFileEntity)db.GetObjectByKey(key);

				DefaultFileService.DeleteFile(file, ControllerContext.HttpContext);
				var propValue = TypeHelpers.GetPropertyValue(entity, model.PropName);
				if (propValue is IEnumerable)
				{
					var list = ((IListSource)propValue).GetList();
					list.Remove(file);
				}
				else
				{
					TypeHelpers.SetPropertyValue(entity, model.PropName, null);
				}
				db.DeleteObject(file);
				db.SaveChanges();
				return Json(new { success = true });
			}
			catch (Exception exception)
			{
				return Json(new { success = false, error = exception.Message });
			}
		}

		[HttpPost]
		public virtual ActionResult SaveOrder(string model, int objId, string propName)
		{
			try
			{
				var index = 0;
				var obj = JObject.Parse(model);
				var list = (JArray)obj["items"];
				var db = Repository.DataContext;
				var entity = Repository.GetByPrimaryKey(objId);
				var prop = TypeHelpers.GetPropertyValue(entity, propName);
				var files = ((IEnumerable<IFileEntity>)prop).ToList();
				foreach (var item in list)
				{
					var fileId = item["Id"].Value<int>();
					var file = files.FirstOrDefault(f => f.Id == fileId);
					if (file != null)
					{
						index++;
						file.Sort = index;
					}
				}
				db.SaveChanges();
				return Json(new { success = true });
			}
			catch (Exception exception)
			{
				return Json(new { success = false, error = exception.Message });
			}
		}

		[HttpPost]
		public virtual ActionResult SaveFileData(CustomFileEntity model)
		{
			try
			{
				var db = Repository.DataContext;
				var entity = Repository.GetByPrimaryKey(model.ObjId);
				var type = TypeHelpers.GetPropertyType(entity, model.PropName);
				var entitySetName = TypeHelpers.GetEntitySetName(type, db);
				var key = new EntityKey(entitySetName, "Id", model.Id);
				var file = (IFileEntity)db.GetObjectByKey(key);
				file.Alt = model.Alt;
				file.Description = model.Description;
				file.Title = model.Title;
				file.Visibility = model.Visibility;
				db.SaveChanges();
				return Json(new { success = true });
			}
			catch (Exception exception)
			{
				return Json(new { success = false, error = exception.Message });
			}
		}


	}
}
