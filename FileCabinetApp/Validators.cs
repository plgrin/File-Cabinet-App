using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Provides methods for validating various data types.
    /// </summary>
    public static class Validators
    {
        /// <summary>
        /// Validates a first name.
        /// </summary>
        /// <param name="firstName">The first name to validate.</param>
        /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
        public static Tuple<bool, string> FirstNameValidator(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                return Tuple.Create(false, "First name must be between 2 and 60 characters long.");
            }

            return Tuple.Create(true, string.Empty);
        }

        /// <summary>
        /// Validates a last name.
        /// </summary>
        /// <param name="lastName">The last name to validate.</param>
        /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
        public static Tuple<bool, string> LastNameValidator(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                return Tuple.Create(false, "Last name must be between 2 and 60 characters long.");
            }

            return Tuple.Create(true, string.Empty);
        }

        /// <summary>
        /// Validates a date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to validate.</param>
        /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
        public static Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                return Tuple.Create(false, "Date of birth must be between 01-Jan-1950 and today.");
            }

            return Tuple.Create(true, string.Empty);
        }

        /// <summary>
        /// Validates an age.
        /// </summary>
        /// <param name="age">The age to validate.</param>
        /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
        public static Tuple<bool, string> AgeValidator(short age)
        {
            if (age < 0 || age > 120)
            {
                return Tuple.Create(false, "Age must be between 0 and 120.");
            }

            return Tuple.Create(true, string.Empty);
        }

        /// <summary>
        /// Validates a salary.
        /// </summary>
        /// <param name="salary">The salary to validate.</param>
        /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
        public static Tuple<bool, string> SalaryValidator(decimal salary)
        {
            if (salary < 0)
            {
                return Tuple.Create(false, "Salary must be a positive number.");
            }

            return Tuple.Create(true, string.Empty);
        }

        /// <summary>
        /// Validates a gender.
        /// </summary>
        /// <param name="gender">The gender to validate.</param>
        /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
        public static Tuple<bool, string> GenderValidator(char gender)
        {
            if (gender != 'M' && gender != 'F')
            {
                return Tuple.Create(false, "Gender must be 'M' or 'F'.");
            }

            return Tuple.Create(true, string.Empty);
        }
    }
}
