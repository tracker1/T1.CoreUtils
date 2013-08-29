using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace T1.CoreUtils
{
	public static class XElementExtensions
	{
		public static XElement UseChildOrDefault(this XElement parent, XElement defaultValue)
		{
			if (parent == null) throw new ArgumentException("parent is null");

			if (defaultValue == null) throw new ArgumentException("defaultValue is null");

			foreach (XElement node in parent.Nodes())
			{
				if (node.Name == defaultValue.Name) return node;
			}

			parent.Add(defaultValue);
			return defaultValue;
		}

		public static string AsCleanString(this XElement xml)
		{
			if (xml == null) return null;

			var ret = xml.ToString();
			ret = Regex.Replace(ret, @"\>[\r\n\s]*\<", "><");
			return ret;
		}

		public static string GetElementValue(this XElement xe, string elementName)
		{
			if (xe == null) return string.Empty;

			var nodes = xe.Elements(elementName);
			if (nodes.Count() == 0) return string.Empty;

			return nodes.First().Value.Trim();
		}

		/// <summary>
		/// Gets the text value from an element.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="name">The name.</param>
		/// <param name="removeLineBreaks">if set to <c>true</c> [remove line breaks].</param>
		/// <returns></returns>
		public static string GetElementStringValue(this XElement source, string name, bool removeLineBreaks = false)
		{
			var element = source.DescendantsAndSelf(name).FirstOrDefault();
			if (element == null)
				return null;
			return removeLineBreaks ? element.Value.Replace("\r\n", "") : element.Value;
		}

		/// <summary>
		/// Gets a nullable date value from the element.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="name">The name.</param>
		/// <param name="returnDefault">if set to <c>true</c> [return default].</param>
		/// <returns></returns>
		public static DateTime? GetElementDateValue(this XElement source, string name, bool returnDefault = false)
		{
			var valueToParse = GetElementStringValue(source, name);
			DateTime result;
			if (DateTime.TryParse(valueToParse, out result))
				return result;
			if (returnDefault)
				return DateTime.MinValue;
			return null;
		}

		/// <summary>
		/// Gets a nullable int value from the element.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="name">The name.</param>
		/// <param name="returnDefault">if set to <c>true</c> [return default].</param>
		/// <returns></returns>
		public static int? GetElementIntValue(this XElement source, string name, bool returnDefault = false)
		{
			var valueToParse = GetElementStringValue(source, name);
			int result;
			if (int.TryParse(valueToParse, out result))
				return result;
			if (returnDefault)
				return 0;
			return null;
		}

		/// <summary>
		/// Gets a nullable boolean value from the element.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="name">The name.</param>
		/// <param name="valueOfTrue">The text value equivalent to true.</param>
		/// <param name="returnDefault">if set to <c>true</c> [return default].</param>/// 
		/// <returns></returns>
		public static bool? GetElementBoolValue(this XElement source, string name, string valueOfTrue = null, bool returnDefault = false)
		{
			var valueToParse = GetElementStringValue(source, name);
			if (valueOfTrue != null)
			{
				return valueToParse == valueOfTrue;
			}
			bool result;
			if (bool.TryParse(valueToParse, out result))
				return result;
			if (returnDefault)
				return false;
			return null;
		}

		/// <summary>
		/// Gets a nullable decimal value from the element.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="name">The name.</param>
		/// <param name="returnDefault">if set to <c>true</c> [return default].</param>
		/// <returns></returns>
		public static Decimal? GetElementDecimalValue(this XElement source, string name, bool returnDefault = false)
		{
			var valueToParse = GetElementStringValue(source, name);
			decimal result;
			if (decimal.TryParse(valueToParse, out result))
				return result;
			if (returnDefault)
				return 0;
			return null;
		}
	}
}
