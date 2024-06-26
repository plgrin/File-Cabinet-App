using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class CustomFirstNameValidator
    {
        public void Validate(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 3 || firstName.Length > 50)
            {
                throw new ArgumentException("First name must be between 3 and 50 characters long and cannot be empty or whitespace.");
            }
        }
    }
}
