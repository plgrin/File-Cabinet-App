using System;
using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp
{
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        public DeleteCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Delete(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Delete(string parameters)
        {
            var criteria = parameters.Trim().Split('=', 2);
            if (criteria.Length != 2)
            {
                Console.WriteLine("Invalid delete command format. Use: delete where <field>=<value>");
                return;
            }

            var field = criteria[0].Trim().ToLower();
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
