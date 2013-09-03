using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Text;

namespace T1.CoreUtils
{
    public static class App
    {
        #region UseDebugMode
        public static bool InDebugMode(this HttpApplication app)
        {
            return App.UseDebugMode;
        }

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
        #endregion

        private static object _hnd = new Object();

        private static bool CryptoInitialized;
        private static string AuthPassphrase;
        private static byte[] CryptKey;
        private static byte[] CryptIV;

        private static void IntitializeCrypto()
        {
            if (CryptoInitialized) return;
            lock (_hnd)
            {
                if (CryptoInitialized) return;
                CryptoInitialized = true;
                AuthPassphrase = ConfigurationManager.AppSettings["auth:crypto-keyphrase"] ?? "";
                if (string.IsNullOrWhiteSpace(AuthPassphrase)) throw new ApplicationException("Application Setting 'auth:token-passphrase' Not Set");
                CryptoUtility.PassphraseToSCryptKeyAndIV(AuthPassphrase, out CryptKey, out CryptIV);
            }
        }

        public static string Encrypt(this HttpApplication app, string input)
        {
            IntitializeCrypto();
            var bytes = HashUtility.GenerateSalt(16).ToList();
            bytes.AddRange(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(CryptoUtility.EncryptBytes(bytes.ToArray(), CryptKey, CryptIV));
        }

        public static string Decrypt(this HttpApplication app, string base64input)
        {
            IntitializeCrypto();
            var bytes = CryptoUtility.DecryptBytes(Convert.FromBase64String(base64input), CryptKey, CryptIV);
            var ret = Encoding.UTF8.GetString(bytes, 16, bytes.Length - 16);
            return ret;
        }

    }
}
