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
    private readonly IMessageBoxService _messageBoxService;

    public PersonViewModel2() : this(null)
    {
    }

    public PersonViewModel2(IMessageBoxService? messageBoxService)
    {
        _messageBoxService = messageBoxService ?? new DefaultMessageBoxService();
        _person = new Person
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            DateOfBirth = DateTime.Today
        };

        // Initialize fields from model
        firstName = _person.FirstName;
        lastName = _person.LastName;
        dateOfBirth = _person.DateOfBirth;
        IsInitialized = true;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyPropertyChangedFor(nameof(CanUpdatePerson))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string firstName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyPropertyChangedFor(nameof(CanUpdatePerson))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string lastName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyPropertyChangedFor(nameof(Age))]
    private DateTime dateOfBirth;

    partial void OnFirstNameChanging(string value)
    {
        // Get value from person model initially
        if (!IsInitialized)
        {
            firstName = _person.FirstName;
            IsInitialized = true;
        }
    }

    partial void OnFirstNameChanged(string value)
    {
        _person.FirstName = value;
    }

    partial void OnLastNameChanging(string value)
    {
        // Get value from person model initially
        if (!IsInitialized)
        {
            lastName = _person.LastName;
            IsInitialized = true;
        }
    }

    partial void OnLastNameChanged(string value)
    {
        _person.LastName = value;
    }

    partial void OnDateOfBirthChanging(DateTime value)
    {
        // Get value from person model initially
        if (!IsInitialized)
        {
            dateOfBirth = _person.DateOfBirth;
            IsInitialized = true;
        }
    }

    partial void OnDateOfBirthChanged(DateTime value)
    {
        _person.DateOfBirth = value;
    }

    private bool IsInitialized { get; set; }

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
        // In a real application, this would save the person model to a database or service
        // The model is already up to date since we're using its properties directly
    }
}
