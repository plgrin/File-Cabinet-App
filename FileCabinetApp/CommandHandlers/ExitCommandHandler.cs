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
        private readonly Action<bool> setIsRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="setIsRunning">Delegate to set the running state.</param>
        public ExitCommandHandler(Action<bool> setIsRunning)
        {
            this.setIsRunning = setIsRunning;
        }

        /// <summary>
        /// Handles the "exit" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                this.setIsRunning(false);
                Console.WriteLine("Exiting an application...");
            }
            else
            {
                base.Handle(request);
            }
        }
    }
}
