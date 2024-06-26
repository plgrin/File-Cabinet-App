using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Validators;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    public static class ValidatorBuilderExtensions
    {
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder, IConfigurationSection section)
        {
            var firstNameConfig = section.GetSection("firstName");
            var lastNameConfig = section.GetSection("lastName");
            var dateOfBirthConfig = section.GetSection("dateOfBirth");
            var ageConfig = section.GetSection("age");
            var salaryConfig = section.GetSection("salary");
            var genderConfig = section.GetSection("gender");

            return builder
                .ValidateFirstName(firstNameConfig.GetValue<int>("min"), firstNameConfig.GetValue<int>("max"))
                .ValidateLastName(lastNameConfig.GetValue<int>("min"), lastNameConfig.GetValue<int>("max"))
                .ValidateDateOfBirth(dateOfBirthConfig.GetValue<DateTime>("from"), dateOfBirthConfig.GetValue<DateTime>("to"))
                .ValidateAge(ageConfig.GetValue<short>("min"), ageConfig.GetValue<short>("max"))
                .ValidateSalary(salaryConfig.GetValue<decimal>("min"), salaryConfig.GetValue<decimal>("max"))
                .ValidateGender(genderConfig.Get<string[]>().SelectMany(s => s.ToCharArray()).ToArray())
                .Create();
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder, IConfigurationSection section)
        {
            var firstNameConfig = section.GetSection("firstName");
            var lastNameConfig = section.GetSection("lastName");
            var dateOfBirthConfig = section.GetSection("dateOfBirth");
            var ageConfig = section.GetSection("age");
            var salaryConfig = section.GetSection("salary");
            var genderConfig = section.GetSection("gender");

            return builder
                .ValidateFirstName(firstNameConfig.GetValue<int>("min"), firstNameConfig.GetValue<int>("max"))
                .ValidateLastName(lastNameConfig.GetValue<int>("min"), lastNameConfig.GetValue<int>("max"))
                .ValidateDateOfBirth(dateOfBirthConfig.GetValue<DateTime>("from"), dateOfBirthConfig.GetValue<DateTime>("to"))
                .ValidateAge(ageConfig.GetValue<short>("min"), ageConfig.GetValue<short>("max"))
                .ValidateSalary(salaryConfig.GetValue<decimal>("min"), salaryConfig.GetValue<decimal>("max"))
                .ValidateGender(genderConfig.Get<string[]>().SelectMany(s => s.ToCharArray()).ToArray())
                .Create();
        }
    }
}
