using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Provides a builder pattern for constructing composite validators.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        /// Adds a first name validator to the composite validator.
        /// </summary>
        /// <param name="minLength">The minimum length of the first name.</param>
        /// <param name="maxLength">The maximum length of the first name.</param>
        /// <returns>The current instance of ValidatorBuilder.</returns>
        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        /// <summary>
        /// Adds a last name validator to the composite validator.
        /// </summary>
        /// <param name="minLength">The minimum length of the last name.</param>
        /// <param name="maxLength">The maximum length of the last name.</param>
        /// <returns>The current instance of ValidatorBuilder.</returns>
        public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new LastNameValidator(minLength, maxLength));
            return this;
        }

        /// <summary>
        /// Adds a date of birth validator to the composite validator.
        /// </summary>
        /// <param name="from">The earliest valid date of birth.</param>
        /// <param name="to">The latest valid date of birth.</param>
        /// <returns>The current instance of ValidatorBuilder.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>
        /// Adds an age validator to the composite validator.
        /// </summary>
        /// <param name="min">The minimum valid age.</param>
        /// <param name="max">The maximum valid age.</param>
        /// <returns>The current instance of ValidatorBuilder.</returns>
        public ValidatorBuilder ValidateAge(short min, short max)
        {
            this.validators.Add(new AgeValidator(min, max));
            return this;
        }

        /// <summary>
        /// Adds a salary validator to the composite validator.
        /// </summary>
        /// <param name="min">The minimum valid salary.</param>
        /// <param name="max">The maximum valid salary.</param>
        /// <returns>The current instance of ValidatorBuilder.</returns>
        public ValidatorBuilder ValidateSalary(decimal min, decimal max)
        {
            this.validators.Add(new SalaryValidator(min, max));
            return this;
        }

        /// <summary>
        /// Adds a gender validator to the composite validator.
        /// </summary>
        /// <param name="validGenders">An array of valid gender characters.</param>
        /// <returns>The current instance of ValidatorBuilder.</returns>
        public ValidatorBuilder ValidateGender(char[] validGenders)
        {
            this.validators.Add(new GenderValidator(validGenders));
            return this;
        }

        /// <summary>
        /// Creates a composite validator from the added validators.
        /// </summary>
        /// <returns>A composite validator that validates all added criteria.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
