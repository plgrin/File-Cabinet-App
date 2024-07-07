using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "help" command.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const string HelpCommandText = "help";

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the record statistics", "The 'stat' command prints the record statistics." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "select", "selects records by criteria", "The 'select' command selects records by criteria. Usage: select (field1, field2, ...) where (criteria)." },
            new string[] { "export", "exports records to a file", "The 'export' command exports records to a file. Usage: export <format> <filename>." },
            new string[] { "import", "imports records from a file", "The 'import' command imports records from a file. Usage: import <format> <filename>." },
            new string[] { "purge", "defragments the data file", "The 'purge' command defragments the data file by removing deleted records." },
            new string[] { "insert", "inserts a new record", "The 'insert' command inserts a new record. Usage: insert (field1, field2, ...) values ('value1', 'value2', ...)." },
            new string[] { "delete", "deletes records by specified criteria", "The 'delete' command deletes records by specified criteria. Usage: delete where <field> = '<value>'." },
            new string[] { "update", "updates records by specified criteria", "The 'update' command updates records by specified criteria. Usage: update set <field1> = '<value1>', <field2> = '<value2>' where <field3> = '<value3>'." },
        };

        /// <summary>
        /// Handles the "help" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == HelpCommandText)
            {
                this.PrintHelp(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[0], parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][2]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[0], helpMessage[1]);
                }
            }

            Console.WriteLine();
        }
    }
}
