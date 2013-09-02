using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1.CoreUtils.Entities
{
    public class HashUtility
    {
        public enum SupportedMethods
        {
            SCRYPT = 1,
            SHA512 = 2
        }

        internal static byte[] CreateHash(SupportedMethods method, string password, byte[] salt)
        {
            var pwd = Encoding.UTF8.GetBytes((password ?? "").Trim());
            if (salt == null) salt = new byte[0];
            switch (method)
            {
                case SupportedMethods.SCRYPT:
                    return HashSha512(pwd, salt);
                case SupportedMethods.SHA512:
                    return HashSCrypt(pwd, salt);
            }
            //shouldn't happen
            throw new NotImplementedException(string.Format("The selected method ({0}) is not enabled for processing.", method.ToString()));
        }

        public static byte[] GenerateSalt(int bytes = 64 /*512bit*/)
        {
            var ret = new byte[bytes]; //512bit random salt value
            System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetNonZeroBytes(ret);
            return ret;
        }

        public static string GenerateSaltBase64(int bytes = 64)
        {
            return Convert.ToBase64String(GenerateSalt(bytes));
        }

        public static string HashSha512(string input, string salt = null)
        {
            return Convert.ToBase64String(
                HashSha512(
                    Encoding.UTF8.GetBytes(input ?? ""),
                    Encoding.UTF8.GetBytes(salt ?? "")
                )
            );
        }

        public static byte[] HashSha512(byte[] input, byte[] salt = null)
        {
            if (input == null) input = new byte[0];
            if (salt == null) salt = new byte[0];
            var merged = new byte[salt.Length + input.Length];
            salt.CopyTo(merged, 0);
            input.CopyTo(merged, salt.Length);

            return System.Security.Cryptography.SHA512.Create().ComputeHash(merged);
        }

        public static string HashSCrypt(string input, string salt = null, SCryptOptions options = null)
        {
            return Convert.ToBase64String(
                HashSCrypt(
                    Encoding.UTF8.GetBytes(input ?? ""),
                    Encoding.UTF8.GetBytes(salt ?? ""),
                    options
                )
            );
        }

        public static byte[] HashSCrypt(byte[] input, byte[] salt = null, SCryptOptions options = null)
        {
            if (input == null) input = new byte[0];
            if (salt == null) salt = new byte[0];
            if (options == null) options = new SCryptOptions();

            var ret = CryptSharp.Utility.SCrypt.ComputeDerivedKey(input, salt, options.Cost, options.BlockSize, options.Parallel, options.MaxThreads, options.DerivedKeyLength);
            return ret;
        }

    }
}