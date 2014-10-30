using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Encoder = System.Drawing.Imaging.Encoder;

namespace DotOrg.Libs.ImageProcessing
{
	class SaveHandler : ImageHandler
	{
		protected override bool ProcessInternal(ImageDescriptor descriptor)
		{
			var img = (Image)(descriptor.Parameters[ImageDescriptor.ParametersNames.DestinationImage] ?? descriptor.Parameters[ImageDescriptor.ParametersNames.SourceImage]);

			var path = GenerateName(descriptor);
			var fullname = HttpContext.Current.Server.MapPath(path);
			SaveImage(img, fullname);

			return true;
		}

		private static void SaveImage(Image img, string path)
		{
			var format = GetImageFormat(path);
			if (format.Equals(ImageFormat.Jpeg))
			{
				var qualityParam = new EncoderParameter(Encoder.Quality, 83L);
				var jpegCodec = GetEncoderInfo(format);
				if (jpegCodec == null)
					return;
				var encoderParams = new EncoderParameters(1);
				encoderParams.Param[0] = qualityParam;
				img.Save(path, jpegCodec, encoderParams);
			}
			else
			{
				img.Save(path, format);
			}
		}

		private static ImageFormat GetImageFormat(string path)
		{
			var ext = Path.GetExtension(path);
			if (".png".Equals(ext, StringComparison.InvariantCultureIgnoreCase))
				return ImageFormat.Png;
			return ".gif".Equals(ext, StringComparison.InvariantCultureIgnoreCase) ? ImageFormat.Gif : ImageFormat.Jpeg;
		}

		private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
		{
			// Get image codecs for all image formats
			var codecs = ImageCodecInfo.GetImageEncoders();
			// Find the correct image codec
			return codecs.FirstOrDefault(t => t.FormatID == format.Guid);
		}
	}
}
