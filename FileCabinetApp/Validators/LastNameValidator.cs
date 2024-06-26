﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class LastNameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < minLength || lastName.Length > maxLength)
            {
                throw new ArgumentException($"Last name must be between {minLength} and {maxLength} characters long and cannot be empty or whitespace.");
            }
        }
    }
}
