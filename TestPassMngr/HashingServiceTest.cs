using PassMngr;
using PassMngr.Services;
using Logger;

namespace TestPassMngr
{
    [TestClass]
    public class HashingServiceTest
    {
        [TestMethod]
        public void TestHashing()
        {
            string hash1 = "my_random_start_string231";
            string hash2 = "my_random_start_string231";
            LoggerService logger = new LoggerService();
            HashingService hashingService = new HashingService(logger);
            string hashedString1 = hashingService.HashString(hash1);
            string hashedString2 = hashingService.HashString(hash2);
            Assert.AreEqual(hashedString1, hashedString2);
        }
        [TestMethod]
        public void addSpecificPepper()
        {
            string hash = "my_random_start_string231";
            LoggerService logger = new LoggerService();
            HashingService hashingService = new HashingService(logger);
            string pepperedString = hashingService.addPepper(hash, "pepper");
            Assert.AreEqual(pepperedString, "my_random_start_string231pepper");
        }
        [TestMethod]
        public void addRandomPepper() 
        {
            string hash = "my_random_start_string231";
            LoggerService logger = new LoggerService();
            HashingService hashingService = new HashingService(logger);
            string pepperedString = hashingService.addPepper(hash);
            Assert.IsTrue(pepperedString.Count() > hash.Count());
        }
    }
}

