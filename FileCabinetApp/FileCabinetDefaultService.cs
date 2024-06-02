using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetDefaultService : FileCabinetService
    {
        protected override void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name must be between 2 and 60 characters long and cannot be empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Last name must be between 2 and 60 characters long and cannot be empty or whitespace.");
            }

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Date of birth must be between 01-Jan-1950 and the current date.");
            }

            if (age < 0 || age > 120)
            {
                throw new ArgumentException("Age must be a positive number less than or equal to 120.");
            }

            if (salary < 0)
            {
                throw new ArgumentException("Salary must be a positive number.");
            }

            if (!"MF".Contains(gender))
            {
                throw new ArgumentException("Gender must be 'M' or 'F'.");
            }
        }
    }
}
