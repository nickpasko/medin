using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace DotOrg.Libs.ImageProcessing
{
	public sealed class ImageDescriptor : IDisposable
	{
		public ImageDescriptor()
		{
			Parameters = new NameValueDictionary();
		}

		public string SourceRelativeName { get; set; }
		public string DestinationRelativeName { get; set; }
		public string DestinationRelativeFolder { get; set; }

		public IDictionary<string, object> Parameters { get; set; }

		public static class ParametersNames
		{
			public static readonly string WatermarkPath = "WatermarkPath";
			public static readonly string SourceImage = "SourceImage";
			public static readonly string DestinationImage = "DestinationImage";
			public static readonly string TargetWidth = "TargetWidth";
			public static readonly string TargetHeight = "TargetHeight";
			public static readonly string ResultWidth = "ResultWidth";
			public static readonly string ResultHeight = "ResultHeight";
			public static readonly string ResultLeft = "ResultLeft";
			public static readonly string ResultTop = "ResultTop";
			public static readonly string SourceWidth = "SourceWidth";
			public static readonly string SourceHeight = "SourceHeight";
			public static readonly string Postfixes = "Postfixes";
			public static readonly string FullLocalPath = "FullLocalPath";
		}

		public void Dispose()
		{
			foreach (var parameter in Parameters)
			{
				var value = parameter.Value as IDisposable;
				if (value != null)
				{
					value.Dispose();
				}
			}
		}
	}

	internal class NameValueDictionary : IDictionary<string, object>
	{
		private readonly IDictionary<string, object> _dict = new Dictionary<string, object>();

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _dict.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(KeyValuePair<string, object> item)
		{
			_dict.Add(item);
		}

		public void Clear()
		{
			_dict.Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return _dict.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			_dict.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return _dict.Remove(item);
		}

		public int Count { get { return _dict.Count; } }
		public bool IsReadOnly { get { return _dict.IsReadOnly; } }
		public bool ContainsKey(string key)
		{
			return _dict.ContainsKey(key);
		}

		public void Add(string key, object value)
		{
			_dict.Add(key, value);
		}

		public bool Remove(string key)
		{
			return _dict.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return _dict.TryGetValue(key, out value);
		}

		public object this[string key]
		{
			get
			{
				return _dict.ContainsKey(key) ? _dict[key] : null;
			}
			set
			{
				if (_dict.ContainsKey(key))
				{
					_dict[key] = value;
				}
				else
				{
					_dict.Add(key, value);
				}
			}
		}

		public ICollection<string> Keys { get { return _dict.Keys; } }
		public ICollection<object> Values { get { return _dict.Values; } }
	}
}
