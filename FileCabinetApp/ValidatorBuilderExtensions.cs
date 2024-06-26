using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class ValidatorBuilderExtensions
    {
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            return builder
                .ValidateFirstName(2, 60)
                .ValidateLastName(2, 60)
                .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now)
                .ValidateAge(0, 120)
                .ValidateSalary(0, 1000000)
                .ValidateGender(new char[] { 'M', 'F' })
                .Create();
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            return builder
                .ValidateFirstName(3, 50)
                .ValidateLastName(3, 50)
                .ValidateDateOfBirth(new DateTime(1960, 1, 1), DateTime.Now.AddYears(-18))
                .ValidateAge(18, 100)
                .ValidateSalary(1000, 1000000)
                .ValidateGender(new[] { 'M', 'F', 'N', 'B' })
                .Create();
        }
    }
}
