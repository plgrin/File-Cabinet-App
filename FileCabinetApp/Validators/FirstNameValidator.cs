using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator for the first name field.
    /// </summary>
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">The minimum length of the first name.</param>
        /// <param name="maxLength">The maximum length of the first name.</param>
        public FirstNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <summary>
        /// Validates the first name parameter.
        /// </summary>
        /// <param name="firstName">The first name to validate.</param>
        /// <param name="lastName">The last name to validate (not used).</param>
        /// <param name="dateOfBirth">The date of birth to validate (not used).</param>
        /// <param name="age">The age to validate (not used).</param>
        /// <param name="salary">The salary to validate (not used).</param>
        /// <param name="gender">The gender to validate (not used).</param>
        /// <exception cref="ArgumentException">Thrown when the first name is invalid.</exception>
        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < this.minLength || firstName.Length > this.maxLength)
            {
                throw new ArgumentException($"First name must be between {this.minLength} and {this.maxLength} characters long and cannot be empty or whitespace.");
            }
        }
    }
}
