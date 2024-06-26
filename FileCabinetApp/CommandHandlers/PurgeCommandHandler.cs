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
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        private const string PurgeCommandText = "purge";

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to manage file cabinet records.</param>
        public PurgeCommandHandler(IFileCabinetService service)
           : base(service)
        {
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
