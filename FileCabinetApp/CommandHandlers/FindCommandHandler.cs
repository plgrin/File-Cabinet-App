using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "find" command.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private const string FindCommandText = "find";
        private readonly IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to manage file cabinet records.</param>
        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
           : base(service)
        {
            this.printer = printer;
        }

        /// <summary>
        /// Handles the "find" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == FindCommandText)
            {
                this.Find(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Find(string parameters)
        {
            var inputs = parameters.Split(' ', 2);
            if (inputs.Length < 2)
            {
                Console.WriteLine("Invalid parameters. Usage: find <property> <value>");
                return;
            }

            var property = inputs[0];
            var value = inputs[1].Trim('\"');

            if (property.Equals("firstname", StringComparison.OrdinalIgnoreCase))
            {
                var records = this.service.FindByFirstName(value);
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            }
            else if (property.Equals("lastname", StringComparison.OrdinalIgnoreCase))
            {
                var records = this.service.FindByLastName(value);
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            }
            else if (property.Equals("dateofbirth", StringComparison.OrdinalIgnoreCase))
            {
                var records = this.service.FindByDateOfBirth(value);
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            }
            else
            {
                Console.WriteLine($"Search by {property} is not supported.");
            }
        }
    }
}
