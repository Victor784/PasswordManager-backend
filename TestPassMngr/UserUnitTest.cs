using PassMngr.Models;

namespace TestPassMngr
{
    [TestClass]
    public class UserUnitTest
    {
        [TestMethod]
        public void TestUserConstruction()
        {
            // Arrange
            int id = 1;
            string email = "user@example.com";
            string password = "password123";
            string name = "John";
            string surname = "Doe";
            string dateOfRegistration = DateTime.Now.ToString("yyyy-MM-dd");
            bool isActive = true;
            bool isConfirmed = false;
            

            // Act
            User newUser = new User(id, email, password, name, surname, dateOfRegistration, isActive, isConfirmed);

            // Assert
            Assert.AreEqual(id, newUser.id);
            Assert.AreEqual(email, newUser.email);
            Assert.AreEqual(password, newUser.password);
            Assert.AreEqual(name, newUser.name);
            Assert.AreEqual(surname, newUser.surname);
            Assert.AreEqual(dateOfRegistration, newUser.date_of_registration);
            Assert.AreEqual(isActive, newUser.is_active);
            Assert.AreEqual(isConfirmed, newUser.is_confirmed);

        }

        [TestMethod]
        public void TestDefaultUserConstruction()
        {
            // Arrange & Act
            User defaultUser = new User();

            // Assert
            Assert.AreEqual(0, defaultUser.id);
            Assert.IsNull(defaultUser.email);
            Assert.IsNull(defaultUser.password);
            Assert.IsNull(defaultUser.name);
            Assert.IsNull(defaultUser.surname);
            Assert.AreEqual(null, defaultUser.date_of_registration);
            Assert.IsNull(defaultUser.is_active);
            Assert.IsNull(defaultUser.is_confirmed);
            Assert.IsNotNull(defaultUser.password_list);
            Assert.AreEqual(0, defaultUser.password_list.Count);
        }
    }
}
