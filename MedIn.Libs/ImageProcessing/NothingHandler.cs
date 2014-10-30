using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotOrg.Libs.ImageProcessing
{
	class NothingHandler : ImageHandler
	{
		protected override bool ProcessInternal(ImageDescriptor descriptor)
		{
			descriptor.DestinationRelativeName = descriptor.SourceRelativeName;
			return true;
		}
	}
}
