using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "remove" command.
    /// </summary>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        private const string RemoveCommandText = "remove";

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to manage file cabinet records.</param>
        public RemoveCommandHandler(IFileCabinetService service)
           : base(service)
        {
        }

        /// <summary>
        /// Handles the "remove" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == RemoveCommandText)
            {
                this.Remove(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Remove(string parameters)
        {
            if (int.TryParse(parameters, out int id))
            {
                try
                {
                    this.service.RemoveRecord(id);
                    Console.WriteLine($"Record #{id} is removed.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid record id.");
            }
        }
    }
}
