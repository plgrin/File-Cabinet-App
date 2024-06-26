using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Defines a method to handle a command request.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets the next handler in the chain.
        /// </summary>
        /// <param name="handler">The next handler.</param>
        void SetNext(ICommandHandler handler);

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        void Handle(AppCommandRequest request);
    }
}
