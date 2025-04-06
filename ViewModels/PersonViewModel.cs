using System;
using MVVMBenchmark.Models;

namespace MVVMBenchmark.ViewModels
{
    public class PersonViewModel : ViewModelBase, IPersonVM
    {
        private readonly Person _person;
        private string _firstName;
        private string _lastName;
        private DateTime _dateOfBirth;

        public PersonViewModel()
        {
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

        public string DisplayText => $"{FirstName} {LastName}, is {Age} years old";
    }
}
