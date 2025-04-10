using System.Windows;
using ViewModels;
using Services;

namespace ModelAndVMTests;

public class PersonViewModel2Tests
{
    [Fact]
    public void Constructor_ShouldInitializeWithEmptyValues()
    {
        // Arrange & Act
        var viewModel = new PersonViewModel2(new DefaultMessageBoxService());

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
        var viewModel = new PersonViewModel2(new DefaultMessageBoxService());

        // Act & Assert
        Assert.Equal(string.Empty, viewModel.DisplayText);
    }

    [Fact]
    public void DisplayText_ShouldShowFullInfo_WhenNamesAreSet()
    {
        // Arrange
        var dateOfBirth = new DateTime(1990, 1, 1);
        var viewModel = new PersonViewModel2(new DefaultMessageBoxService())
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = dateOfBirth
        };

        // Act
        var displayText = viewModel.DisplayText;

        // Assert
        var expected = TestHelper.GetExpectedDisplayText("John", "Doe", dateOfBirth);
        Assert.Equal(expected, displayText);
    }

    [Fact]
    public void Reset_ShouldClearValues_WhenUserConfirms()
    {
        // Arrange
        var messageBoxParams = new MessageBoxParameters();
        var messageBoxService = new TestMessageBoxService((text, caption, button, image) =>
        {
            messageBoxParams.Text = text;
            messageBoxParams.Caption = caption;
            messageBoxParams.Button = button;
            return MessageBoxResult.Yes;
        });

        var dateOfBirth = new DateTime(1990, 1, 1);
        var viewModel = new PersonViewModel2(messageBoxService)
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = dateOfBirth
        };

        // Act
        viewModel.ResetCommand.Execute(null);

        // Assert
        TestHelper.AssertMessageBoxParameters("Are you sure?", "Confirm Clear", MessageBoxButton.YesNo, messageBoxParams);
        TestHelper.AssertEmptyPersonState(viewModel);
    }

    [Fact]
    public void Reset_ShouldKeepValues_WhenUserCancels()
    {
        // Arrange
        var messageBoxParams = new MessageBoxParameters();
        var messageBoxService = new TestMessageBoxService((text, caption, button, image) =>
        {
            messageBoxParams.Text = text;
            messageBoxParams.Caption = caption;
            messageBoxParams.Button = button;
            return MessageBoxResult.No;
        });

        var dateOfBirth = new DateTime(1990, 1, 1);
        var viewModel = new PersonViewModel2(messageBoxService)
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = dateOfBirth
        };

        // Act
        viewModel.ResetCommand.Execute(null);

        // Assert
        TestHelper.AssertMessageBoxParameters("Are you sure?", "Confirm Clear", MessageBoxButton.YesNo, messageBoxParams);
        TestHelper.AssertPersonState(viewModel, "John", "Doe", dateOfBirth);
    }

    [Fact]
    public void Save_ShouldUpdatePersonModel()
    {
        // Arrange
        var viewModel = new PersonViewModel2(new DefaultMessageBoxService())
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var saveCommand = viewModel.SaveCommand;
        saveCommand.Execute(null);

        // Assert
        Assert.Equal(DateTime.Today.Year - 1990, viewModel.Age);
        Assert.Equal("John", viewModel.FirstName);
        Assert.Equal("Doe", viewModel.LastName);
        Assert.Equal(new DateTime(1990, 1, 1), viewModel.DateOfBirth);
    }

    [Theory]
    [InlineData("", "Smith", false)]
    [InlineData("John", "", false)]
    [InlineData("", "", false)]
    [InlineData("John", "Smith", true)]
    public void CanSave_ShouldReturnCorrectValue(string firstName, string lastName, bool expectedCanSave)
    {
        // Arrange
        var viewModel = new PersonViewModel2(new DefaultMessageBoxService())
        {
            FirstName = firstName,
            LastName = lastName
        };

        // Act
        var canExecute = TestHelper.GetCommandCanExecute(viewModel.SaveCommand);

        // Assert
        Assert.Equal(expectedCanSave, canExecute);
    }
}
