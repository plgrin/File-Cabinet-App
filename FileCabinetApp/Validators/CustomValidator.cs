using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class CustomValidator : IRecordValidator
    {
        private readonly CustomFirstNameValidator firstNameValidator = new CustomFirstNameValidator();
        private readonly CustomLastNameValidator lastNameValidator = new CustomLastNameValidator();
        private readonly CustomDateOfBirthValidator dateOfBirthValidator = new CustomDateOfBirthValidator();
        private readonly CustomAgeValidator ageValidator = new CustomAgeValidator();
        private readonly CustomSalaryValidator salaryValidator = new CustomSalaryValidator();
        private readonly CustomGenderValidator genderValidator = new CustomGenderValidator();

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
