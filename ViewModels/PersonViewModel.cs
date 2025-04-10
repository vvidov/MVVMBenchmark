using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace ViewModels;

public partial class PersonViewModel : ObservableObject, IPersonVM
{
    private readonly Person _person;
    private readonly IMessageBoxService _messageBoxService;

    public PersonViewModel() : this(null)
    {
    }

    public PersonViewModel(IMessageBoxService? messageBoxService)
    {
        _messageBoxService = messageBoxService ?? new DefaultMessageBoxService();
        _person = new Person
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            DateOfBirth = DateTime.Today
        };
    }

    public string FirstName
    {
        get => _person.FirstName;
        set
        {
            if (_person.FirstName != value)
            {
                _person.FirstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayText));
                OnPropertyChanged(nameof(CanUpdatePerson));
                SaveCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public string LastName
    {
        get => _person.LastName;
        set
        {
            if (_person.LastName != value)
            {
                _person.LastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayText));
                OnPropertyChanged(nameof(CanUpdatePerson));
                SaveCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public DateTime DateOfBirth
    {
        get => _person.DateOfBirth;
        set
        {
            if (_person.DateOfBirth != value)
            {
                _person.DateOfBirth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Age));
                OnPropertyChanged(nameof(DisplayText));
            }
        }
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
        // In a real application, this would save the person model to a database or service
        // The model is already up to date since we're using its properties directly
    }
}
