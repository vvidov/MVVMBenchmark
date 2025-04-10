using System.Windows;
using ViewModels;
using Xunit;

namespace ModelAndVMTests;

public static class TestHelper
{
    public static string GetExpectedDisplayText(string firstName, string lastName, DateTime dateOfBirth) => 
        $"{firstName} {lastName}, is {DateTime.Today.Year - dateOfBirth.Year} years old";

    public static bool GetCommandCanExecute(object command) 
    {
        var canExecuteMethod = command.GetType().GetMethod("CanExecute");
        if (canExecuteMethod == null) return false;
        return (bool)canExecuteMethod.Invoke(command, new object?[] { null })!;
    }

    public static void AssertMessageBoxParameters(string text, string caption, MessageBoxButton button, 
        MessageBoxParameters actual)
    {
        Assert.Equal(text, actual.Text);
        Assert.Equal(caption, actual.Caption);
        Assert.Equal(button, actual.Button);
    }

    public static void AssertEmptyPersonState(IPersonVM viewModel)
    {
        Assert.Equal(string.Empty, viewModel.FirstName);
        Assert.Equal(string.Empty, viewModel.LastName);
        Assert.Equal(DateTime.Today, viewModel.DateOfBirth);
        Assert.Equal(string.Empty, viewModel.DisplayText);
    }

    public static void AssertPersonState(IPersonVM viewModel, string firstName, string lastName, DateTime dateOfBirth)
    {
        Assert.Equal(firstName, viewModel.FirstName);
        Assert.Equal(lastName, viewModel.LastName);
        Assert.Equal(dateOfBirth, viewModel.DateOfBirth);
    }
}
