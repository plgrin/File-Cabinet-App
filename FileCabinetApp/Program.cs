﻿using FileCabinetApp.CommandHandlers;
using System.ComponentModel.DataAnnotations;

namespace FileCabinetApp
{
    /// <summary>
    /// Main class for the File Cabinet Application.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Grin Polina";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        public static IFileCabinetService? fileCabinetService;

        public static bool isRunning = true;

       

        /// <summary>
        /// Main method for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            string validationRules = "default";
            string storageType = "memory";
            foreach (string arg in args)
            {
                if (arg.StartsWith("--validation-rules=", StringComparison.InvariantCultureIgnoreCase))
                {
                    validationRules = arg.Substring("--validation-rules=".Length);
                }
                else if (arg.StartsWith("-v", StringComparison.InvariantCultureIgnoreCase))
                {
                    validationRules = arg.Substring(2);
                }
                else if (arg.StartsWith("--storage=", StringComparison.InvariantCultureIgnoreCase))
                {
                    storageType = arg.Substring("--storage=".Length);
                }
                else if (arg.StartsWith("-s", StringComparison.InvariantCultureIgnoreCase))
                {
                    storageType = arg.Substring(2);
                }
            }

            IRecordValidator validator;
            if (validationRules.Equals("custom", StringComparison.OrdinalIgnoreCase))
            {
                validator = new CustomValidator();
                Console.WriteLine("Using custom validation rules.");
            }
            else
            {
                validator = new DefaultValidator();
                Console.WriteLine("Using default validation rules.");
            }

            if (storageType.Equals("file", StringComparison.OrdinalIgnoreCase))
            {
                var fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate);
                fileCabinetService = new FileCabinetFilesystemService(fileStream, validator);
                Console.WriteLine("Using file storage.");
            }
            else
            {
                fileCabinetService = new FileCabinetMemoryService(validator);
                Console.WriteLine("Using memory storage.");
            }

            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            var commandHandler = CreateCommandHandlers();

            while (isRunning)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int CommandIndex = 0;
                var command = inputs[CommandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(HintMessage);
                    continue;
                }

                const int ParametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[ParametersIndex] : string.Empty;

                commandHandler.Handle(new AppCommandRequest
                {
                    Command = command,
                    Parameters = parameters
                });
            }
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var exitHandler = new ExitCommandHandler();
            var statHandler = new StatCommandHandler();
            var createHandler = new CreateCommandHandler();
            var listHandler = new ListCommandHandler();
            var editHandler = new EditCommandHandler();
            var findHandler = new FindCommandHandler();
            var exportHandler = new ExportCommandHandler();
            var importHandler = new ImportCommandHandler();
            var removeHandler = new RemoveCommandHandler();
            var purgeHandler = new PurgeCommandHandler();

            helpHandler.SetNext(exitHandler)
                       .SetNext(statHandler)
                       .SetNext(createHandler)
                       .SetNext(listHandler)
                       .SetNext(editHandler)
                       .SetNext(findHandler)
                       .SetNext(exportHandler)
                       .SetNext(importHandler)
                       .SetNext(removeHandler)
                       .SetNext(purgeHandler);

            return helpHandler;
        }
    }
}
