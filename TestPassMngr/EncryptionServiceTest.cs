using PassMngr;
using PassMngr.Services;

namespace TestPassMngr
{
    [TestClass]
    public class EncryptionServiceTest
    {
        [TestMethod]
        public void TestEncryptDecrypt()
        {
            string toBeEncrypted = "my_random_start_string231";
            EncryptionService encryptionService = new EncryptionService();
            string encryptedString = encryptionService.EncryptString(toBeEncrypted);
            string decryptedString = encryptionService.DecryptString(encryptedString);
            Assert.AreEqual(toBeEncrypted, decryptedString);
        }
    }
}