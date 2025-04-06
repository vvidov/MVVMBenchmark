using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using System;

namespace ViewModels;

public partial class PersonViewModel : ObservableObject, IPersonVM
{
    private readonly Person _person;

    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private DateTime dateOfBirth = DateTime.Today;

    public PersonViewModel()
    {
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
        OnPropertyChanged(nameof(DisplayText));
    }

    partial void OnLastNameChanged(string value)
    {
        _person.LastName = value;
        OnPropertyChanged(nameof(DisplayText));
    }

    partial void OnDateOfBirthChanged(DateTime value)
    {
        _person.DateOfBirth = value;
        OnPropertyChanged(nameof(Age));
        OnPropertyChanged(nameof(DisplayText));
    }

    public int Age => _person.Age;

    public string DisplayText => !string.IsNullOrWhiteSpace(FirstName) 
        || !string.IsNullOrWhiteSpace(LastName) 
            ? $"{FirstName} {LastName}, is {Age} years old"
            : string.Empty;
}
