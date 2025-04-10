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
        private readonly IMessageBoxService _messageBoxService;

        public PersonViewModelOldStyle() : this(null)
        {
        }

        public PersonViewModelOldStyle(IMessageBoxService? messageBoxService)
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
                        // In a real application, this would save the person model to a database or service
                        // The model is already up to date since we're using its properties directly
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
