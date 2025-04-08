using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace ViewModels;

public partial class PersonViewModel2 : ObservableObject, IPersonVM
{
    private readonly Person _person;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyPropertyChangedFor(nameof(CanUpdatePerson))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string firstName = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyPropertyChangedFor(nameof(CanUpdatePerson))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string lastName = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyPropertyChangedFor(nameof(Age))]
    private DateTime dateOfBirth = DateTime.Today;

    private readonly IMessageBoxService _messageBoxService;

    public PersonViewModel2(IMessageBoxService? messageBoxService = null)
    {
        _messageBoxService = messageBoxService ?? new DefaultMessageBoxService();
        _person = new Person
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            DateOfBirth = DateTime.Today
        };
    }

    partial void OnFirstNameChanged(string value)
    {
        _person.FirstName = value;
    }

    partial void OnLastNameChanged(string value)
    {
        _person.LastName = value;
    }

    partial void OnDateOfBirthChanged(DateTime value)
    {
        _person.DateOfBirth = value;
    }

    public int Age => _person.Age;

    public string DisplayText => !string.IsNullOrWhiteSpace(FirstName) 
        || !string.IsNullOrWhiteSpace(LastName) 
            ? $"{FirstName} {LastName}, is {Age} years old"
            : string.Empty;

    private bool CanUpdatePerson => !string.IsNullOrWhiteSpace(FirstName) 
        && !string.IsNullOrWhiteSpace(LastName);

    [RelayCommand]
    private void Reset()
    {
        var result = _messageBoxService.Show("Are you sure?", "Confirm Clear", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Today;
        }
    }

    [RelayCommand(CanExecute = nameof(CanUpdatePerson))]
    private void Save()
    {
        // Update and save the person
        _person.FirstName = FirstName;
        _person.LastName = LastName;
        _person.DateOfBirth = DateOfBirth;
        // Add actual save implementation here
    }
}
