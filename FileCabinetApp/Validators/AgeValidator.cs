using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates the age parameter of a FileCabinetRecord.
    /// </summary>
    public class AgeValidator : IRecordValidator
    {
        private readonly short min;
        private readonly short max;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgeValidator"/> class.
        /// </summary>
        /// <param name="min">The minimum valid age.</param>
        /// <param name="max">The maximum valid age.</param>
        public AgeValidator(short min, short max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Validates the age parameter of a FileCabinetRecord.
        /// </summary>
        /// <param name="firstName">The first name of the record.</param>
        /// <param name="lastName">The last name of the record.</param>
        /// <param name="dateOfBirth">The date of birth of the record.</param>
        /// <param name="age">The age of the record.</param>
        /// <param name="salary">The salary of the record.</param>
        /// <param name="gender">The gender of the record.</param>
        /// <exception cref="ArgumentException">Thrown when the age is not within the valid range.</exception>
        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (age < this.min || age > this.max)
            {
                throw new ArgumentException($"Age must be between {this.min} and {this.max}.");
            }
        }
    }
}
