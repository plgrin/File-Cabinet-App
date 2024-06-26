using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "remove" command.
    /// </summary>
    public class RemoveCommandHandler : CommandHandlerBase
    {
        private const string RemoveCommandText = "remove";

        /// <summary>
        /// Handles the "remove" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == RemoveCommandText)
            {
                this.Remove(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Remove(string parameters)
        {
            if (int.TryParse(parameters, out int id))
            {
                try
                {
                    Program.fileCabinetService.RemoveRecord(id);
                    Console.WriteLine($"Record #{id} is removed.");
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
    }
}
