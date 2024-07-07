using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates the gender parameter of a FileCabinetRecord.
    /// </summary>
    public class GenderValidator : IRecordValidator
    {
        private readonly char[] validGenders;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenderValidator"/> class.
        /// </summary>
        /// <param name="validGenders">An array of valid gender characters.</param>
        public GenderValidator(char[] validGenders)
        {
            this.validGenders = validGenders;
        }

        /// <summary>
        /// Validates the gender parameter of a FileCabinetRecord.
        /// </summary>
        /// <param name="firstName">The first name of the record.</param>
        /// <param name="lastName">The last name of the record.</param>
        /// <param name="dateOfBirth">The date of birth of the record.</param>
        /// <param name="age">The age of the record.</param>
        /// <param name="salary">The salary of the record.</param>
        /// <param name="gender">The gender of the record.</param>
        /// <exception cref="ArgumentException">Thrown when the gender is not valid.</exception>
        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (!this.validGenders.Contains(gender))
            {
                throw new ArgumentException($"Gender must be one of the following: {this.validGenders}.");
            }
        }
    }
}
