using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
    {
        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            this.ValidateFirstName(firstName);
            this.ValidateLastName(lastName);
            this.ValidateDateOfBirth(dateOfBirth);
            this.ValidateAge(age);
            this.ValidateSalary(salary);
            this.ValidateGender(gender);
        }

        private void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 3 || firstName.Length > 50)
            {
                throw new ArgumentException("First name must be between 3 and 50 characters long and cannot be empty or whitespace.");
            }
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 3 || lastName.Length > 50)
            {
                throw new ArgumentException("Last name must be between 3 and 50 characters long and cannot be empty or whitespace.");
            }
        }

        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < new DateTime(1960, 1, 1) || dateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new ArgumentException("Date of birth must be between 01-Jan-1960 and 18 years ago.");
            }
        }

        private void ValidateAge(short age)
        {
            if (age < 18 || age > 100)
            {
                throw new ArgumentException("Age must be between 18 and 100.");
            }
        }

        private void ValidateSalary(decimal salary)
        {
            if (salary < 1000 || salary > 1000000)
            {
                throw new ArgumentException("Salary must be between 1,000 and 1,000,000.");
            }
        }

        private void ValidateGender(char gender)
        {
            if (!"MFNB".Contains(gender))
            {
                throw new ArgumentException("Gender must be 'M', 'F', 'N' or 'B'.");
            }
        }
    }
}
