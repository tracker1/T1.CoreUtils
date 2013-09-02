using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace T1.CoreUtils.Test.Utilities.Tests
{
    [TestClass]
    public class CryptoUtilityTests
    {
        [TestMethod]
        public void EncryptReturnsExpectedValue1_unicode_in_plaintext()
        {
            var passkey = "This is my password.";
            var plain = "This is î╥≤ what it is.";
            var encrypted = "9rTbNbfJkYVE2m5d8g/8b/qAfeCU9rbk09Na/Pw0bak=";

            var actual = T1.CoreUtils.Utilities.CryptoUtility.EncryptDefault(plain, passkey);
            Assert.AreEqual(encrypted, actual);
        }

        [TestMethod]
        public void EncryptReturnsExpectedValue2_unicode_in_passkey()
        {
            var passkey = "I am a ≥ò'ÿ boy baby!";
            var plain = "I am the walrus, coo coo cachoo!";
            var encrypted = "j/e+f5JU5yerSvO7FBJzR1tGro0Ie3L8sWYaupRW1JJhraGqBfQ9z+h85VhSzEjD";

            var actual = T1.CoreUtils.Utilities.CryptoUtility.EncryptDefault(plain, passkey);
            Assert.AreEqual(encrypted, actual);
        }

        [TestMethod]
        public void DecryptReturnsExpectedValue1()
        {
            var passkey = "This is my password.";
            var plain = "This is î╥≤ what it is.";
            var encrypted = "9rTbNbfJkYVE2m5d8g/8b/qAfeCU9rbk09Na/Pw0bak=";

            var actual = T1.CoreUtils.Utilities.CryptoUtility.Decrypt(encrypted, passkey);
            Assert.AreEqual(plain, actual);
        }

        [TestMethod]
        public void DecryptReturnsExpectedValue2()
        {
            var passkey = "I am a ≥ò'ÿ boy baby!";
            var plain = "I am the walrus, coo coo cachoo!";
            var encrypted = "j/e+f5JU5yerSvO7FBJzR1tGro0Ie3L8sWYaupRW1JJhraGqBfQ9z+h85VhSzEjD";

            var actual = T1.CoreUtils.Utilities.CryptoUtility.Decrypt(encrypted, passkey);
            Assert.AreEqual(plain, actual);
        }
    }
}
