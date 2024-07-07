#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to manage file cabinet records.</param>
        public FindCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
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
            if (request.Command.Equals("find", StringComparison.InvariantCultureIgnoreCase))
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
            var paramParts = parameters.Split(' ', 2);
            if (paramParts.Length != 2)
            {
                Console.WriteLine("Invalid find command format.");
                return;
            }

            var fieldName = paramParts[0].Trim().ToLower();
            var value = paramParts[1].Trim().Trim('\'');
            ReadOnlyCollection<FileCabinetRecord> records;

            switch (fieldName)
            {
                case "firstname":
                    records = this.service.FindByFirstName(value);
                    break;
                case "lastname":
                    records = this.service.FindByLastName(value);
                    break;
                case "dateofbirth":
                    records = this.service.FindByDateOfBirth(value);
                    break;
                default:
                    Console.WriteLine($"Search by {fieldName} is not supported.");
                    return;
            }

            if (records.Any())
            {
                this.printer(records);
            }
            else
            {
                Console.WriteLine("No records found.");
            }
        }
    }
}
