using NUnit.Framework;
using System;
using c971.Shared;
using c971.Shared.Models;

namespace c971.Tests
{
    public class UserTests
    {
        [Test]
        public void User_InvalidEmail_ShouldThrowException()
        {
            // Arrange
            var user = new GraduateUser();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => user.Email = "invalidemail");
        }
    }
}
