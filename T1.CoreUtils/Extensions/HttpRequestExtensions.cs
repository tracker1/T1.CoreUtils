using System;
using System.IO;
using System.Web;

namespace T1.CoreUtils
{
	public static class HttpRequestExtensions
	{

		public static bool IsAsyncPostBackRequest(this HttpRequest request)
		{
			var values = request.Headers.GetValues("X-MicrosoftAjax");
			if (values != null) {
				foreach (string v in values)
				{
					var opts = v.Split(',');
					foreach (string o in opts)
					{
						if (o.Trim().ToLower() == "delta-true") return true;
					}
				}
			}

			return false;
		}
		
		public static string GetRequestId(this HttpRequest request)
		{
			var id = request.RequestContext.HttpContext.Items["__REQUEST_ID__"] as string;
			if (id == null) 
			{ 
				request.RequestContext.HttpContext.Items["__REQUEST_ID__"] = id = GetNextRequestId();
			}
			return id;
		}

		public static string GetRequestId(this HttpRequestBase request)
		{
			var id = request.RequestContext.HttpContext.Items["__REQUEST_ID__"] as string;
			if (id == null) 
			{ 
				request.RequestContext.HttpContext.Items["__REQUEST_ID__"] = id = GetNextRequestId();
			}
			return id;
		}

		private static object _hnd = new Object();
		private static string _RequestIdPath;
		private static string GetNextRequestId()
		{
			ulong ret = 37059; //starting number "aaaa"

			//lock to reduce the chance of file contention
			lock(_hnd) {
				if (_RequestIdPath == null) {
					if (HttpContext.Current != null) {
						_RequestIdPath = HttpContext.Current.Server.MapPath("~/App_Data/request_id_counter.bin");
					} else {
						_RequestIdPath = "./request_id_counter.bin";
					}
				}
				var b = new byte[0];
				if (File.Exists(_RequestIdPath)) b = File.ReadAllBytes(_RequestIdPath);
				if (b.Length >= 8) {
					ret = BitConverter.ToUInt64(b,0);
					ret++;
				}
				File.WriteAllBytes(_RequestIdPath, BitConverter.GetBytes(ret));
				return ret.AsBase33Safe();
			}
		}

	}
}
