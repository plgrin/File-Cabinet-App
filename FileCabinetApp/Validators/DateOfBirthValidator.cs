using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates the date of birth parameter of a FileCabinetRecord.
    /// </summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime to;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="from">The earliest valid date of birth.</param>
        /// <param name="to">The latest valid date of birth.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Validates the date of birth parameter of a FileCabinetRecord.
        /// </summary>
        /// <param name="firstName">The first name of the record.</param>
        /// <param name="lastName">The last name of the record.</param>
        /// <param name="dateOfBirth">The date of birth of the record.</param>
        /// <param name="age">The age of the record.</param>
        /// <param name="salary">The salary of the record.</param>
        /// <param name="gender">The gender of the record.</param>
        /// <exception cref="ArgumentException">Thrown when the date of birth is not within the valid range.</exception>
        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (dateOfBirth < this.from || dateOfBirth > this.to)
            {
                throw new ArgumentException($"Date of birth must be between {this.from.ToShortDateString()} and {this.to.ToShortDateString()}.");
            }
        }
    }
}
