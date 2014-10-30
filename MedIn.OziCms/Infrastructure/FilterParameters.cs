namespace MedIn.OziCms.Infrastructure
{
	public class FilterParameters
	{
		public int? Page { get; set; }
		public int? ItemsCount { get; set; }
		public int? PageSize { get; set; }

		public int GetPagesCount()
		{
			if (!ItemsCount.HasValue || !PageSize.HasValue)
				return 0;
			var result = ItemsCount.Value / PageSize.Value;
			if (ItemsCount.Value % PageSize.Value > 0)
			{
				result++;
			}
			return result;
		}

		public bool HasPager()
		{
			return PageSize.HasValue && ItemsCount.HasValue;
		}

		public int StartPosition
		{
			get
			{
				return (PageSize ?? 0) * ((Page ?? 1) - 1);
			}
		}
	}
}
