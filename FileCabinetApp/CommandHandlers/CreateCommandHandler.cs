﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Helpers;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "create" command.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to manage file cabinet records.</param>
        public CreateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the "create" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("create", StringComparison.OrdinalIgnoreCase))
            {
                this.Create(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = this.ReadInput(Converters.StringConverter, Validators.Validator.FirstNameValidator);

            Console.Write("Last name: ");
            var lastName = this.ReadInput(Converters.StringConverter, Validators.Validator.LastNameValidator);

            Console.Write("Date of birth (mm/dd/yyyy): ");
            var dateOfBirth = this.ReadInput(Converters.DateConverter, Validators.Validator.DateOfBirthValidator);

            Console.Write("Age: ");
            var age = this.ReadInput(Converters.ShortConverter, Validators.Validator.AgeValidator);

            Console.Write("Salary: ");
            var salary = this.ReadInput(Converters.DecimalConverter, Validators.Validator.SalaryValidator);

            Console.Write("Gender (M/F): ");
            var gender = this.ReadInput(Converters.CharConverter, Validators.Validator.GenderValidator);

            int recordId = this.service.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            Console.WriteLine($"Record #{recordId} is created.");
        }

        private T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Action<T> validator)
         {
            do
            {
                T value;
                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                try
                {
                    validator(value);
                    return value;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Validation failed: {ex.Message}. Please, correct your input.");
                }
            }
            while (true);
        }
    }
}
