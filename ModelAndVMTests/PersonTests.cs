using System;
using Models;
using Xunit;

namespace Models.Tests
{
    public class PersonTests
    {
        [Fact]
        public void Age_ShouldReturnCorrectAge_WhenDateOfBirthIsValid()
        {
            // Arrange
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            // Act
            var age = person.Age;

            // Assert
            var expectedAge = DateTime.Today.Year - 2000;
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 1, 1))
            {
                expectedAge--;
            }
            Assert.Equal(expectedAge, age);
        }

        [Fact]
        public void Age_ShouldReturnZero_WhenDateOfBirthIsInTheFuture()
        {
            // Arrange
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Today.AddYears(1)
            };

            // Act
            var age = person.Age;

            // Assert
            Assert.Equal(0, age);
        }

        [Fact]
        public void Age_ShouldDecrementByOne_WhenBirthdayHasNotOccurredThisYear()
        {
            // Arrange
            var today = DateTime.Today;
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(2000, 12, 31)
            };

            // Act
            var age = person.Age;

            // Assert
            var expectedAge = today.Year - 2000;
            if (today < new DateTime(today.Year, 12, 31))
                expectedAge--;

            Assert.Equal(expectedAge, age);
        }
    }
}
