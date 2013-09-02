using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1.CoreUtils.Entities
{
    public class HashedPassword
    {
        #region Properties

        public HashUtility.SupportedMethods Method { get; private set; }

        public byte[] Salt { get; private set; }

        public byte[] Hash { get; private set; }

        #endregion

        #region Constructor(s)


        public HashedPassword()
        {
            Method = HashUtility.SupportedMethods.SHA512; //default
        }
        #endregion

        #region Instance Methods
        private void SetPassword(string password, byte[] salt = null, HashUtility.SupportedMethods method = HashUtility.SupportedMethods.SHA512)
        {
            if (salt == null || salt.Length == 0) salt = HashUtility.GenerateSalt();
            byte[] ph = HashUtility.CreateHash(method, password, salt);

            this.Method = method;
            this.Salt = salt;
            this.Hash = ph;
        }
        #endregion

        #region overrides
        public override string ToString()
        {
            var ret = string.Format(
                "{0}\\{1}\\{2}"
                , Method.ToString()
                , Convert.ToBase64String(Salt)
                , Convert.ToBase64String(Hash)
            );
            return ret;
        }
        #endregion

        public static HashedPassword FromString(string hashdetails)
        {
            if (string.IsNullOrWhiteSpace(hashdetails)) return new HashedPassword();
            try
            {
                var tmp = hashdetails.Split('\\');
                var method = tmp[0];
                var salt = tmp[1];
                var hash = tmp[2];

                var ret = new HashedPassword();
                ret.Method = (HashUtility.SupportedMethods)Enum.Parse(typeof(HashUtility.SupportedMethods), method, true);
                ret.Salt = Convert.FromBase64String(salt);
                ret.Hash = Convert.FromBase64String(hash);
                return ret;
            }
            catch (Exception)
            {
                //invalid JSON, assume it's a clear text password set inside the db
                var ret = new HashedPassword();
                ret.SetPassword(hashdetails);
                return ret;
            }
        }

        public static HashedPassword FromPassword(string rawPassword, byte[] salt = null, HashUtility.SupportedMethods method = HashUtility.SupportedMethods.SHA512)
        {
            var ret = new HashedPassword();
            ret.SetPassword(rawPassword, salt, method);
            return ret;
        }
    }
}
