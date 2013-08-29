using System;

namespace T1.CoreUtils
{
	public static class GuidExtensions
	{
		public static Boolean IsEmpty(this Guid input)
		{
			return Guid.Empty.Equals(input);
		}

		public static Boolean IsEmpty(this Guid? input)
		{
			return Guid.Empty.Equals(input.GetValueOrDefault());
		}

		public static Boolean IsNullOrEmpty(this Guid? input)
		{
			return Guid.Empty.Equals(input.GetValueOrDefault());
		}

		public static Guid? CheckEmpty(this Guid input)
		{
			if (Guid.Empty.Equals(input)) return null;
			return input;
		}

		public static Guid? CheckEmpty(this Guid? input)
		{
			if (!input.HasValue) return null;
			return CheckEmpty(input.Value);
		}

		public static string AsToken(this Guid input)
		{
			return Convert.ToBase64String(input.ToByteArray()).Replace("==","").Replace('+','_').Replace('/','-');
		}

		public static Guid? ToGuid(this object o, bool nullIfGuidEmpty = true)
		{
			try
			{
				if (o == null || DBNull.Value.Equals(o)) return null;
				var ret = o.ToString().AsGuid();
				if (ret.Equals(Guid.Empty)) return null;
				return ret;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static Guid ToGuid(this object o, Guid defaultIfNullOrEmpty)
		{
			var ret = ToGuid(o);
			if (ret.HasValue) return ret.Value;
			return defaultIfNullOrEmpty;
		}
		
		public static Guid ToGuid(this DBNull n, Guid defaultIfNullOrEmpty)
		{
			return defaultIfNullOrEmpty;
		}

		public static Guid? ToGuid(this DBNull n)
		{
			return null;
		}


	}
}
