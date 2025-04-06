using System;
using Models;

namespace ViewModels
{
    public class PersonViewModelOldStyle : ViewModelBase, IPersonVM
    {
        private readonly Person _person;
        private string _firstName;
        private string _lastName;
        private DateTime _dateOfBirth;

        public PersonViewModelOldStyle()
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

        public string   DisplayText => !string.IsNullOrWhiteSpace(FirstName) 
                                        || !string.IsNullOrWhiteSpace(LastName) 
                                            ? $"{FirstName} {LastName}, is {Age} years old"
                                            : string.Empty;
    }
}
