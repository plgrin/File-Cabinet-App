using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "stat" command.
    /// </summary>
    public class StatCommandHandler : CommandHandlerBase
    {
        private const string StatCommandText = "stat";
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The file cabinet service to use for getting statistics.</param>
        public StatCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Handles the "stat" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == StatCommandText)
            {
                this.Stat(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Stat(string parameters)
        {
            var (totalCount, deletedCount) = this.service.GetStat();
            Console.WriteLine($"{totalCount} record(s).");
            Console.WriteLine($"{deletedCount} record(s) are deleted.");
        }
    }
}
