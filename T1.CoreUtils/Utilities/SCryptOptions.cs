using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1.CoreUtils
{
    public class SCryptOptions
    {
        public const int DefaultCost = 16384; // Math.Pow(2,14) - recommended strength for password hashing
        public const int DefaultBlockSize = 8;
        public const int DefaultParallel = 1;
        public const int DefaultDerivedKeyLength = 64;

        private int? _Cost = null;
        public int Cost {
            get {
                return _Cost ?? DefaultCost;
            }
            set {
                _Cost = CheckCost(value);
            }
        }

        private int? _BlockSize = null;
        public int BlockSize
        {
            get
            {
                return _BlockSize ?? DefaultBlockSize;
            }
            set
            {
                _BlockSize = CheckNumber(value);
            }
        }

        private int? _Parallel = null;
        public int Parallel
        {
            get
            {
                return _Parallel ?? DefaultParallel;
            }
            set
            {
                _Parallel = CheckNumber(value);
            }
        }

        private int? _MaxThreads = null;
        public int? MaxThreads
        {
            get
            {
                return _MaxThreads;
            }
            set
            {
                _MaxThreads = value == null ? null : CheckNumber(value.GetValueOrDefault());
            }
        }

        private int? _DerivedKeyLength = null;
        public int DerivedKeyLength
        {
            get
            {
                return _DerivedKeyLength ?? DefaultDerivedKeyLength;
            }
            set
            {
                _DerivedKeyLength = CheckLength(value);
            }
        }

        private int? CheckCost(int input) {
            if (input < 256) return null;
            if (input > Math.Pow(2, 64)) return null;
            for (var m = 8; m <= 64; m++)
            {
                var checkVal = (int)Math.Pow(2, m);
                if (checkVal > input) return null;
                if (checkVal == input) return checkVal;
            }
            return null;
        }

        private int? CheckNumber(int input) {
            if (input < 1) return null;
            if (input > 256) return null;
            return input;
        }

        private int? CheckLength(int input) {
            if (input < 1) return null;
            if (input > 2048) return null;
            return input;
        }
    }
}
