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
    public class UserRepositoryTest
    {
        private ApplicationDbContext _context;
        private UserRepository _userRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestUserDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _userRepository = new UserRepository(_context);
        }

        

        [TestMethod]
        public void TestAddUserAndGetById()
        {
            // Arrange
            User user = new User
            {
                id = 1,
                email = "user@example.com",
                password = "password123",
                name = "John",
                surname = "Doe",
                date_of_registration = "12-03-2023",
                is_active = true,
                is_confirmed = false
            };

            // Act
            _userRepository.Add(user);
            User retrievedUser = _userRepository.GetById(1);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(user.id, retrievedUser.id);
            Assert.AreEqual(user.email, retrievedUser.email);
            Assert.AreEqual(user.password, retrievedUser.password);
            Assert.AreEqual(user.name, retrievedUser.name);
            Assert.AreEqual(user.surname, retrievedUser.surname);
            Assert.AreEqual(user.date_of_registration, retrievedUser.date_of_registration);
            Assert.AreEqual(user.is_active, retrievedUser.is_active);
            Assert.AreEqual(user.is_confirmed, retrievedUser.is_confirmed);
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            // Arrange
            User user = new User
            {
                id = 1,
                email = "user@example.com",
                password = "password123",
                name = "John",
                surname = "Doe",
                date_of_registration = "12-02-2023",
                is_active = true,
                is_confirmed = false
            };
            _userRepository.Add(user);
            Assert.AreEqual(1, _userRepository.GetAll().Count());

            _userRepository.Delete(user);
            Assert.AreEqual(0, _userRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestUpdateUser()
        {
            // Arrange
            User user = new User
            {
                id = 3,
                email = "user@example.com",
                password = "password123",
                name = "John",
                surname = "Doe",
                date_of_registration = "12-03-2023",
                is_active = true,
                is_confirmed = false
            };
            _userRepository.Add(user);

            // Update the user
            user.email = "updated@example.com";
            _userRepository.Update(1, user);

            // Act
            User updatedUser = _userRepository.GetById(3);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("updated@example.com", updatedUser.email);
        }

        [TestMethod]

        public void TestGetAllUsers()
        {
            // Arrange
            User user1 = new User
            {
                id = 1,
                email = "user1@example.com",
                password = "password123",
                name = "John",
                surname = "Doe",
                date_of_registration = "12-03-2023",
                is_active = true,
                is_confirmed = false
            };
            User user2 = new User
            {
                id = 2,
                email = "user2@example.com",
                password = "password456",
                name = "Jane",
                surname = "Doe",
                date_of_registration = "12-03-2023",
                is_active = true,
                is_confirmed = false
            };
            _userRepository.Add(user1);
            _userRepository.Add(user2);

            // Act
            List<User> allUsers = _userRepository.GetAll();

            // Assert
            Assert.AreEqual(2, allUsers.Count);
            Assert.IsTrue(allUsers.Any(u => u.id == 1));
            Assert.IsTrue(allUsers.Any(u => u.id == 2));
        }

        [TestCleanup]
        public void TestCleanup()
        {

            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
            Assert.AreEqual(0, _context.Passwords.Count());
            _context.Dispose();
        }
    }
}
