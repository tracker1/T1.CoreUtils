using System;
using System.Text.RegularExpressions;
using System.Web;

namespace T1.CoreUtils
{
	public static class HttpContextExtensionMethods
	{
		public static string GetExternalUrl(HttpContext context, string virtualPath)
		{
			if (Regex.Match(@"^\w{2,7}\:", virtualPath, RegexOptions.Singleline).Success) return virtualPath;
			return String.Format(
				"{0}{1}"
				,Regex.Match(context.Request.Url.OriginalString, @"^https?\:\/\/[^\/]+", RegexOptions.Singleline).Value
				,VirtualPathUtility.ToAbsolute(virtualPath)
			);
		}

	}
}
