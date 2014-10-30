using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using MedIn.Db.Entities;
using MedIn.OziCms.PagesSettings.Forms;
using MedIn.OziCms.ViewModels;

namespace MedIn.OziCms.Services
{
	public class DefaultFileService
	{
		public static string GetImageUrl(string name, int width, int height, bool proportional)
		{
			return HttpContext.Current.Server.MapPath(name);
		}

		public static void DeleteFile(IFileEntity file, HttpContextBase context)
		{
			if (file == null)
				return;
			try
			{
				var fullname = context.Server.MapPath(file.Name);
				var path = Path.GetDirectoryName(fullname);
				var filename = Path.GetFileNameWithoutExtension(fullname);
				var ext = Path.GetExtension(fullname);
				var mask = string.Format("{0}*{1}", filename, ext);
				var files = Directory.GetFiles(path, mask);
				foreach (var f in files)
				{
					try
					{
						File.Delete(f);
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		public static void ResaveImage(HttpPostedFileBase upload, string filename)
		{
			using (var img = Image.FromStream(upload.InputStream))
			{
				SaveByExt(img, filename);
			}
		}

		public static void ResaveImage(FineUpload upload, string filename, UploadFileSettings settings = null)
		{
			// в некоторые типы картинок можно запихать код для исполнения, чтобы пресечь это, необходимо пересохранить кратинку
			using (var img = Image.FromStream(upload.InputStream))
			{

				// подготовка картинки к сохраниению, рисуем ватермарки и тыды
				if (settings != null && settings.IsImage)
				{
					if (settings.Watermarks.Any(watermarkSettings => watermarkSettings.FillType != WatermarkFillType.None))
					{
						using (var g = Graphics.FromImage(img))
						{
							foreach (var wm in settings.Watermarks)
							{
								using (var wmImage = Image.FromFile(HttpContext.Current.Server.MapPath(wm.RelativePath)))
								{
									var k = 1.0;
									if ((wm.ReduceWidth.HasValue && img.Width <= wm.ReduceWidth.Value) || (wm.ReduceHeight.HasValue && img.Height <= wm.ReduceHeight.Value))
									{
										k = wm.ReduceFactor;
									}
									var width = wmImage.Width / k;
									var height = wmImage.Height / k;
									if (wm.FillType == WatermarkFillType.Mosaic)
									{
										var cY = 0;
										while (cY < img.Height)
										{
											var cX = 0;
											while (cX < img.Width)
											{
												g.DrawImage(wmImage, new Rectangle(cX, cY, (int)width, (int)height));
												cX += wmImage.Width;
											}
											cY += wmImage.Height;
										}
									}
									else
									{
										// координаты на исходном изображении куда выводить ватермарк
										var left = 0.0;
										var top = 0.0;
										switch (wm.FillType)
										{
											case WatermarkFillType.TopLeft:
												left = wm.Margins.Left / k;
												top = wm.Margins.Top / k;
												break;
											case WatermarkFillType.TopRight:
												left = img.Width - wm.Margins.Right / k - width;
												top = wm.Margins.Top / k;
												break;
											case WatermarkFillType.BottomLeft:
												left = wm.Margins.Left / k;
												top = img.Height - wm.Margins.Bottom / k - height;
												break;
											case WatermarkFillType.BottomRight:
												left = img.Width - wm.Margins.Right / k - width;
												top = img.Height - wm.Margins.Bottom / k - height;
												break;
											case WatermarkFillType.Center:
												left = (img.Width - width)/2;
												top = (img.Height - height)/2;
												break;
											case WatermarkFillType.Stretch:
												left = wm.Margins.Left / k;
												top = wm.Margins.Top / k;
												width = img.Width - wm.Margins.Right / k;
												height = img.Height - wm.Margins.Bottom / k;
												break;
											case WatermarkFillType.Custom:
												left = wm.Left + wm.Margins.Left;
												top = wm.Top + wm.Margins.Top;
												width = wm.Width == 0 ? wmImage.Width : wm.Width;
												height = wm.Height == 0 ? wmImage.Height : wm.Height;
												break;
											default:
												throw new ArgumentOutOfRangeException();
										}
										g.DrawImage(wmImage, new Rectangle((int)left, (int)top, (int)width, (int)height));
									}
								}
							}
						}
					}
				}
				SaveByExt(img, filename);
			}
		}

		private static void SaveByExt(Image img, string filename)
		{
			var ext = Path.GetExtension(filename);
			if (".jpg".Equals(ext, StringComparison.InvariantCultureIgnoreCase) || ".jpeg".Equals(ext, StringComparison.InvariantCultureIgnoreCase))
			{
				SaveJpg(img, filename);
			}
			else if (".gif".Equals(ext, StringComparison.InvariantCultureIgnoreCase))
			{
				SaveGif(img, filename);
			}
			else if (".png".Equals(ext, StringComparison.InvariantCultureIgnoreCase))
			{
				SavePng(img, filename);
			}
		}

		private static void SavePng(Image img, string filename)
		{
			img.Save(filename, ImageFormat.Png);
		}

		private static void SaveGif(Image img, string filename)
		{
			img.Save(filename, ImageFormat.Gif);
		}

		private static void SaveJpg(Image img, string filename)
		{
			var jgpEncoder = GetEncoder(ImageFormat.Jpeg);
			var myEncoder = Encoder.Quality;
			var myEncoderParameters = new EncoderParameters(1);
			var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
			myEncoderParameters.Param[0] = myEncoderParameter;
			img.Save(filename, jgpEncoder, myEncoderParameters);
		}

		private static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			var codecs = ImageCodecInfo.GetImageDecoders();
			return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
		}
	}
}