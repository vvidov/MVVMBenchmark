using System;
using System.Windows;
using System.Windows.Input;
using Models;
using Services;

namespace ViewModels
{
    public class PersonViewModelOldStyle : ViewModelBase, IPersonVM
    {
        private readonly Person _person;
        private string _firstName;
        private string _lastName;
        private DateTime _dateOfBirth;

        private readonly IMessageBoxService _messageBoxService;

        public PersonViewModelOldStyle(IMessageBoxService? messageBoxService = null)
        {
            _messageBoxService = messageBoxService ?? new DefaultMessageBoxService();
            _person = new Person
            {
                FirstName = string.Empty,
                LastName = string.Empty
            };
            _firstName = string.Empty;
            _lastName = string.Empty;
            DateOfBirth = DateTime.Today;
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                {
                    _person.FirstName = value;
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                {
                    _person.LastName = value;
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (SetProperty(ref _dateOfBirth, value))
                {
                    _person.DateOfBirth = value;
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

        private ICommand? _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ??= new RelayCommand(
                    execute: () =>
                    {
                        var result = _messageBoxService.Show("Are you sure?", "Confirm Clear", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            FirstName = string.Empty;
                            LastName = string.Empty;
                            DateOfBirth = DateTime.Today;
                        }
                    });
            }
        }

        private ICommand? _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(
                    execute: () =>
                    {
                        // Update and save the person
                        _person.FirstName = FirstName;
                        _person.LastName = LastName;
                        _person.DateOfBirth = DateOfBirth;
                        // Add actual save implementation here
                    },
                    canExecute: () => CanUpdatePerson);
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();
    }
}
