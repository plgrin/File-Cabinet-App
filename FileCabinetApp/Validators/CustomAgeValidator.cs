using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class CustomAgeValidator
    {
        public void Validate(short age)
        {
            if (age < 18 || age > 100)
            {
                throw new ArgumentException("Age must be between 18 and 100.");
            }
        }
    }
}
