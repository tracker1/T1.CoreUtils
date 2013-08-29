using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace T1.CoreUtils
{
	public static class StringExtensions
	{
		public static bool IsNullOrEmpty(this string input)
		{
			return String.IsNullOrEmpty(input);
		}

		public static bool HasValue(this string input)
		{
			return String.IsNullOrWhiteSpace(input);
		}

		public static string ForceUrlEncoding(this string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return string.Empty;
			return string.Join("", input.ToCharArray().Select(c => (uint)c <= 253 ? "%" + ((int)c).ToString("x2") : "%u" + ((int)c).ToString("x4")));
		}

		/// <summary>
		/// Will take a string as input, and if it is null, returns null;  a generally accepted false-like value, returns false;  otherwise will return true.
		/// </summary>
		/// <param name="s">input string to convert to a boolean.</param>
		/// <param name="valueWhenNull">value to use when the input string is itself null</param>
		/// <returns>value to use when the input string is itself null</returns>
		/// <remarks>
		///		A null string will return null.
		///		The strings starting with the following characters will be false:
		///			"f", "n", "u", "0"
		///		Strings containing the following words will be false (lowercased)
		///			"", "0", "f", "false", "no", "off", "null", "undefined", "nan"
		///		Everything else is true.
		/// </remarks>
		public static bool AsBoolean(this string s, bool valueWhenNull = false)
		{
			if (s == null) return valueWhenNull;

			s = s.Trim().ToLowerInvariant();
			if (
				s == string.Empty
				|| s == "0"
				|| s == "f"
				|| s == "false"
				|| s == "no"
				|| s == "off"
				|| s == "null"
				|| s == "undefined"
				|| s == "nan"
				|| s[0] == 'f'
				|| s[0] == 'n'
				|| s[0] == 'u'
				|| s[0] == '0'
			) return false;

			return true;
		}
		
		/// <summary>
		/// Convert a Guid token to a guid-like string, or the original string
		/// </summary>
		/// <param name="input">the string to normalize</param>
		/// <returns>original input, or reformatted string</returns>
		private static string NormalizeGuidFormat(string input)
		{
			if (string.IsNullOrWhiteSpace(input) || input.Length != 22) return input;

			try 
			{	        
				var temp = input.Replace('_', '+').Replace('-', '/') + "==";
				var bytes = Convert.FromBase64String(temp);
				var ret = new Guid(bytes);
				return ret.ToString().ToLower();
			}
			catch (Exception)
			{
				return input;
			}
		}
		
		/// <summary>
		/// Will return a guid value for the string or Nothing.  Guid.Empty will return null.
		/// </summary>
		/// <param name="s">String to convert to a Guid</param>
		/// <returns>Returns the parsed value.</returns>
		/// <remarks>
		///		When the value equates to Guid.Empty or null will return null.
		/// </remarks>
		public static Guid? AsGuid(this string s)
		{
			s = NormalizeGuidFormat(s);
			var ret = Guid.Empty;
			if (Guid.TryParse(s, out ret)) {
				if (!Guid.Empty.Equals(ret)) return ret;
			}
			return null;
		}

		/// <summary>
		/// Will return a guid value for the string or Nothing.  Guid.Empty will return null.
		/// </summary>
		/// <param name="s">String to convert to a Guid</param>
		/// <param name="defaultValue">Value to return when parsing fails</param>
		/// <returns>Returns the parsed value.</returns>
		/// <remarks>
		///		When the value equates to Guid.Empty and defaultWhenNothing is null will return null.
		/// </remarks>
		public static Guid AsGuid(this string s, Guid defaultValue)
		{
			s = NormalizeGuidFormat(s);
			var ret = Guid.Empty;
			return Guid.TryParse(s, out ret) ? ret : defaultValue;
		}

		/// <summary>
		/// Attempts to parse the string into a nullable DateTimeOffset value.
		/// </summary>
		/// <param name="s">The string to parse into an offset value.</param>
		/// <returns>Nullable DateTimeOffset</returns>
		public static DateTimeOffset? AsDateTimeOffset(this string s)
		{
			var ret = DateTimeOffset.MinValue;
			if (DateTimeOffset.TryParse(s, out ret)) return ret;
			return null;
		}

		/// <summary>
		/// Attempts to parse the string into a nullable DateTimeOffset value.
		/// </summary>
		/// <param name="s">The string to parse into an offset value.</param>
		/// <param name="defaultWhenNothing">The default value to use if unable to parse.</param>
		/// <returns>DateTimeOffset</returns>
		public static DateTimeOffset AsDateTimeOffset(this string s, DateTimeOffset defaultWhenNothing)
		{
			var ret = AsDateTimeOffset(s);
			return (ret.HasValue) ? ret.Value : defaultWhenNothing;
		}

		/// <summary>
		/// Attempts to parse the string into a nullable DateTime value.
		/// </summary>
		/// <param name="s">The string to parse into an offset value.</param>
		/// <returns>ullable DateTime</returns>
		public static DateTime? AsDateTime(this string s)
		{
			var ret = DateTime.MinValue;
			if (DateTime.TryParse(s, out ret)) return ret;
			return null;
		}

		/// <summary>
		/// Attempts to parse the string into a nullable DateTime value.
		/// </summary>
		/// <param name="s">The string to parse into an offset value.</param>
		/// <param name="defaultWhenNothing">The default value to use if unable to parse.</param>
		/// <returns>DateTime</returns>
		public static DateTime AsDateTime(this string s, DateTime defaultWhenNothing)
		{
			var ret = AsDateTime(s);
			return (ret.HasValue) ? ret.Value : defaultWhenNothing;
		}

		public static int? AsInteger(this string s)
		{
			var ret = int.MinValue;
			if (int.TryParse(s, out ret)) return ret;
			return null;
		}

		public static int AsInteger(this string s, int defaultValue)
		{
			var ret = int.MinValue;
			return int.TryParse(s, out ret) ? ret : defaultValue;
		}

		public static long? AsLong(this string s)
		{
			var ret = long.MinValue;
			if (long.TryParse(s, out ret)) return ret;
			return null;
		}

		public static long AsLong(this string s, int defaultValue)
		{
			var ret = long.MinValue;
			return long.TryParse(s, out ret) ? ret : defaultValue;
		}

		public static decimal? AsDecimal(this string s)
		{
			var ret = decimal.MinValue;
			if (decimal.TryParse(s, out ret)) return ret;
			return null;
		}

		public static decimal AsDecimal(this string s, decimal defaultValue)
		{
			var ret = decimal.MinValue;
			return decimal.TryParse(s, out ret) ? ret : defaultValue;
		}

		public static ulong? FromArbitraryBase(this string s, char[] keys, ulong? defaultValue = null)
		{
			if (keys == null || keys.Length < 2 || string.IsNullOrWhiteSpace(s)) return defaultValue;
			ulong ret = 0;
			bool found;

			for (var i=0; i<s.Length; i++) {
				var c = s[i];
				found = false;
				for (ulong j=0; j<(ulong)keys.Length; j++) {
					if (keys[j] == s[i]) {
						found = true;
						ret = ret * ((ulong)keys.Length) + j;
					}
				}
				if (!found) return defaultValue; //invalid input, contains character not in the keys list.
			}

			return ret;
		}

		public static ulong? FromBase26Alpha(this string s)
		{
			return FromArbitraryBase(s, IntegerExtensions.Base26Alpha.ToCharArray());
		}

		public static ulong? FromBase33Safe(this string s)
		{
			return FromArbitraryBase(s, IntegerExtensions.Base33Safe.ToCharArray());
		}

		public static ulong? FromCertificationNumber(this string s)
		{
			//handle potential miss input of 0 for o, and 1 or l for i.
			s = (s ?? "").ToLower().Replace('0','o').Replace('l','i').Replace('1','i');
			return FromArbitraryBase(s, IntegerExtensions.Base33SafeMixed.ToCharArray());
		}

		public static string RemoveIllegalFileCharacters(this string input)
		{
			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			return r.Replace(input, "");
		}

		public static string S3Encode(this string source)
		{
			source = Regex.Replace(source.Replace('&',' ').Trim().RemoveIllegalFileCharacters(), "[\\s\\&]+", "+");
			source = source.Replace("/", "");
			source = source.Replace("'", "");
			source = source.Replace("\"", "");
			source = source.Replace("(", "");
			source = source.Replace(")", "");
			source = HttpUtility.UrlEncode(source);
			return source;
		}

		public static string PadTLeft(this String text, int totalWidth, char paddingChar)
		{
			string arg = text.Length > totalWidth ? text.Substring(0, totalWidth) : text;
			return arg.PadLeft(totalWidth, paddingChar);
		}

		public static string PadTRight(this String text, int totalWidth, char paddingChar)
		{
			string arg = text.Length > totalWidth ? text.Substring(0, totalWidth) : text;
			return arg.PadRight(totalWidth, paddingChar);
		}

		public static string GetMD5(this string input)
		{
			// step 1, calculate MD5 hash from input
			var md5 = MD5.Create();
			byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder(33);
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}
	}
}
