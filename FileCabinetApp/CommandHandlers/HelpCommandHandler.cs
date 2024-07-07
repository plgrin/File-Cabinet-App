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
            new string[] { "list", "lists all records", "The 'list' command lists all records." },
            new string[] { "edit", "edits an existing record", "The 'edit' command edits an existing record." },
            new string[] { "find", "finds records by a property", "The 'find' command finds records by a property." },
            new string[] { "export", "exports records to a file", "The 'export' command exports records to a file. Usage: export <format> <filename>." },
            new string[] { "import", "imports records from a file", "The 'import' command imports records from a file. Usage: import <format> <filename>." },
            new string[] { "remove", "removes a record by ID", "The 'remove' command removes a record by ID. Usage: remove <id>." },
            new string[] { "purge", "defragments the data file", "The 'purge' command defragments the data file by removing deleted records." },
            new string[] { "insert", "inserts a new record", "The 'insert' command inserts a new record. Usage: insert (field1, field2, ...) values ('value1', 'value2', ...)." },
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
