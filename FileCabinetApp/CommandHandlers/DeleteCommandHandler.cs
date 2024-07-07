using System;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Services;

namespace FileCabinetApp
{
    /// <summary>
    /// Handles the delete command.
    /// </summary>
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to perform delete operations on.</param>
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the delete command request.
        /// </summary>
        /// <param name="request">The command request containing the command and parameters.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("delete", StringComparison.OrdinalIgnoreCase))
            {
                this.Delete(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Deletes records based on the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters for the delete command.</param>
        private void Delete(string parameters)
        {
            var criteria = parameters.Trim().Split('=', 2);
            if (criteria.Length != 2)
            {
                Console.WriteLine("Invalid delete command format. Use: delete where <field>=<value>");
                return;
            }

            var field = criteria[0].Trim().ToLower(System.Globalization.CultureInfo.CurrentCulture);
            var value = criteria[1].Trim().Trim('\'');

            try
            {
                int deletedCount = 0;

                if (field == "id")
                {
                    int id = int.Parse(value);
                    this.service.RemoveRecord(id);
                    deletedCount = 1;
                }
                else if (field == "firstname")
                {
                    var records = this.service.FindByFirstName(value);
                    deletedCount = records.Count;
                    foreach (var record in records)
                    {
                        this.service.RemoveRecord(record.Id);
                    }
                }
                else if (field == "lastname")
                {
                    var records = this.service.FindByLastName(value);
                    deletedCount = records.Count;
                    foreach (var record in records)
                    {
                        this.service.RemoveRecord(record.Id);
                    }
                }
                else if (field == "dateofbirth")
                {
                    var records = this.service.FindByDateOfBirth(value);
                    deletedCount = records.Count;
                    foreach (var record in records)
                    {
                        this.service.RemoveRecord(record.Id);
                    }
                }
                else
                {
                    Console.WriteLine($"Delete by {field} is not supported.");
                    return;
                }

                Console.WriteLine(deletedCount == 1 ? $"Record #{value} is deleted." : $"Records matching {field}='{value}' are deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting records: {ex.Message}");
            }
        }
    }
}
