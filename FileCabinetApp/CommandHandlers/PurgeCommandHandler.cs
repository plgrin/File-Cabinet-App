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
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The file cabinet service to use for purging records.</param>
        public PurgeCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

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
            if (this.service is FileCabinetFilesystemService)
            {
                int purgedCount = this.service.Purge();
                Console.WriteLine($"Data file processing is completed: {purgedCount} of {this.service.GetStat()} records were purged.");
            }
            else
            {
                Console.WriteLine("Purge command is only applicable for file storage.");
            }
        }
    }
}
