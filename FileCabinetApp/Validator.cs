using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    public static class Validator
    {
        public static readonly Action<string> FirstNameValidator = value => new FirstNameValidator(2, 60).ValidateParameters(value, null, default, default, default, default);
        public static readonly Action<string> LastNameValidator = value => new LastNameValidator(2, 60).ValidateParameters(null, value, default, default, default, default);
        public static readonly Action<DateTime> DateOfBirthValidator = value => new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now).ValidateParameters(null, null, value, default, default, default);
        public static readonly Action<short> AgeValidator = value => new AgeValidator(0, 120).ValidateParameters(null, null, default, value, default, default);
        public static readonly Action<decimal> SalaryValidator = value => new SalaryValidator(0, 1000000).ValidateParameters(null, null, default, default, value, default);
        public static readonly Action<char> GenderValidator = value => new GenderValidator(new[] { 'M', 'F' }).ValidateParameters(null, null, default, default, default, value);
    }
}
