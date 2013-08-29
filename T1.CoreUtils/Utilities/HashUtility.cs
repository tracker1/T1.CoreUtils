using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1.CoreUtils
{
	public class HashUtility
	{
		public enum SupportedMethods
		{
			SHA512 = 0 //Current Default
		}

		public static byte[] GenerateSalt()
		{
			var ret = new byte[64]; //512bit random salt value
			System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetNonZeroBytes(ret);
			return ret;
		}

		public static byte[] CreateHash(SupportedMethods method, byte[] salt, string input)
		{
			//input normalization
			if (salt == null || salt.Length == 0) salt = new byte[0];
			if (string.IsNullOrWhiteSpace(input)) input = "";
			input = input.Trim();

			switch (method)
			{
				case SupportedMethods.SHA512:
					return CreateHash_SHA512(salt, input);
			}
			return null; //invalid method
		}

		private static byte[] CreateHash_SHA512(byte[] salt, string input)
		{
			var binput = System.Text.Encoding.UTF8.GetBytes(input);

			var merged = new byte[salt.Length + binput.Length];
			salt.CopyTo(merged, 0);
			binput.CopyTo(merged, salt.Length);

			return System.Security.Cryptography.SHA512Managed.Create().ComputeHash(merged);
		}

	}
}
