using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Composite validator class that combines multiple validators.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">The list of validators to combine.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = new List<IRecordValidator>(validators);
        }

        /// <summary>
        /// Validates the parameters using the combined validators.
        /// </summary>
        /// <param name="firstName">The first name to validate.</param>
        /// <param name="lastName">The last name to validate.</param>
        /// <param name="dateOfBirth">The date of birth to validate.</param>
        /// <param name="age">The age to validate.</param>
        /// <param name="salary">The salary to validate.</param>
        /// <param name="gender">The gender to validate.</param>
        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(firstName, lastName, dateOfBirth, age, salary, gender);
            }
        }
    }
}
