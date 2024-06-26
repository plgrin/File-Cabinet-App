﻿using System.ComponentModel.DataAnnotations;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Validators;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    /// <summary>
    /// Main class for the File Cabinet Application.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Grin Polina";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private static IFileCabinetService? fileCabinetService;

        private static bool isRunning = true;

        /// <summary>
        /// Main method for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            string validationRulesType = "default";
            string storageType = "memory";
            foreach (string arg in args)
            {
                if (arg.StartsWith("--validation-rules=", StringComparison.InvariantCultureIgnoreCase))
                {
                    validationRulesType = arg.Substring("--validation-rules=".Length);
                }
                else if (arg.StartsWith("-v", StringComparison.InvariantCultureIgnoreCase))
                {
                    validationRulesType = arg.Substring(2);
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

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("validation-rules.json", optional: false, reloadOnChange: true)
                .Build();

            IConfigurationSection validationRules = configuration.GetSection(validationRulesType);

            IRecordValidator validator;
            var builder = new ValidatorBuilder();
            if (validationRulesType.Equals("custom", StringComparison.OrdinalIgnoreCase))
            {
                validator = builder.CreateCustom(validationRules);
                Console.WriteLine("Using custom validation rules.");
            }
            else
            {
                validator = builder.CreateDefault(validationRules);
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
            var exitHandler = new ExitCommandHandler(SetIsRunning);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, DefaultRecordPrint);
            var editHandler = new EditCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, DefaultRecordPrint);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);

            helpHandler.SetNext(exitHandler)
                       .SetNext(statHandler)
                       .SetNext(createHandler)
                       .SetNext(findHandler)
                       .SetNext(editHandler)
                       .SetNext(listHandler)
                       .SetNext(exportHandler)
                       .SetNext(importHandler)
                       .SetNext(removeHandler)
                       .SetNext(purgeHandler);

            return helpHandler;
        }

        private static void SetIsRunning(bool running)
        {
            isRunning = running;
        }

        public static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
            }
        }
    }
}
