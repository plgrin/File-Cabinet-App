using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "create" command.
    /// </summary>
    public class CreateCommandHandler : CommandHandlerBase
    {
        private const string CreateCommandText = "create";

        /// <summary>
        /// Handles the "create" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == CreateCommandText)
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
            var firstName = this.ReadInput(Converters.StringConverter, Validators.FirstNameValidator);

            Console.Write("Last name: ");
            var lastName = this.ReadInput(Converters.StringConverter, Validators.LastNameValidator);

            Console.Write("Date of birth (mm/dd/yyyy): ");
            var dateOfBirth = this.ReadInput(Converters.DateConverter, Validators.DateOfBirthValidator);

            Console.Write("Age: ");
            var age = this.ReadInput(Converters.ShortConverter, Validators.AgeValidator);

            Console.Write("Salary: ");
            var salary = this.ReadInput(Converters.DecimalConverter, Validators.SalaryValidator);

            Console.Write("Gender (M/F): ");
            var gender = this.ReadInput(Converters.CharConverter, Validators.GenderValidator);

            int recordId = Program.fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            Console.WriteLine($"Record #{recordId} is created.");
        }

        private T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
