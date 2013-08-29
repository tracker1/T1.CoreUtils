using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1.CoreUtils
{
	public class HashUtility
	{
		public static string GenerateSaltBase64(int bytes = 64)
		{
			var ret = new byte[bytes]; //512bit random salt value
			System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetNonZeroBytes(ret);
			return Convert.ToBase64String(ret);
		}

        public static string HashSha512(string input, string saltBase64)
        {
            byte[] salt = string.IsNullOrWhiteSpace(saltBase64) ? new byte[0] : Convert.FromBase64String(saltBase64);
            if (string.IsNullOrWhiteSpace(input)) input = "";
            input = input.Trim();
            return Convert.ToBase64String(CreateHash_SHA512(salt, input));
        }

        public static string HashSCrypt(string input, string saltBase64)
        {
            byte[] salt = string.IsNullOrWhiteSpace(saltBase64) ? new byte[0] : Convert.FromBase64String(saltBase64);
            if (string.IsNullOrWhiteSpace(input)) input = "";
            input = input.Trim();
            return Convert.ToBase64String(CreateHash_SCrypt(salt, input));
        }


        private static byte[] CreateHash_SCrypt(byte[] salt, string input)
        {
            var ret = CryptSharp.Utility.SCrypt.ComputeDerivedKey(Encoding.UTF8.GetBytes(input), salt, 1024, 8, 1, null, 64);
            return ret;
        }

		private static byte[] CreateHash_SHA512(byte[] salt, string input)
		{
			var binput = System.Text.Encoding.UTF8.GetBytes(input);

			var merged = new byte[salt.Length + binput.Length];
			salt.CopyTo(merged, 0);
			binput.CopyTo(merged, salt.Length);

			return System.Security.Cryptography.SHA512Managed.Create().ComputeHash(merged);
		}

        private static byte[] MergeInput(byte[] salt, string input)
        {
            var binput = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] merged;
            if (salt == null)
            {
                merged = binput;
            }
            else
            {
                merged = new byte[salt.Length + binput.Length];
                salt.CopyTo(merged, 0);
                binput.CopyTo(merged, salt.Length);
            }

            return merged;
        }
	}
}
