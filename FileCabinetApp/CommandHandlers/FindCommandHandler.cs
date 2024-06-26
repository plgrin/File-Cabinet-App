#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

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
            var parameters = request.Parameters.Split(' ', 2);

            if (parameters.Length < 2)
            {
                Console.WriteLine("Invalid parameters. Usage: find <property> <value>");
                return;
            }

            var property = parameters[0].ToLower();
            var value = parameters[1];

            IEnumerable<FileCabinetRecord> records = property switch
            {
                "firstname" => this.service.FindByFirstName(value),
                "lastname" => this.service.FindByLastName(value),
                "dateofbirth" => this.service.FindByDateOfBirth(value),
                _ => null
            };

            if (records == null)
            {
                Console.WriteLine($"Search by {property} is not supported.");
            }
            else
            {
                this.printer(records);
            }
        }
    }
}
