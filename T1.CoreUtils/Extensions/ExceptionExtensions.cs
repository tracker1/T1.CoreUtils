using System;

namespace T1.CoreUtils
{
	public static class ExceptionExtensions
	{
		public static Entities.SerialError AsSerialError(this Exception ex)
		{
			return Entities.SerialError.FromObject(ex);
		}
	}
}
