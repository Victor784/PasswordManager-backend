using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PassMngr.DBContext;
using PassMngr.Models;
using PassMngr.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPassMngr
{
    [TestClass]
    public class PasswordRepositoryTest
    {
        //TestContext? testContext { get; set; }

        private ApplicationDbContext _context;
        private PasswordRepository _passwordRepository;

        [TestInitialize]
        public void TestInitializeF()
        {
            //testContext.WriteLine("Start of TestInitializeF");
            //In-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestPasswordDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _passwordRepository = new PasswordRepository(_context);
            //testContext.WriteLine("Finish of TestInitializeF");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //testContext.WriteLine("Start of TestCleanup");
            // Clean up the in-memory database after each test
            _context.Passwords.RemoveRange(_context.Passwords);
            _context.SaveChanges();
            Assert.AreEqual(0, _context.Passwords.Count());
            _context.Dispose();
            //testContext.WriteLine("Finish of TestCleanup");
        }

        [TestMethod]
        public void TestAddPasswordAndGetById()
        {
            //testContext.WriteLine("Start of TestAddPasswordAndGetById");
            Password password = new Password
            {
                id = 1,
                user_id = 1,
                associated_website = "example.com",
                associated_email = "user@example.com",
                password_value = "password123",
                time_of_creation = "12-02-2023",
                time_of_last_update = "12-02-2023",
                months_until_expired = "12-03-2023"
            };
            _passwordRepository.Add(password);
            Password retrievedPassword = _passwordRepository.GetById(1);

            // Assert
            Assert.IsNotNull(retrievedPassword);
            Assert.AreEqual(password.id, retrievedPassword.id);
            Assert.AreEqual(password.user_id, retrievedPassword.user_id);
            Assert.AreEqual(password.associated_website, retrievedPassword.associated_website);
            Assert.AreEqual(password.associated_email, retrievedPassword.associated_email);
            Assert.AreEqual(password.password_value, retrievedPassword.password_value);
            Assert.AreEqual(password.time_of_creation, retrievedPassword.time_of_creation);
            Assert.AreEqual(password.time_of_last_update, retrievedPassword.time_of_last_update);
            Assert.AreEqual(password.months_until_expired, retrievedPassword.months_until_expired);
            //testContext.WriteLine("Finish of TestAddPasswordAndGetById");
        }

        [TestMethod]
        public void TestDeletePassword()
        {
            //testContext.WriteLine("Start of TestDeletePassword");
            // Arrange
            Password password = new Password
            {
                id = 1,
                user_id = 1,
                associated_website = "example.com",
                associated_email = "user@example.com",
                password_value = "password123",
                time_of_creation = "12-02-2023",
                time_of_last_update = "12-02-2023",
                months_until_expired = "12-03-2023"
            };
            _passwordRepository.Add(password);
            Assert.AreEqual(1, _passwordRepository.GetAll().Count());

            // Act
            _passwordRepository.Delete(password);

            // Assert
            Assert.AreEqual (0, _passwordRepository.GetAll().Count());
            //testContext.WriteLine("Finish of TestDeletePassword");
        }

        [TestMethod]
        public void TestUpdatePassword()
        {
            //testContext.WriteLine("Start of TestUpdatePassword");
            // Arrange
            Password password = new Password
            {
                id = 3,
                user_id = 3,
                associated_website = "example.com",
                associated_email = "user@example.com",
                password_value = "password123",
                time_of_creation = "12-02-2023",
                time_of_last_update = "12-02-2023",
                months_until_expired = "12-03-2023"
            };
            _passwordRepository.Add(password);

            // Update the password
            password.associated_website = "updatedexample.com";
            _passwordRepository.Update(3, password);

            // Act
            Password updatedPassword = _passwordRepository.GetById(3);

            // Assert
            Assert.IsNotNull(updatedPassword);
            Assert.AreEqual("updatedexample.com", updatedPassword.associated_website);
            //testContext.WriteLine("Finish of TestUpdatePassword");
        }

        [TestMethod]
        public void TestGetAllPasswords()
        {
            //testContext.WriteLine("Start of TestGetAllPasswords");
            // Arrange
            Password password1 = new Password
            {
                id = 1,
                user_id = 1,
                associated_website = "example.com",
                associated_email = "user@example.com",
                password_value = "password123",
                time_of_creation = "12-02-2023",
                time_of_last_update = "12-02-2023",
                months_until_expired = "12-03-2023"
            };
            Password password2 = new Password
            {
                id = 2,
                user_id = 2,
                associated_website = "example2.com",
                associated_email = "user2@example.com",
                password_value = "password456",
                time_of_creation = "12-02-2023",
                time_of_last_update = "12-02-2023",
                months_until_expired = "12-03-2023"
            };
            _passwordRepository.Add(password1);
            _passwordRepository.Add(password2);

            // Act
            List<Password> allPasswords = _passwordRepository.GetAll();

            // Assert
            Assert.AreEqual(2, allPasswords.Count);
            Assert.IsTrue(allPasswords.Any(p => p.id == 1));
            Assert.IsTrue(allPasswords.Any(p => p.id == 2));
            //testContext.WriteLine("Finish of TestGetAllPasswords");
        }
    }
}
