using PassMngr.Models;

namespace TestPassMngr
{
    [TestClass]
    public class PasswordUnitTest
    {
        [TestMethod]
        public void TestPasswordConstruction()
        {
            int id = 1;
            int userId = 123;
            string website = "example.com";
            string email = "user@example.com";
            string password = "my_password";
            string creationTime = "2024-04-02";
            string updateTime = "2024-04-02";
            string expirationDate = "2024-05-02";

            Password newPassword = new Password(id, userId, website, email, password, creationTime, updateTime, expirationDate);

            Assert.AreEqual(id, newPassword.id);
            Assert.AreEqual(userId, newPassword.user_id);
            Assert.AreEqual(website, newPassword.associated_website);
            Assert.AreEqual(email, newPassword.associated_email);
            Assert.AreEqual(password, newPassword.password_value);
            Assert.AreEqual(creationTime, newPassword.time_of_creation);
            Assert.AreEqual(updateTime, newPassword.time_of_last_update);
            Assert.AreEqual(expirationDate, newPassword.expiration_date);
        }

        [TestMethod]
        public void TestDefaultPasswordConstruction()
        {
            Password defaultPassword = new Password();

            Assert.AreEqual(0, defaultPassword.id);
            Assert.AreEqual(0, defaultPassword.user_id);
            Assert.AreEqual("", defaultPassword.associated_website);
            Assert.AreEqual("", defaultPassword.associated_email);
            Assert.AreEqual("", defaultPassword.password_value);
            Assert.AreEqual("", defaultPassword.time_of_creation);
            Assert.AreEqual("", defaultPassword.time_of_last_update);
            Assert.AreEqual("", defaultPassword.expiration_date);
        }
    }
}
