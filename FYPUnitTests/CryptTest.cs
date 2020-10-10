using FYP.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FYPUnitTests
{
    [TestClass]
    public class CryptTest
    {
        [TestMethod]
        public void ShouldEncryptAndDecryptString()
        {
            //Given
            const string expectedString = "someString";

            //When
            string encryptedString = expectedString.Encrypt();
            string decryptedString = encryptedString.Decrypt();

            //Then
            Assert.AreNotEqual(expectedString, encryptedString);
            Assert.AreEqual(expectedString, decryptedString);
        }
    }
}