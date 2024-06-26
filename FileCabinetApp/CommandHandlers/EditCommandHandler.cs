﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "edit" command.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        private const string EditCommandText = "edit";

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to manage file cabinet records.</param>
        public EditCommandHandler(IFileCabinetService service)
           : base(service)
        {
        }

        /// <summary>
        /// Handles the "edit" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == EditCommandText)
            {
                Edit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Edit(string parameters)
        {
            if (int.TryParse(parameters, out int id))
            {
                try
                {
                    Console.Write("First name: ");
                    var firstName = this.ReadInput(Converters.StringConverter, Validator.FirstNameValidator);

                    Console.Write("Last name: ");
                    var lastName = this.ReadInput(Converters.StringConverter, Validator.LastNameValidator);

                    Console.Write("Date of birth (mm/dd/yyyy): ");
                    var dateOfBirth = this.ReadInput(Converters.DateConverter, Validator.DateOfBirthValidator);

                    Console.Write("Age: ");
                    var age = this.ReadInput(Converters.ShortConverter, Validator.AgeValidator);

                    Console.Write("Salary: ");
                    var salary = this.ReadInput(Converters.DecimalConverter, Validator.SalaryValidator);

                    Console.Write("Gender (M/F): ");
                    var gender = this.ReadInput(Converters.CharConverter, Validator.GenderValidator);

                    this.service.EditRecord(id, firstName, lastName, dateOfBirth, age, salary, gender);
                    Console.WriteLine($"Record #{id} is updated.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid record id.");
            }
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
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Validation failed: {ex.Message}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
