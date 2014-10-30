using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DotOrg.Libs.ImageProcessing
{
	class ResizeHandler : ImageHandler
	{
		protected override bool ProcessInternal(ImageDescriptor descriptor)
		{
			var tw = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.TargetWidth]);
			var th = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.TargetHeight]);
			var w = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.ResultWidth] ?? descriptor.Parameters[ImageDescriptor.ParametersNames.TargetWidth]);
			var h = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.ResultHeight] ?? descriptor.Parameters[ImageDescriptor.ParametersNames.TargetHeight]);
			var x = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.ResultLeft] ?? 0);
			var y = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.ResultTop] ?? 0);
			var sImg = (Image)descriptor.Parameters[ImageDescriptor.ParametersNames.SourceImage];
			if (th == 0)
			{
				th = tw * h / w;
			}
			else if (tw == 0)
			{
				tw = th * w / h;
			}

			var img = new Bitmap(Math.Min(w, tw), Math.Min(h, th));
			using (var g = Graphics.FromImage(img))
			{
				g.DrawImage(sImg, new Rectangle(x, y, w, h));
			}
			descriptor.Parameters[ImageDescriptor.ParametersNames.DestinationImage] = img;
			return true;
		}

		protected override void PostProcess(ImageDescriptor descriptor)
		{
			base.PostProcess(descriptor);
			var postfix = (StringBuilder)descriptor.Parameters[ImageDescriptor.ParametersNames.Postfixes] ?? new StringBuilder();
			var targetWidth = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.TargetWidth]);
			var targetHeight = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.TargetHeight]);
			postfix.AppendFormat("_w{0}_h{1}", targetWidth, targetHeight);
			descriptor.Parameters[ImageDescriptor.ParametersNames.Postfixes] = postfix;
		}
	}
}
