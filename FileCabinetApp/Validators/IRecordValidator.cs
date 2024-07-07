using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Interface for record validators.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates the parameters of a file cabinet record.
        /// </summary>
        /// <param name="firstName">The first name of the record.</param>
        /// <param name="lastName">The last name of the record.</param>
        /// <param name="dateOfBirth">The date of birth of the record.</param>
        /// <param name="age">The age of the record.</param>
        /// <param name="salary">The salary of the record.</param>
        /// <param name="gender">The gender of the record.</param>
        void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender);
    }
}