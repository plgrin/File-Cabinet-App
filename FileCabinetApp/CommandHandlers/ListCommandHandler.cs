using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "list" command.
    /// </summary>
    public class ListCommandHandler : CommandHandlerBase
    {
        private const string ListCommandText = "list";

        /// <summary>
        /// Handles the "list" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == ListCommandText)
            {
                this.List(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void List(string parameters)
        {
            var records = Program.fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
            }
        }
    }
}
