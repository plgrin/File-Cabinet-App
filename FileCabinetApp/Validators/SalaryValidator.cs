using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal min;
        private readonly decimal max;

        public SalaryValidator(decimal min, decimal max)
        {
            this.min = min;
            this.max = max;
        }

        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (salary < min || salary > max)
            {
                throw new ArgumentException($"Salary must be between {min} and {max}.");
            }
        }
    }
}
