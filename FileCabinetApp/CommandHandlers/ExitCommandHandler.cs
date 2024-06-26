using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "exit" command.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private const string ExitCommandText = "exit";

        /// <summary>
        /// Handles the "exit" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == ExitCommandText)
            {
                Exit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
        }
    }
}
