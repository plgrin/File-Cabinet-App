#pragma warning disable CS8625
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Static class providing default validation actions for various parameters of FileCabinetRecord.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validates the first name of a FileCabinetRecord.
        /// </summary>
        public static readonly Action<string> FirstNameValidator = value => new FirstNameValidator(2, 60).ValidateParameters(value, null, default, default, default, default);

        /// <summary>
        /// Validates the last name of a FileCabinetRecord.
        /// </summary>
        public static readonly Action<string> LastNameValidator = value => new LastNameValidator(2, 60).ValidateParameters(null, value, default, default, default, default);

        /// <summary>
        /// Validates the date of birth of a FileCabinetRecord.
        /// </summary>
        public static readonly Action<DateTime> DateOfBirthValidator = value => new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now).ValidateParameters(null, null, value, default, default, default);

        /// <summary>
        /// Validates the age of a FileCabinetRecord.
        /// </summary>
        public static readonly Action<short> AgeValidator = value => new AgeValidator(0, 120).ValidateParameters(null, null, default, value, default, default);

        /// <summary>
        /// Validates the salary of a FileCabinetRecord.
        /// </summary>
        public static readonly Action<decimal> SalaryValidator = value => new SalaryValidator(0, 1000000).ValidateParameters(null, null, default, default, value, default);

        /// <summary>
        /// Validates the gender of a FileCabinetRecord.
        /// </summary>
        public static readonly Action<char> GenderValidator = value => new GenderValidator(new[] { 'M', 'F' }).ValidateParameters(null, null, default, default, default, value);
    }
}
