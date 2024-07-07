using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handles the insert command.
    /// </summary>
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to perform insert operations on.</param>
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the insert command request.
        /// </summary>
        /// <param name="request">The command request containing the command and parameters.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("insert", StringComparison.OrdinalIgnoreCase))
            {
                this.Insert(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Inserts a new record based on the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters for the insert command.</param>
        private void Insert(string parameters)
        {
            string[] paramParts = parameters.Split("values");
            if (paramParts.Length != 2)
            {
                Console.WriteLine("Invalid insert command format.");
                return;
            }

            string fieldsPart = paramParts[0].Trim().Trim('(', ')');
            string valuesPart = paramParts[1].Trim().Trim('(', ')');

            string[] fields = fieldsPart.Split(',');
            string[] values = valuesPart.Split(',');

            if (fields.Length != values.Length)
            {
                Console.WriteLine("The number of fields and values do not match.");
                return;
            }

            int id = 0;
            string firstName = string.Empty;
            string lastName = string.Empty;
            DateTime dateOfBirth = DateTime.MinValue;
            short age = 0;
            decimal salary = 0;
            char gender = ' ';  // Default value for gender

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Trim().ToLower(CultureInfo.CurrentCulture);
                values[i] = values[i].Trim().Trim('\'');

                switch (fields[i])
                {
                    case "id":
                        id = int.Parse(values[i]);
                        break;
                    case "firstname":
                        firstName = values[i];
                        break;
                    case "lastname":
                        lastName = values[i];
                        break;
                    case "dateofbirth":
                        if (!DateTime.TryParseExact(values[i], new[] { "M/d/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                        {
                            Console.WriteLine($"Invalid date format: {values[i]}");
                            return;
                        }

                        break;
                    case "age":
                        age = short.Parse(values[i]);
                        break;
                    case "salary":
                        salary = decimal.Parse(values[i]);
                        break;
                    case "gender":
                        gender = char.Parse(values[i]);
                        break;
                    default:
                        Console.WriteLine($"Unknown field: {fields[i]}");
                        return;
                }
            }

            try
            {
                this.service.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender == ' ' ? 'M' : gender);
                Console.WriteLine($"Record #{id} is created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert failed: {ex.Message}");
            }
        }
    }
}
