namespace MedIn.Libs
{
	public struct Margins
	{
		private int _left;
		private int _top;
		private int _right;
		private int _bottom;

		public Margins(int left, int top, int right, int bottom)
		{
			_left = left;
			_top = top;
			_right = right;
			_bottom = bottom;
		}

		public int Left
		{
			get { return _left; }
			set { _left = value; }
		}

		public int Top
		{
			get { return _top; }
			set { _top = value; }
		}

		public int Right
		{
			get { return _right; }
			set { _right = value; }
		}

		public int Bottom
		{
			get { return _bottom; }
			set { _bottom = value; }
		}
	}
}
