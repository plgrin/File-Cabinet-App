using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates the salary parameter of a file cabinet record.
    /// </summary>
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal min;
        private readonly decimal max;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryValidator"/> class.
        /// </summary>
        /// <param name="min">The minimum valid salary.</param>
        /// <param name="max">The maximum valid salary.</param>
        public SalaryValidator(decimal min, decimal max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Validates the salary parameter.
        /// </summary>
        /// <param name="firstName">The first name of the record (not used in this validator).</param>
        /// <param name="lastName">The last name of the record (not used in this validator).</param>
        /// <param name="dateOfBirth">The date of birth of the record (not used in this validator).</param>
        /// <param name="age">The age of the record (not used in this validator).</param>
        /// <param name="salary">The salary to validate.</param>
        /// <param name="gender">The gender of the record (not used in this validator).</param>
        /// <exception cref="ArgumentException">Thrown when the salary is outside the valid range.</exception>
        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (salary < this.min || salary > this.max)
            {
                throw new ArgumentException($"Salary must be between {this.min} and {this.max}.");
            }
        }
    }
}
