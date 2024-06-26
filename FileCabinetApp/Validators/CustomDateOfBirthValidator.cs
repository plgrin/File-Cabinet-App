using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class CustomDateOfBirthValidator
    {
        public void Validate(DateTime dateOfBirth)
        {
            if (dateOfBirth < new DateTime(1960, 1, 1) || dateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new ArgumentException("Date of birth must be between 01-Jan-1960 and 18 years ago.");
            }
        }
    }
}
