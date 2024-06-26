using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class GenderValidator : IRecordValidator
    {
        private readonly char[] validGenders;

        public GenderValidator(char[] validGenders)
        {
            this.validGenders = validGenders;
        }

        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (!validGenders.Contains(gender))
            {
                throw new ArgumentException($"Gender must be one of the following: {validGenders}.");
            }
        }
    }
}
