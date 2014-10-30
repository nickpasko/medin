using System;
using System.Globalization;
using System.Xml.Linq;

namespace MedIn.Libs
{
	public static class DotOrgHelpers
	{
		public static int GetInt(this XElement element, string attributeName, int defaultValue)
		{
			return GetInt(element, attributeName) ?? defaultValue;
		}

		public static int? GetInt(this XElement element, string attributeName)
		{
			var attribute = element.Attribute(attributeName);
			if (attribute == null)
				return null;
			int result;
			return int.TryParse(attribute.Value, out result) ? result : (int?)null;
		}

		public static XName ToXName(this string localName)
		{
			return XName.Get(localName, "urn:settings");
		}

		public static Margins GetMargins(this XElement element, string attributeName)
		{
			var attribute = element.Attribute(attributeName);
			if (attribute == null)
			{
				return new Margins();
			}
			var src = attribute.Value;
			var items = src.Split(new[] {','});
			if (items.Length == 1)
			{
				var v = int.Parse(items[0].Trim());
				return new Margins(v, v, v, v);
			}
			if (items.Length == 2)
			{
				var tb = int.Parse(items[0].Trim());
				var lr = int.Parse(items[1].Trim());
				return new Margins(lr, tb, lr, tb);
			}
			if (items.Length == 3)
			{
				var t = int.Parse(items[0].Trim());
				var lr = int.Parse(items[1].Trim());
				var b = int.Parse(items[2].Trim());
				return new Margins(lr, t, lr, b);
			}
			if (items.Length == 4)
			{
				var t = int.Parse(items[0].Trim());
				var r = int.Parse(items[1].Trim());
				var b = int.Parse(items[2].Trim());
				var l = int.Parse(items[3].Trim());
				return new Margins(l, t, r, b);
			}
			throw new FormatException(string.Format("incorrect format: {0}", src));
		}

		public static double GetDouble(this XElement element, string attributeName, double defaultValue)
		{
			return GetDouble(element, attributeName) ?? defaultValue;
		}

		public static double? GetDouble(this XElement element, string attributeName)
		{
			var attribute = element.Attribute(attributeName);
			if (attribute == null)
				return null;
			double result;
			return double.TryParse(attribute.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ? result : (double?)null;
		}

		public static string GetString(this XElement element, string attributeName, string defaultValue = null)
		{
			var attribute = element.Attribute(attributeName);
			return attribute != null ? attribute.Value : defaultValue;
		}

		public static bool? GetNullableBoolean(this XElement element, string attributeName)
		{
			var attribute = element.Attribute(attributeName);
			return attribute == null ? null : (bool?)Boolean.Parse(attribute.Value);
		}

		public static bool GetBoolean(this XElement element, string attributeName, bool defaultValue = false)
		{
			var attribute = element.Attribute(attributeName);
			return attribute != null && Boolean.Parse(attribute.Value);
		}

		public static T GetValue<T>(this XElement field, string name, Func<string, T> converter, T defaultValue)
		{
			var attr = field.Attribute(name);
			return attr != null ? converter(attr.Value) : defaultValue;
		}

	}
}