using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "purge" command.
    /// </summary>
    public class PurgeCommandHandler : CommandHandlerBase
    {
        private const string PurgeCommandText = "purge";

        /// <summary>
        /// Handles the "purge" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(System.Globalization.CultureInfo.CurrentCulture) == PurgeCommandText)
            {
                this.Purge(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Purge(string parameters)
        {
            if (Program.fileCabinetService is FileCabinetFilesystemService)
            {
                int purgedCount = Program.fileCabinetService.Purge();
                Console.WriteLine($"Data file processing is completed: {purgedCount} of {Program.fileCabinetService.GetStat()} records were purged.");
            }
            else
            {
                Console.WriteLine("Purge command is only applicable for file storage.");
            }
        }
    }
}
