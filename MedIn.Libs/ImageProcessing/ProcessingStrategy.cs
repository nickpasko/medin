using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotOrg.Libs.ImageProcessing
{
	public abstract class ProcessingStrategy
	{
		public abstract IEnumerable<ImageHandler> Handlers { get; }

		private static ProcessingStrategy _dontProcess;
		public static ProcessingStrategy DontProcess
		{
			get { return _dontProcess ?? (_dontProcess = new NoProcessStrategy()); }
		}

		private static ProcessingStrategy _resize;
		public static ProcessingStrategy Resize
		{
			get { return _resize ?? (_resize = new ResizeStrategy()); }
		}

		private static ProcessingStrategy _proportional;
		public static ProcessingStrategy Proportional
		{
			get { return _proportional ?? (_proportional = new ProportionalResize()); }
		}

		private static ProcessingStrategy _proportionalFill;
		public static ProcessingStrategy ProportionalFill
		{
			get { return _proportionalFill ?? (_proportionalFill = new ProportionalFillResize()); }
		}
	}

	class NoProcessStrategy : ProcessingStrategy
	{
		public override IEnumerable<ImageHandler> Handlers
		{
			get { return new[] {new NothingHandler()}; }
		}
	}

	class ResizeStrategy : ProcessingStrategy
	{
		public override IEnumerable<ImageHandler> Handlers
		{
			get { return new ImageHandler[] { new LoadHandler(), new ResizeHandler(), new SaveHandler() }; }
		}
	}

	class ProportionalResize : ProcessingStrategy
	{
		public override IEnumerable<ImageHandler> Handlers
		{
			get { return new ImageHandler[] { new LoadHandler(), new CalculateMinSizeHandler(), new ResizeHandler(), new SaveHandler() }; }
		}
	}

	class ProportionalFillResize : ProcessingStrategy
	{
		public override IEnumerable<ImageHandler> Handlers
		{
			get { return new ImageHandler[] { new LoadHandler(), new CalculateMaxSizeHandler(), new CutHandler(), new ResizeHandler(), new SaveHandler() }; }
		}
	}
}
