using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using T1.CoreUtils;
using System.Configuration;
using System.Text;
using T1.CoreUtils.Authorization;

namespace T1.CoreUtils.Authorization
{
    public static class AuthUtility
	{
        public static void HandlePostAuthorizeRequest<TIdType, TRoleEnum>(HttpApplication app) where TRoleEnum : struct, IConvertible
		{
            var u = app.GetLoggedInUser<TIdType, TRoleEnum>();

            if (u != null) {
                app.Context.User = u;
            }
		}

        public static LoginUser<TIdType, TRoleEnum> GetLoggedInUser<TIdType, TRoleEnum>(this HttpApplication app) where TRoleEnum : struct, IConvertible
        {
            var ret = app.Context.Items["__CURRENT_USER__"] as LoginUser<TIdType, TRoleEnum>;
            if (ret != null) return ret;

            var c = app.Request.Cookies["auth"];
            if (c != null) {
                try
                {
                    var ticket = JS.Parse(app.Decrypt(c.Value)).ToObject<LoginTicket<TIdType, TRoleEnum>>();
                    if (!ticket.Expired && ticket.User != null) {
                        //active
                        app.Context.Items["__CURRENT_USER__"] = ticket.User;
                        return ticket.User;
                    }
                    //expired
                    ClearAuthenticationTicket(app);
                }
                catch (Exception)
                {
                    //invalid
                    ClearAuthenticationTicket(app);
                }
            }
            return null;
        }

        public static void ClearAuthenticationTicket(this HttpApplication app)
        {
            var cookie = new HttpCookie("auth", "");
            cookie.Expires = new DateTime(1970, 1, 1);
            app.Response.Cookies.Remove("auth");
            app.Response.Cookies.Add(cookie);
        }

        public static void SetAuthenticationTicket<TIdType, TRoleEnum>(this HttpApplication app, LoginUser<TIdType, TRoleEnum> user, bool remember) where TRoleEnum : struct, IConvertible
        {
            var ticket = new LoginTicket<TIdType, TRoleEnum>(user, remember);
            var cookie = new HttpCookie("auth", app.Encrypt(JS.Stringify(ticket)));
            if (remember) cookie.Expires = DateTime.Now.AddDays(14);
            app.Response.Cookies.Remove("auth");
            app.Response.Cookies.Add(cookie);
        }
	}
}