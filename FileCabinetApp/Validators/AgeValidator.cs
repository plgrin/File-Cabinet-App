using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class AgeValidator : IRecordValidator
    {
        private readonly short min;
        private readonly short max;

        public AgeValidator(short min, short max)
        {
            this.min = min;
            this.max = max;
        }

        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (age < min || age > max)
            {
                throw new ArgumentException($"Age must be between {min} and {max}.");
            }
        }
    }
}
