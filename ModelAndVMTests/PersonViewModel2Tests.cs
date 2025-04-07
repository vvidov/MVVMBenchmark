using ViewModels;

namespace ModelAndVMTests;

public class PersonViewModel2Tests
{
    [Fact]
    public void Constructor_ShouldInitializeWithEmptyValues()
    {
        // Arrange & Act
        var viewModel = new PersonViewModel2();

        // Assert
        Assert.Equal(string.Empty, viewModel.FirstName);
        Assert.Equal(string.Empty, viewModel.LastName);
        Assert.Equal(DateTime.Today, viewModel.DateOfBirth);
        Assert.Equal(string.Empty, viewModel.DisplayText);
    }

    [Fact]
    public void DisplayText_ShouldBeEmpty_WhenNamesAreEmpty()
    {
        // Arrange
        var viewModel = new PersonViewModel2();

        // Act & Assert
        Assert.Equal(string.Empty, viewModel.DisplayText);
    }

    [Fact]
    public void DisplayText_ShouldShowFullInfo_WhenNamesAreSet()
    {
        // Arrange
        var viewModel = new PersonViewModel2
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var displayText = viewModel.DisplayText;

        // Assert
        Assert.Equal($"John Doe, is {DateTime.Today.Year - 1990} years old", displayText);
    }

    [Fact]
    public void Reset_ShouldClearAllValues()
    {
        // Arrange
        var viewModel = new PersonViewModel2
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var resetCommand = viewModel.ResetCommand;
        resetCommand.Execute(null);

        // Assert
        Assert.Equal(string.Empty, viewModel.FirstName);
        Assert.Equal(string.Empty, viewModel.LastName);
        Assert.Equal(DateTime.Today, viewModel.DateOfBirth);
        Assert.Equal(string.Empty, viewModel.DisplayText);
    }

    [Fact]
    public void Save_ShouldUpdatePersonModel()
    {
        // Arrange
        var viewModel = new PersonViewModel2
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var saveCommand = viewModel.SaveCommand;
        saveCommand.Execute(null);

        // Assert - We can verify the person model was updated through the Age property
        Assert.Equal(DateTime.Today.Year - 1990, viewModel.Age);
    }

    [Theory]
    [InlineData("", "Smith", false)]
    [InlineData("John", "", false)]
    [InlineData("", "", false)]
    [InlineData("John", "Smith", true)]
    public void CanSave_ShouldReturnCorrectValue(string firstName, string lastName, bool expectedCanSave)
    {
        // Arrange
        var viewModel = new PersonViewModel2
        {
            FirstName = firstName,
            LastName = lastName
        };

        // Act & Assert
        var saveCommand = viewModel.GetType().GetProperty("SaveCommand")?.GetValue(viewModel);
        Assert.NotNull(saveCommand);
        var canExecuteMethod = saveCommand.GetType().GetMethod("CanExecute");
        Assert.NotNull(canExecuteMethod);
        
        var canExecute = (bool)canExecuteMethod.Invoke(saveCommand, new object?[] { null })!;
        Assert.Equal(expectedCanSave, canExecute);
    }
}
