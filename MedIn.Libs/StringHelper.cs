using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MedIn.Libs
{
	public static class StringHelper
	{
		private static readonly Dictionary<char, string> Letters = new Dictionary<char, string>
		{
			{'а', "a"},  {'б', "b"},  {'в', "v"},   {'г', "g"}, {'д', "d"}, {'е', "e"}, {'ё', "yo"}, {'ж', "zh"},
			{'з', "z"},  {'и', "i"},  {'й', "j"},   {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},  {'о', "o"},
			{'п', "p"},  {'р', "r"},  {'с', "s"},   {'т', "t"}, {'у', "u"}, {'ф', "f"}, {'х', "h"},  {'ц', "c"},
			{'ч', "ch"}, {'ш', "sh"}, {'щ', "sch"}, {'ъ', ""},  {'ы', "y"}, {'ь', ""},  {'э', "e"},  {'ю', "yu"},
			{'я', "ya"}, {' ', "-"},  {'-', "-"},   {'+', "-"}, {'=', "-"}, {'_', "_"}, {'0', "0"},  {'1', "1"},
			{'2', "2"},  {'3', "3"},  {'4', "4"},   {'5', "5"}, {'6', "6"}, {'7', "7"}, {'8', "8"},  {'9', "9"},
			{'a', "a"},  {'b', "b"},  {'c', "c"},   {'d', "d"}, {'e', "e"}, {'f', "f"}, {'g', "g"},  {'h', "h"},
			{'i', "i"},  {'j', "j"},  {'k', "k"},   {'l', "l"}, {'m', "m"}, {'n', "n"}, {'o', "o"},  {'p', "p"},
			{'q', "q"},  {'r', "r"},  {'s', "s"},   {'t', "t"}, {'u', "u"}, {'v', "v"}, {'w', "w"},  {'x', "x"},
			{'y', "y"},  {'z', "z"}
		};

		public static string Transliterate(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;
			var result = new StringBuilder();
			value.ToLower().ToCharArray().Where(item => Letters.ContainsKey(item)).ToList().ForEach(item => result.Append(Letters[item]));
			if (result.Length == 0)
			{
				result.Append(Guid.NewGuid());
			}
			return result.ToString();
		}

		public static string CapitalizeWords(this string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Length == 0)
				return value;

			var sb = new StringBuilder(value.Length);
			sb.Append(char.ToUpper(value[0]));
			for (var i = 1; i < value.Length; i++)
			{
				var c = value[i];
				if (char.IsWhiteSpace(value[i - 1]))
					c = char.ToUpper(c);
				else
					c = char.ToLower(c);
				sb.Append(c);
			}
			return sb.ToString();
		}

		public static string Capitalize(this string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Length == 0)
				return value;
			if (value.Length == 1)
				return value.ToUpper();

			var sb = new StringBuilder(value.Length);
			sb.Append(char.ToUpper(value[0]));
			sb.Append(value.Substring(1).ToLower());
			return sb.ToString();
		}

		private static readonly Regex HtmlTagExpression = new Regex(@"(?'tag_start'</?)(?'tag'\w+)((\s+(?'attr'(?'attr_name'\w+)(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+)))?)+\s*|\s*)(?'tag_end'/?>)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		//private static readonly Regex WhiteSpaceBetweenHtmlTagsExpression = new Regex(@">(/w+)<", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		//private static readonly Regex HtmlLineBreakExpression = new Regex(@"<br(/s+)/>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Dictionary<string, List<string>> ValidHtmlTags = new Dictionary<string, List<string>> {
			{ "p", new List<string>() },
			{ "br", new List<string>() }, 
			{ "strong", new List<string>() }, 
			{ "b", new List<string>() }, 
			{ "em", new List<string>() }, 
			{ "i", new List<string>() }, 
			{ "u", new List<string>() }, 
			{ "strike", new List<string>() }, 
			{ "ol", new List<string>() }, 
			{ "ul", new List<string>() }, 
			{ "li", new List<string>() }, 
			{ "a", new List<string> { "href" } }, 
			{ "img", new List<string> { "src", "height", "width", "alt" } },
			{ "q", new List<string> { "cite" } }, 
			{ "cite", new List<string>() }, 
			{ "abbr", new List<string>() }, 
			{ "acronym", new List<string>() }, 
			{ "del", new List<string>() }, 
			{ "ins", new List<string>() }
		};

		public static string ToSafeHtml(this string text)
		{
			return text.RemoveInvalidHtmlTags();
		}

		public static string RemoveInvalidHtmlTags(this string text)
		{

			return string.IsNullOrWhiteSpace(text) ? string.Empty : HtmlTagExpression.Replace(text, m =>
			{
				if (!ValidHtmlTags.ContainsKey(m.Groups["tag"].Value))
					return String.Empty;

				var generatedTag = String.Empty;

				var tagStart = m.Groups["tag_start"];
				var tagEnd = m.Groups["tag_end"];
				var tag = m.Groups["tag"];
				var tagAttributes = m.Groups["attr"];

				generatedTag += (tagStart.Success ? tagStart.Value : "<");
				generatedTag += tag.Value;
				generatedTag = (from Capture attr in tagAttributes.Captures
								let indexOfEquals = attr.Value.IndexOf('=')
								where indexOfEquals >= 1
								let attrName = attr.Value.Substring(0, indexOfEquals)
								where ValidHtmlTags[tag.Value].Contains(attrName)
								let attrValue = attr.Value.Substring(indexOfEquals + 1)
								where attrName != "src" || ValidateUri(attrValue)
								select attr).Aggregate(generatedTag, (current, attr) => current + (" " + attr.Value));

				if (tagStart.Success && tagStart.Value == "<" && tag.Value.Equals("a", StringComparison.OrdinalIgnoreCase))
					generatedTag += " rel=\"nofollow\"";
				generatedTag += (tagEnd.Success ? tagEnd.Value : ">");
				return generatedTag;
			});
		}

		private static bool ValidateUri(string url)
		{
			return url != null;
		}

		public static string RemoveAllHtmlTags(this string text)
		{
			return string.IsNullOrWhiteSpace(text) ? string.Empty : HtmlTagExpression.Replace(text, string.Empty);
		}

		public static string SafeCutString(this string text, int length, bool dots = true)
		{
			if (string.IsNullOrWhiteSpace(text) || text.Length < length || length < 3)
			{
				if (dots)
					return text + "...";
				return text;
			}
			if (dots)
				return text.Substring(0, length) + "...";
			return text.Substring(0, length);
		}


		public static float? TryParseFloat(this string value)
		{
			float result;
			if (float.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result))
				return result;
			return null;
		}

		public static int? TryParseInt(this string value)
		{
			int result;
			if (int.TryParse(value, out result))
				return result;
			return null;
		}
	}
}
