using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace MedIn.Libs
{
    public static class CommonHelpers
    {
		#region make thumb

		private static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			// Get image codecs for all image formats
			var codecs = ImageCodecInfo.GetImageEncoders();
			// Find the correct image codec
			return codecs.FirstOrDefault(t => t.MimeType == mimeType);
		}

	    public static void MakeThumb(string sourceFile, string destinationFile, int maxWidth, int maxHeight, bool constProp)
	    {
		    try
		    {
			    using (var image = Image.FromFile(sourceFile))
			    {
				    var size = GetDesctinationImageSize(image, maxWidth, maxHeight, constProp);
				    if (size.IsEmpty)
					    return;
				    using (var thumbnail = new Bitmap(size.Width, size.Height))
				    using (var graphic = Graphics.FromImage(thumbnail))
				    {
					    graphic.DrawImage(image, new Rectangle(Point.Empty, size));
					    // Encoder parameter for image quality
					    var qualityParam = new EncoderParameter(Encoder.Quality, (long)75);
					    // Jpeg image codec
					    var jpegCodec = GetEncoderInfo("image/jpeg");
					    if (jpegCodec == null)
						    return;
					    var encoderParams = new EncoderParameters(1);
					    encoderParams.Param[0] = qualityParam;
					    thumbnail.Save(destinationFile, jpegCodec, encoderParams);
				    }
			    }
		    }
		    catch (OutOfMemoryException) // file is corrupted
		    {
		    }
	    }

	    private static Size GetDesctinationImageSize(Image image, int maxWidth, int maxHeight, bool constProp)
	    {
			var resultWidth = image.Width;
			var resultHeight = image.Height;

			if (maxWidth == 0 && maxHeight == 0)
			{
				return Size.Empty;
			}
			if (maxWidth == 0 && maxHeight != 0)
			{
				resultHeight = Math.Min(resultHeight, maxHeight);
				resultWidth = image.Width * resultHeight / image.Height;
			}
			else if (maxHeight == 0 && maxWidth != 0)
			{
				resultWidth = Math.Min(resultWidth, maxWidth);
				resultHeight = image.Height * resultWidth / image.Width;
			}
			else if (constProp)
			{
				if (resultWidth > maxWidth)
				{
					resultHeight = resultHeight * maxWidth / resultWidth;
					resultWidth = maxWidth;
				}

				if (resultHeight > maxHeight)
				{
					resultWidth = resultWidth * maxHeight / resultHeight;
					resultHeight = maxHeight;
				}
			}
			else
			{
				resultWidth = maxWidth == 0 ? image.Width : maxWidth;
				resultHeight = maxHeight == 0 ? image.Height : maxHeight;
			}
			return new Size(resultWidth, resultHeight);
	    }

	    #endregion

        public static Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(IEnumerable<Dictionary<TKey, TValue>> enumerable)
        {
            return enumerable.SelectMany(x => x).ToDictionary(x => x.Key, y => y.Value);
        }

        //public static IEnumerable<T> WhereForAll<T>(this ObjectSet<T> set, Expression<Func<T, bool>> predicate) where T : class
        //{
        //    var dbResult = set.Where(predicate);

        //    var offlineResult = set.Context.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Select(entry => entry.Entity).OfType<T>().Where(predicate.Compile());

        //    return offlineResult.Union(dbResult);
        //}

        //public static T FirstOrDefaultForAll<T>(this ObjectSet<T> set, Func<T, bool> predicate) where T : class
        //{
        //    var dbResult = set.ToList().FirstOrDefault(predicate);

        //    if (dbResult == null)
        //    {
        //        var offlineResult = set.Context.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Select(entry => entry.Entity).OfType<T>().FirstOrDefault(predicate);
        //        return offlineResult;
        //    }
			
        //    return dbResult;
        //}

	    public static bool ParseCheckbox(string value)
	    {
			return value != null && (value.ToLower() == "on" || value.ToLower() == "true");
	    }
    }
}