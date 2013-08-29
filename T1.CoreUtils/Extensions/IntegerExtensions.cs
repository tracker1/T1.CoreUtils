using System;
using System.Collections.Generic;

namespace T1.CoreUtils
{
	public static class IntegerExtensions
	{
		public const string Base26Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		internal const string Base33Safe = "abcdefghijkmnopqrstuvwxyz23456789";
		internal const string Base33SafeMixed = "zty83ceq4xwdornabgup52s9khj6mi7fv";

		public static string AsBase26Alpha(this ulong input)
		{
			return AsArbitraryBase(input, Base26Alpha.ToCharArray());
		}

		public static string AsBase33Safe(this ulong input)
		{
			return AsArbitraryBase(input, Base33Safe.ToCharArray());
		}

		public static string AsCertificationNumber(this ulong input)
		{
			return AsArbitraryBase(input, Base33SafeMixed.ToCharArray());
		}

		public static string AsArbitraryBase(this ulong input, char[] keys)
		{
			if (keys == null || keys.Length < 2) return null; //invalid input
			if (input <= 0) return keys[0].ToString(); //first entry in keys

			var ret = new Stack<char>();
			ulong process = input;
			ulong current = 0;

			var done = false;
			while (!done)
			{
				current = process % (ulong)keys.Length;
				ret.Push(keys[current]);

				process = (ulong)Math.Floor((decimal)process / (decimal)keys.Length);
				if (process == 0) 
				{
					done = true;
				}
			}

			return new String(ret.ToArray());
		}

		public static string AsBase26Alpha(this long input)
		{
			return AsArbitraryBase(input, Base26Alpha.ToCharArray());
		}

		public static string AsBase33Safe(this long input)
		{
			return AsArbitraryBase(input, Base33Safe.ToCharArray());
		}

		public static string AsCertificationNumber(this long input)
		{
			return AsArbitraryBase(input, Base33SafeMixed.ToCharArray());
		}

		public static string AsArbitraryBase(this long input, char[] keys)
		{
			if (keys == null || keys.Length < 2) return null; //invalid input
			if (input <= 0) return keys[0].ToString(); //first entry in keys

			var ret = new Stack<char>();
			long process = input;
			long current = 0;

			while (process > 0)
			{
				current = process % keys.Length;
				process = (int)Math.Floor((decimal)process / (decimal)keys.Length);
				ret.Push(keys[current]);
			}

			return new String(ret.ToArray());
		}

	}
}
