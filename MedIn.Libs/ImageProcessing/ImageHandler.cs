using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DotOrg.Libs.ImageProcessing
{
	public abstract class ImageHandler
	{
		public virtual bool Process(ImageDescriptor descriptor)
		{
			try
			{
				return ProcessInternal(descriptor);
			}
			catch (Exception exception)
			{
				return false;
			}
		}

		protected abstract bool ProcessInternal(ImageDescriptor descriptor);

		protected virtual void PostProcess(ImageDescriptor descriptor)
		{
		}

		public static bool Handle(ImageDescriptor descriptor, IEnumerable<ImageHandler> chain)
		{
			var p = descriptor.Parameters;
			descriptor.Parameters = new NameValueDictionary();
			foreach (var o in p)
			{
				descriptor.Parameters.Add(o);
			}
			foreach (var handler in chain)
			{
				handler.PostProcess(descriptor);
			}
			var result = true;
			if (!File.Exists(HttpContext.Current.Server.MapPath(GenerateName(descriptor))))
			{
				descriptor.Parameters = p;
				foreach (var handler in chain)
				{
					result &= handler.Process(descriptor);
					handler.PostProcess(descriptor);
				}
			}
			return result;
			//return chain.Aggregate(true, (current, handler) => current & handler.Process(descriptor));
		}

		protected static string GenerateName(ImageDescriptor descriptor)
		{
			var postfixes = (StringBuilder)descriptor.Parameters[ImageDescriptor.ParametersNames.Postfixes];
			var postfix = postfixes == null ? string.Empty : postfixes.ToString();
			var filename = Path.GetFileNameWithoutExtension(Path.GetFileName(descriptor.SourceRelativeName));
			var ext = Path.GetExtension(descriptor.SourceRelativeName);
			var name = string.Format("{0}{1}{2}", filename, postfix, ext);
			var srcFolder = Path.GetDirectoryName(descriptor.SourceRelativeName);
			var path = Path.Combine(descriptor.DestinationRelativeFolder ?? srcFolder, name);
			descriptor.DestinationRelativeName = path;
			return path;
		}
	}
}
