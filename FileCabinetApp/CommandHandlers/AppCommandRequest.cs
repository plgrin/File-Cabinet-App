using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents a request to execute a command in the File Cabinet application.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Gets or sets the command to execute.
        /// </summary>
        /// <value>
        /// The command to execute.
        /// </value>
        required public string Command { get; set; }

        /// <summary>
        /// Gets or sets the parameters for the command.
        /// </summary>
        /// <value>
        /// The parameters for the command.
        /// </value>
        required public string Parameters { get; set; }
    }
}
