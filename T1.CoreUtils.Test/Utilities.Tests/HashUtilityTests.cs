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
            var expectedBase64 = "h5MvzhswjghT7wGiq97n9nBN8Ey3jNMynrTploAl95AI+1Tu7aI6Jy7AWPT/XCwHLiAxGfSQsgl5mIbEe4A1/Q==";
            Assert.AreEqual(encodedBase64, expectedBase64);
        }
    }
}
