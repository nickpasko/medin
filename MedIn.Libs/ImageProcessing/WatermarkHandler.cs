using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotOrg.Libs.ImageProcessing
{
	class WatermarkHandler : ImageHandler
	{
		protected override bool ProcessInternal(ImageDescriptor descriptor)
		{
			throw new NotImplementedException();
		}

		protected override void PostProcess(ImageDescriptor descriptor)
		{
			base.PostProcess(descriptor);
			var postfix = (StringBuilder)descriptor.Parameters[ImageDescriptor.ParametersNames.Postfixes] ?? new StringBuilder();
			postfix.Append("_wm");
			descriptor.Parameters[ImageDescriptor.ParametersNames.Postfixes] = postfix;
		}
	}
}
