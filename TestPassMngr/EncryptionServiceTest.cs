using PassMngr;
using PassMngr.Services;
using Logger;

namespace TestPassMngr
{
    [TestClass]
    public class EncryptionServiceTest
    {
        [TestMethod]
        public void TestEncryptDecrypt()
        {
            string toBeEncrypted = "my_random_start_string231";
            LoggerService logger = new LoggerService();
            EncryptionService encryptionService = new EncryptionService(logger);
            string encryptedString = encryptionService.EncryptString(toBeEncrypted);
            string decryptedString = encryptionService.DecryptString(encryptedString);
            Assert.AreEqual(toBeEncrypted, decryptedString);
        }
    }
}