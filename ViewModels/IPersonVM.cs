using System;
using System.ComponentModel;

namespace ViewModels
{
    public interface IPersonVM : INotifyPropertyChanged
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DateOfBirth { get; set; }
        int Age { get; }
        string DisplayText { get; }
    }
}
