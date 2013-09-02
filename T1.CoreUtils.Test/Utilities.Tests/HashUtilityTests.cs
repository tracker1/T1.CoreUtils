using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace T1.CoreUtils.Test.Utilities.Tests
{
    [TestClass]
    public class HashUtilityTests
    {
        [TestMethod]
        public void TestSha512()
        {
            var plain = "This is î╥≤ what it is.";
            var encodedBase64 = HashUtility.HashSha512(plain, null);
            var expectedBase64 = "hjAWsX21sZnXmDlf8wbHUae5eifEBtpvhrbZYmB2splezxomuXR/gbwYRUdAlJkJihRTqpnkv1BNQ9W0ozyLEg==";
            Assert.AreEqual(encodedBase64, expectedBase64);
        }

        [TestMethod]
        public void TestScrypt()
        {
            var plain = "This is î╥≤ what it is.";
            var encodedBase64 = HashUtility.HashSCrypt(plain, null);
            var expectedBase64 = "XzjdgY+VbzIiT157VMAClCtimhFk0exI36PNY0SygEH93SXsh9aVuGbvM+g5W7sBV8GFlMvgCwwwUlMAPd+6XQ==";
            Assert.AreEqual(encodedBase64, expectedBase64);
        }
    }
}
