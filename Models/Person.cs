using System;

namespace Models
{
    public class Person
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (age < 0)
                    return 0;
                if( age > 0 && DateOfBirth.Date > today.AddYears(-age))
                    age--;
                return age;
            }
        }
    }
}
