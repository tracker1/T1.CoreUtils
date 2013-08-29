using System;

namespace T1.CoreUtils
{
	public static class DateTimeExtensions
	{
		public static string ToIsoString(DateTime? dtm)
		{
			if (dtm == null) return string.Empty;
			return ToIsoString(dtm.Value);
		}

		public static string ToIsoString(DateTime dtm)
		{
			return String.Format("{0:yyyy-MM-dd'T'HH:mm:ss'Z'}", dtm.ToUniversalTime());
		}

		public static string ToShortStringUS(DateTime? dtm)
		{
			if (dtm == null) return string.Empty;
			return ToShortStringUS(dtm.Value);
		}

		public static string ToShortStringUS(DateTime dtm)
		{
			return String.Format("{0:MM/dd/yyyy}", dtm);
		}

	}
}
