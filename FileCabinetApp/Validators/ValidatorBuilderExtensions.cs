using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Provides extension methods for the ValidatorBuilder class to create default and custom validators.
    /// </summary>
    public static class ValidatorBuilderExtensions
    {
        /// <summary>
        /// Creates a default validator using configuration from the specified section.
        /// </summary>
        /// <param name="builder">The ValidatorBuilder instance.</param>
        /// <param name="section">The configuration section containing validation rules.</param>
        /// <returns>An IRecordValidator instance configured with default validation rules.</returns>
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

        /// <summary>
        /// Creates a custom validator using configuration from the specified section.
        /// </summary>
        /// <param name="builder">The ValidatorBuilder instance.</param>
        /// <param name="section">The configuration section containing validation rules.</param>
        /// <returns>An IRecordValidator instance configured with custom validation rules.</returns>
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
