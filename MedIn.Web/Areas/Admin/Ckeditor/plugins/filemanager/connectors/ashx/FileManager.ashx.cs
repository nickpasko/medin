using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MedIn.Web.Areas.Admin.Ckeditor.plugins.filemanager.connectors.ashx
{
	/// <summary>
	/// Summary description for FileManager
	/// </summary>
	public class FileManager : IHttpHandler
	{
		//===================================================================
		//==================== EDIT CONFIGURE HERE ==========================
		//===================================================================

		// \Areas\Admin\Ckeditor\plugins\filemanager\scripts\filemanager.config.js
		public string IconDirectory = "/content/w/fileicons/"; // Icon directory for filemanager. [string]
		public string[] ImgExtensions = new[] { ".jpg", ".png", ".jpeg", ".gif", ".bmp" }; // Only allow this image extensions. [string]

		//===================================================================
		//========================== END EDIT ===============================
		//===================================================================       


		private bool IsImage(FileInfo fileInfo)
		{
			return ImgExtensions.Any(ext => ext.Equals(Path.GetExtension(fileInfo.FullName), StringComparison.OrdinalIgnoreCase));
		}


		private string GetFolderInfo(string path)
		{
			var rootDirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
			var sb = new StringBuilder();

			sb.AppendLine("{");

			var i = 0;

			foreach (var dirInfo in rootDirInfo.GetDirectories())
			{
				if (i > 0)
				{
					sb.Append(",");
					sb.AppendLine();
				}

				sb.AppendLine("\"" + Path.Combine(path, dirInfo.Name) + "\": {");
				sb.AppendLine("\"Path\": \"" + Path.Combine(path, dirInfo.Name) + "/\",");
				sb.AppendLine("\"Filename\": \"" + dirInfo.Name + "\",");
				sb.AppendLine("\"File Type\": \"dir\",");
				sb.AppendLine("\"Preview\": \"" + IconDirectory + "_Open.png\",");
				sb.AppendLine("\"Properties\": {");
				sb.AppendLine("\"Date Created\": \"" + dirInfo.CreationTime.ToString(CultureInfo.InvariantCulture) + "\", ");
				sb.AppendLine("\"Date Modified\": \"" + dirInfo.LastWriteTime.ToString(CultureInfo.InvariantCulture) + "\", ");
				sb.AppendLine("\"Height\": 0,");
				sb.AppendLine("\"Width\": 0,");
				sb.AppendLine("\"Size\": 0 ");
				sb.AppendLine("},");
				sb.AppendLine("\"Error\": \"\",");
				sb.AppendLine("\"Code\": 0	");
				sb.Append("}");

				i++;
			}

			foreach (FileInfo fileInfo in rootDirInfo.GetFiles())
			{
				if (i > 0)
				{
					sb.Append(",");
					sb.AppendLine();
				}

				sb.AppendLine("\"" + Path.Combine(path, fileInfo.Name) + "\": {");
				sb.AppendLine("\"Path\": \"" + Path.Combine(path, fileInfo.Name) + "\",");
				sb.AppendLine("\"Filename\": \"" + fileInfo.Name + "\",");
				sb.AppendLine("\"File Type\": \"" + fileInfo.Extension.Replace(".", "") + "\",");

				if (IsImage(fileInfo))
				{
					sb.AppendLine("\"Preview\": \"" + Path.Combine(path, fileInfo.Name) + "\",");
				}
				else
				{
					sb.AppendLine("\"Preview\": \"" + String.Format("{0}{1}.png", IconDirectory, fileInfo.Extension.Replace(".", "")) + "\",");
				}

				sb.AppendLine("\"Properties\": {");
				sb.AppendLine("\"Date Created\": \"" + fileInfo.CreationTime.ToString(CultureInfo.InvariantCulture) + "\", ");
				sb.AppendLine("\"Date Modified\": \"" + fileInfo.LastWriteTime.ToString(CultureInfo.InvariantCulture) + "\", ");

				if (IsImage(fileInfo))
				{
					using (System.Drawing.Image img = System.Drawing.Image.FromFile(fileInfo.FullName))
					{
						sb.AppendLine("\"Height\": " + img.Height.ToString(CultureInfo.InvariantCulture) + ",");
						sb.AppendLine("\"Width\": " + img.Width.ToString(CultureInfo.InvariantCulture) + ",");
					}
				}

				sb.AppendLine("\"Size\": " + fileInfo.Length.ToString(CultureInfo.InvariantCulture) + " ");
				sb.AppendLine("},");
				sb.AppendLine("\"Error\": \"\",");
				sb.AppendLine("\"Code\": 0	");
				sb.Append("}");

				i++;
			}

			sb.AppendLine();
			sb.AppendLine("}");

			return sb.ToString();
		}

		private string GetInfo(string path)
		{
			var sb = new StringBuilder();

			var attr = File.GetAttributes(HttpContext.Current.Server.MapPath(path));

			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
			{
				var dirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));

				sb.AppendLine("{");
				sb.AppendLine("\"Path\": \"" + path + "\",");
				sb.AppendLine("\"Filename\": \"" + dirInfo.Name + "\",");
				sb.AppendLine("\"File Type\": \"dir\",");
				sb.AppendLine("\"Preview\": \"" + IconDirectory + "_Open.png\",");
				sb.AppendLine("\"Properties\": {");
				sb.AppendLine("\"Date Created\": \"" + dirInfo.CreationTime.ToString(CultureInfo.InvariantCulture) + "\", ");
				sb.AppendLine("\"Date Modified\": \"" + dirInfo.LastWriteTime.ToString(CultureInfo.InvariantCulture) + "\", ");
				sb.AppendLine("\"Height\": 0,");
				sb.AppendLine("\"Width\": 0,");
				sb.AppendLine("\"Size\": 0 ");
				sb.AppendLine("},");
				sb.AppendLine("\"Error\": \"\",");
				sb.AppendLine("\"Code\": 0	");
				sb.AppendLine("}");
			}
			else
			{
				var fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(path));

				sb.AppendLine("{");
				sb.AppendLine("\"Path\": \"" + path + "\",");
				sb.AppendLine("\"Filename\": \"" + fileInfo.Name + "\",");
				sb.AppendLine("\"File Type\": \"" + fileInfo.Extension.Replace(".", "") + "\",");

				if (IsImage(fileInfo))
				{
					sb.AppendLine("\"Preview\": \"" + path + "\",");
				}
				else
				{
					sb.AppendLine("\"Preview\": \"" + String.Format("{0}{1}.png", IconDirectory, fileInfo.Extension.Replace(".", "")) + "\",");
				}

				sb.AppendLine("\"Properties\": {");
				sb.AppendLine("\"Date Created\": \"" + fileInfo.CreationTime.ToString(CultureInfo.InvariantCulture) + "\", ");
				sb.AppendLine("\"Date Modified\": \"" + fileInfo.LastWriteTime.ToString(CultureInfo.InvariantCulture) + "\", ");

				if (IsImage(fileInfo))
				{
					using (System.Drawing.Image img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(path)))
					{
						sb.AppendLine("\"Height\": " + img.Height.ToString(CultureInfo.InvariantCulture) + ",");
						sb.AppendLine("\"Width\": " + img.Width.ToString(CultureInfo.InvariantCulture) + ",");
					}
				}

				sb.AppendLine("\"Size\": " + fileInfo.Length.ToString(CultureInfo.InvariantCulture) + " ");
				sb.AppendLine("},");
				sb.AppendLine("\"Error\": \"\",");
				sb.AppendLine("\"Code\": 0	");
				sb.AppendLine("}");
			}

			return sb.ToString();

		}

		private string Rename(string path, string newName)
		{
			var attr = File.GetAttributes(HttpContext.Current.Server.MapPath(path));

			var sb = new StringBuilder();

			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
			{
				var dirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
				Debug.Assert(dirInfo.Parent != null);
				Directory.Move(HttpContext.Current.Server.MapPath(path), Path.Combine(dirInfo.Parent.FullName, newName));

				var fileInfo2 = new DirectoryInfo(Path.Combine(dirInfo.Parent.FullName, newName));

				sb.AppendLine("{");
				sb.AppendLine("\"Error\": \"No error\",");
				sb.AppendLine("\"Code\": 0,");
				sb.AppendLine("\"Old Path\": \"" + path + "\",");
				sb.AppendLine("\"Old Name\": \"" + newName + "\",");
				sb.AppendLine("\"New Path\": \"" +
					fileInfo2.FullName.Replace(HttpRuntime.AppDomainAppPath, "/").Replace(Path.DirectorySeparatorChar, '/') + "\",");
				sb.AppendLine("\"New Name\": \"" + fileInfo2.Name + "\"");
				sb.AppendLine("}");

			}
			else
			{
				var fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(path));
				Debug.Assert(fileInfo.Directory != null);
				File.Move(HttpContext.Current.Server.MapPath(path), Path.Combine(fileInfo.Directory.FullName, newName));

				var fileInfo2 = new FileInfo(Path.Combine(fileInfo.Directory.FullName, newName));

				sb.AppendLine("{");
				sb.AppendLine("\"Error\": \"No error\",");
				sb.AppendLine("\"Code\": 0,");
				sb.AppendLine("\"Old Path\": \"" + path + "\",");
				sb.AppendLine("\"Old Name\": \"" + newName + "\",");
				sb.AppendLine("\"New Path\": \"" +
					fileInfo2.FullName.Replace(HttpRuntime.AppDomainAppPath, "/").Replace(Path.DirectorySeparatorChar, '/') + "\",");
				sb.AppendLine("\"New Name\": \"" + fileInfo2.Name + "\"");
				sb.AppendLine("}");
			}

			return sb.ToString();
		}

		private string Delete(string path)
		{
			var attr = File.GetAttributes(HttpContext.Current.Server.MapPath(path));

			var sb = new StringBuilder();

			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
			{
				Directory.Delete(HttpContext.Current.Server.MapPath(path), true);
			}
			else
			{
				File.Delete(HttpContext.Current.Server.MapPath(path));
			}

			sb.AppendLine("{");
			sb.AppendLine("\"Error\": \"No error\",");
			sb.AppendLine("\"Code\": 0,");
			sb.AppendLine("\"Path\": \"" + path + "\"");
			sb.AppendLine("}");

			return sb.ToString();
		}

		private static string AddFolder(string path, string newFolder)
		{
			var sb = new StringBuilder();

			Directory.CreateDirectory(Path.Combine(HttpContext.Current.Server.MapPath(path), newFolder));

			sb.AppendLine("{");
			sb.AppendLine("\"Parent\": \"" + path + "\",");
			sb.AppendLine("\"Name\": \"" + newFolder + "\",");
			sb.AppendLine("\"Error\": \"No error\",");
			sb.AppendLine("\"Code\": 0");
			sb.AppendLine("}");

			return sb.ToString();
		}

		public void ProcessRequest(HttpContext context)
		{
			//if (!Roles.IsUserInRole("admin"))
			//{
			//	throw new HttpException(403, "forbidden");
			//}
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Clear();

			switch (context.Request["mode"])
			{
				case "getinfo":

					context.Response.ContentType = "plain/text";
					context.Response.ContentEncoding = Encoding.UTF8;

					context.Response.Write(GetInfo(context.Request["path"]));

					break;
				case "getfolder":

					context.Response.ContentType = "plain/text";
					context.Response.ContentEncoding = Encoding.UTF8;

					context.Response.Write(GetFolderInfo(context.Request["path"]));

					break;
				case "rename":

					context.Response.ContentType = "plain/text";
					context.Response.ContentEncoding = Encoding.UTF8;

					context.Response.Write(Rename(context.Request["old"], context.Request["new"]));

					break;
				case "delete":

					context.Response.ContentType = "plain/text";
					context.Response.ContentEncoding = Encoding.UTF8;

					context.Response.Write(Delete(context.Request["path"]));

					break;
				case "addfolder":

					context.Response.ContentType = "plain/text";
					context.Response.ContentEncoding = Encoding.UTF8;

					context.Response.Write(AddFolder(context.Request["path"], context.Request["name"]));

					break;
				case "download":

					var fi = new FileInfo(context.Server.MapPath(context.Request["path"]));

					context.Response.AddHeader("Content-Disposition", "attachment; filename=" + context.Server.UrlPathEncode(fi.Name));
					context.Response.AddHeader("Content-Length", fi.Length.ToString(CultureInfo.InvariantCulture));
					context.Response.ContentType = "application/octet-stream";
					context.Response.TransmitFile(fi.FullName);

					break;
				case "add":

					var file = context.Request.Files[0];

					var path = context.Request["currentpath"];

					Debug.Assert(file.FileName != null);
					var fn = Path.GetFileName(file.FileName);
					Debug.Assert(fn != null);
					file.SaveAs(context.Server.MapPath(Path.Combine(path, fn)));

					context.Response.ContentType = "text/html";
					context.Response.ContentEncoding = Encoding.UTF8;

					var sb = new StringBuilder();

					sb.AppendLine("{");
					sb.AppendLine("\"Path\": \"" + path + "\",");
					sb.AppendLine("\"Name\": \"" + Path.GetFileName(file.FileName) + "\",");
					sb.AppendLine("\"Error\": \"No error\",");
					sb.AppendLine("\"Code\": 0");
					sb.AppendLine("}");

					var txt = new System.Web.UI.WebControls.TextBox { TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine, Text = sb.ToString() };

					var sw = new StringWriter();
					var writer = new System.Web.UI.HtmlTextWriter(sw);
					txt.RenderControl(writer);

					context.Response.Write(sw.ToString());

					break;
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}