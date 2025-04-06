

using System;
using System.ComponentModel;
using ViewModels;
using Xunit;

namespace ViewModels.Tests
{
    public class PersonViewModelTests
    {
        [Fact]
        public void DisplayText_ShouldBeEmpty_WhenNamesAreEmpty()
        {
            // Arrange
            var viewModel = new PersonViewModel();

            // Act
            var displayText = viewModel.DisplayText;

            // Assert
            Assert.Equal(string.Empty, displayText);
        }

        [Fact]
        public void DisplayText_ShouldContainFullNameAndAge()
        {
            // Arrange
            var viewModel = new PersonViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            // Act
            var displayText = viewModel.DisplayText;

            // Assert
            var expectedAge = DateTime.Today.Year - 2000;
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 1, 1))
                expectedAge--;

            Assert.Equal($"John Doe, is {expectedAge} years old", displayText);
        }

        [Fact]
        public void PropertyChanged_ShouldBeRaised_WhenFirstNameChanges()
        {
            // Arrange
            var viewModel = new PersonViewModel();
            var propertyChangedRaised = false;
            viewModel.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == nameof(PersonViewModel.DisplayText))
                    propertyChangedRaised = true;
            };

            // Act
            viewModel.FirstName = "John";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void PropertyChanged_ShouldBeRaised_WhenLastNameChanges()
        {
            // Arrange
            var viewModel = new PersonViewModel();
            var propertyChangedRaised = false;
            viewModel.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == nameof(PersonViewModel.DisplayText))
                    propertyChangedRaised = true;
            };

            // Act
            viewModel.LastName = "Doe";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void PropertyChanged_ShouldBeRaised_WhenDateOfBirthChanges()
        {
            // Arrange
            var viewModel = new PersonViewModel();
            var agePropertyChanged = false;
            var displayTextPropertyChanged = false;
            viewModel.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == nameof(PersonViewModel.Age))
                    agePropertyChanged = true;
                if (e.PropertyName == nameof(PersonViewModel.DisplayText))
                    displayTextPropertyChanged = true;
            };

            // Act
            viewModel.DateOfBirth = new DateTime(2000, 1, 1);

            // Assert
            Assert.True(agePropertyChanged);
            Assert.True(displayTextPropertyChanged);
        }

        [Fact]
        public void Age_ShouldReturnCorrectAge_WhenDateOfBirthIsValid()
        {
            // Arrange
            var viewModel = new PersonViewModel
            {
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            // Act
            var age = viewModel.Age;

            // Assert
            var expectedAge = DateTime.Today.Year - 2000;
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 1, 1))
            {
                expectedAge--;
            }
            Assert.Equal(expectedAge, age);
        }

        [Fact]
        public void Age_ShouldHandleFutureDates()
        {
            // Arrange
            var futureDate = DateTime.Today.AddYears(1);
            var viewModel = new PersonViewModel
            {
                DateOfBirth = futureDate
            };

            // Act
            var age = viewModel.Age;

            // Assert
            Assert.Equal(0, age);
        }
    }
}
