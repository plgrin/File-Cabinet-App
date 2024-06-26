using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base class for command handlers.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        /// <summary>
        /// Sets the next handler in the chain.
        /// </summary>
        /// <param name="handler">The next handler.</param>
        /// <returns>The current handler for chaining.</returns>
        public ICommandHandler SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
            return handler;
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public virtual void Handle(AppCommandRequest request)
        {
            if (this.nextHandler != null)
            {
                this.nextHandler.Handle(request);
            }
            else
            {
                this.PrintMissedCommandInfo(request.Command);
            }
        }

        /// <summary>
        /// Prints a message indicating that the command was not recognized.
        /// </summary>
        /// <param name="command">The unrecognized command.</param>
        protected void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}
