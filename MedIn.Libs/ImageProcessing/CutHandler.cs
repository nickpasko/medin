using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DotOrg.Libs.ImageProcessing
{
	class CutHandler : ImageHandler
	{
		protected override bool ProcessInternal(ImageDescriptor descriptor)
		{
			var w = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.TargetWidth]);
			var h = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.TargetHeight]);
			var tw = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.ResultWidth]);
			var th = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.ResultHeight]);
			var x = 0;
			var y = 0;

			if (h == 0)
			{
				h = w * th / tw;
			}
			else if (w == 0)
			{
				w = h * tw / th;
			}


			if (tw > w)
			{
				x = (w - tw)/2;
			}
			else if (th > h)
			{
				y = (h - th)/2;
			}
			descriptor.Parameters[ImageDescriptor.ParametersNames.ResultLeft] = x;
			descriptor.Parameters[ImageDescriptor.ParametersNames.ResultTop] = y;
			return true;
		}

		protected override void PostProcess(ImageDescriptor descriptor)
		{
			base.PostProcess(descriptor);
			var postfix = (StringBuilder)descriptor.Parameters[ImageDescriptor.ParametersNames.Postfixes] ?? new StringBuilder();
			postfix.Append("_cut");
			descriptor.Parameters[ImageDescriptor.ParametersNames.Postfixes] = postfix;
		}
	}
}
