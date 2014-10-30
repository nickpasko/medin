using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DotOrg.Libs.ImageProcessing
{
	abstract class CalculateSizeHandler : ImageHandler
	{
		protected override bool ProcessInternal(ImageDescriptor descriptor)
		{
			var image = (Image)descriptor.Parameters[ImageDescriptor.ParametersNames.SourceImage];
			var w = image.Width;
			var h = image.Height;
			var targetWidth = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.TargetWidth]);
			var targetHeight = Convert.ToInt32(descriptor.Parameters[ImageDescriptor.ParametersNames.TargetHeight]);

			if (targetHeight == 0)
			{
				targetHeight = targetWidth*h/w;
			}
			else if (targetWidth == 0)
			{
				targetWidth = targetHeight * w / h;
			}

			double resultWidth = targetWidth;
			double resultHeight = (double)h / w * resultWidth;
			if (ShouldBeRecalculated(resultHeight, targetHeight))
			{
				resultHeight = targetHeight;
				resultWidth = (double)w / h * resultHeight;
			}
			descriptor.Parameters[ImageDescriptor.ParametersNames.ResultWidth] = resultWidth;
			descriptor.Parameters[ImageDescriptor.ParametersNames.ResultHeight] = resultHeight;
			descriptor.Parameters[ImageDescriptor.ParametersNames.SourceWidth] = w;
			descriptor.Parameters[ImageDescriptor.ParametersNames.SourceHeight] = h;
			return true;
		}

		protected abstract bool ShouldBeRecalculated(double resultSize, double targetSize);
	}
}
