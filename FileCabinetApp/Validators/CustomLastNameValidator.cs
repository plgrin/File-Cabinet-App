using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class CustomLastNameValidator
    {
        public void Validate(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 3 || lastName.Length > 50)
            {
                throw new ArgumentException("Last name must be between 3 and 50 characters long and cannot be empty or whitespace.");
            }
        }
    }
}
