using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class DefaultValidator : IRecordValidator
    {
        private readonly DefaultFirstNameValidator firstNameValidator = new DefaultFirstNameValidator();
        private readonly DefaultLastNameValidator lastNameValidator = new DefaultLastNameValidator();
        private readonly DefaultDateOfBirthValidator dateOfBirthValidator = new DefaultDateOfBirthValidator();
        private readonly DefaultAgeValidator ageValidator = new DefaultAgeValidator();
        private readonly DefaultSalaryValidator salaryValidator = new DefaultSalaryValidator();
        private readonly DefaultGenderValidator genderValidator = new DefaultGenderValidator();

        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            firstNameValidator.Validate(firstName);
            lastNameValidator.Validate(lastName);
            dateOfBirthValidator.Validate(dateOfBirth);
            ageValidator.Validate(age);
            salaryValidator.Validate(salary);
            genderValidator.Validate(gender);
        }
    }
}
