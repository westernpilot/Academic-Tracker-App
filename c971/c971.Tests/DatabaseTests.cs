using NUnit.Framework;
using System;
using c971.Shared;

namespace c971.Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        private Database _database;

        [SetUp]
        public void Setup()
        {
            // Initialize the database with a test path
            _database = new Database("test_database.db");
        }

        [Test]
        public void SaveTerm_ShouldSaveTermSuccessfully()
        {
            // Arrange
            var term = new AcademicTerm
            {
                Title = "Test Term",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 6, 30)
            };

            // Act
            var result = _database.SaveTermAsync(term).Result;

            // Assert
            Assert.AreEqual(1, result); // Assert that one row was affected (inserted)
            Assert.AreNotEqual(0, term.Id); // Assert that the term ID was set after saving
        }
    }
}
