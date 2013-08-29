using System;
using System.Configuration;

namespace T1.CoreUtils
{
    public static class App
    {
        public static bool UseDebugMode
        {
            get
            {
                //default to false
                var ret = false;
#if DEBUG
                //if in debug, default is true
                ret = true;
#endif
                try
                {
                    //if there is a context
                    if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null)
                    {
                        // if request is local, set to true
                        ret = ret || System.Web.HttpContext.Current.Request.IsLocal;
                    }
                    else
                    {
                        //not http based, use debug details
                        ret = true;
                    }
                }
                catch (Exception)
                {
                    ret = true;
                }

                //override with a "Debug" AppSetting, if present
                ret = ConfigurationManager.AppSettings["Debug"].AsBoolean(ret);

                //return the result
                return ret;

            }
        }
    }
}
