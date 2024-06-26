using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "list" command.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private const string ListCommandText = "list";
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to manage file cabinet records.</param>
        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
           : base(service)
        {
            this.printer = printer;
        }

        /// <summary>
        /// Handles the "list" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var records = this.service.GetRecords();
            this.printer(records);
        }
    }
}
