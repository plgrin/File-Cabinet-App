#pragma warning disable CA1822

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handles commands for the File Cabinet application.
    /// </summary>
    public class CommandHandler : CommandHandlerBase
    {
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
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service to use.</param>
        public CommandHandler(IFileCabinetService fileCabinetService)
        {
            Program.fileCabinetService = fileCabinetService;
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            switch (request.Command.ToLower())
            {
                case "help":
                    this.PrintHelp(request.Parameters);
                    break;
                case "exit":
                    this.Exit(request.Parameters);
                    break;
                case "stat":
                    this.Stat(request.Parameters);
                    break;
                case "create":
                    this.Create(request.Parameters);
                    break;
                case "list":
                    this.List(request.Parameters);
                    break;
                case "edit":
                    this.Edit(request.Parameters);
                    break;
                case "find":
                    this.Find(request.Parameters);
                    break;
                case "export":
                    this.Export(request.Parameters);
                    break;
                case "import":
                    this.Import(request.Parameters);
                    break;
                case "remove":
                    this.Remove(request.Parameters);
                    break;
                case "purge":
                    this.Purge(request.Parameters);
                    break;
                default:
                    this.PrintMissedCommandInfo(request.Command);
                    break;
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

        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
        }

        private void Stat(string parameters)
        {
            var (totalCount, deletedCount) = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{totalCount} record(s).");
            Console.WriteLine($"{deletedCount} record(s) are deleted.");
        }

        private void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = ReadInput(Converters.StringConverter, Validators.FirstNameValidator);

            Console.Write("Last name: ");
            var lastName = ReadInput(Converters.StringConverter, Validators.LastNameValidator);

            Console.Write("Date of birth (mm/dd/yyyy): ");
            var dateOfBirth = ReadInput(Converters.DateConverter, Validators.DateOfBirthValidator);

            Console.Write("Age: ");
            var age = ReadInput(Converters.ShortConverter, Validators.AgeValidator);

            Console.Write("Salary: ");
            var salary = ReadInput(Converters.DecimalConverter, Validators.SalaryValidator);

            Console.Write("Gender (M/F): ");
            var gender = ReadInput(Converters.CharConverter, Validators.GenderValidator);

            int recordId = Program.fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            Console.WriteLine($"Record #{recordId} is created.");
        }

        private T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private void List(string parameters)
        {
            var records = Program.fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
            }
        }

        private void Edit(string parameters)
        {
            if (int.TryParse(parameters, out int id))
            {
                try
                {
                    Console.Write("First name: ");
                    var firstName = ReadInput(Converters.StringConverter, Validators.FirstNameValidator);

                    Console.Write("Last name: ");
                    var lastName = ReadInput(Converters.StringConverter, Validators.LastNameValidator);

                    Console.Write("Date of birth (mm/dd/yyyy): ");
                    var dateOfBirth = ReadInput(Converters.DateConverter, Validators.DateOfBirthValidator);

                    Console.Write("Age: ");
                    var age = ReadInput(Converters.ShortConverter, Validators.AgeValidator);

                    Console.Write("Salary: ");
                    var salary = ReadInput(Converters.DecimalConverter, Validators.SalaryValidator);

                    Console.Write("Gender (M/F): ");
                    var gender = ReadInput(Converters.CharConverter, Validators.GenderValidator);

                    Program.fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, age, salary, gender);
                    Console.WriteLine($"Record #{id} is updated.");
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

        private void Find(string parameters)
        {
            var inputs = parameters.Split(' ', 2);
            if (inputs.Length < 2)
            {
                Console.WriteLine("Invalid parameters. Usage: find <property> <value>");
                return;
            }

            var property = inputs[0];
            var value = inputs[1].Trim('\"');

            if (property.Equals("firstname", StringComparison.OrdinalIgnoreCase))
            {
                var records = Program.fileCabinetService.FindByFirstName(value);
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            }
            else if (property.Equals("lastname", StringComparison.OrdinalIgnoreCase))
            {
                var records = Program.fileCabinetService.FindByLastName(value);
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            }
            else if (property.Equals("dateofbirth", StringComparison.OrdinalIgnoreCase))
            {
                var records = Program.fileCabinetService.FindByDateOfBirth(value);
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            }
            else
            {
                Console.WriteLine($"Search by {property} is not supported.");
            }
        }

        private void Export(string parameters)
        {
            var inputs = parameters.Split(' ', 2);
            if (inputs.Length < 2)
            {
                Console.WriteLine("Invalid parameters. Usage: export <format> <filename>");
                return;
            }

            var format = inputs[0];
            var path = inputs[1];

            if (format.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                ExportCsv(path);
            }
            else if (format.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                ExportXml(path);
            }
            else
            {
                Console.WriteLine($"Export in {format} format is not supported.");
            }
        }

        private void ExportCsv(string path)
        {
            if (File.Exists(path))
            {
                Console.Write($"File is exist - rewrite {path}? [Y/n] ");
                var answer = Console.ReadLine();
                if (answer.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
            }

            try
            {
                using var writer = new StreamWriter(path);
                var snapshot = Program.fileCabinetService.MakeSnapshot();
                snapshot.SaveToCsv(writer);
                Console.WriteLine($"All records are exported to file {path}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }

        private void ExportXml(string path)
        {
            if (File.Exists(path))
            {
                Console.Write($"File is exist - rewrite {path}? [Y/n] ");
                var answer = Console.ReadLine();
                if (answer.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
            }

            try
            {
                using var writer = new StreamWriter(path);
                var snapshot = Program.fileCabinetService.MakeSnapshot();
                snapshot.SaveToXml(writer);
                Console.WriteLine($"All records are exported to file {path}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }

        private void Import(string parameters)
        {
            var inputs = parameters.Split(' ', 2);
            if (inputs.Length < 2)
            {
                Console.WriteLine("Invalid parameters. Usage: import <format> <filename>");
                return;
            }

            var format = inputs[0];
            var path = inputs[1];

            if (format.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                ImportCsv(path);
            }
            else if (format.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                ImportXml(path);
            }
            else
            {
                Console.WriteLine($"Import in {format} format is not supported.");
            }
        }

        private void ImportCsv(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Import error: file {path} does not exist.");
                return;
            }

            try
            {
                using var reader = new StreamReader(path);
                var csvReader = new FileCabinetRecordCsvReader(reader);
                var records = csvReader.ReadAll();

                int importedCount = 0;
                foreach (var record in records)
                {
                    try
                    {
                        Program.fileCabinetService.CreateRecord(record.FirstName, record.LastName, record.DateOfBirth, record.Age, record.Salary, record.Gender);
                        importedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error importing record with ID {record.Id}: {ex.Message}");
                    }
                }

                Console.WriteLine($"{importedCount} records were imported from {path}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import failed: {ex.Message}");
            }
        }

        private void ImportXml(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Import error: file {path} does not exist.");
                return;
            }

            try
            {
                using var reader = new StreamReader(path);
                var xmlReader = new FileCabinetRecordXmlReader();
                var records = xmlReader.ReadAll(reader);

                int importedCount = 0;
                foreach (var record in records)
                {
                    try
                    {
                        Program.fileCabinetService.CreateRecord(
                            record.FirstName,
                            record.LastName,
                            record.DateOfBirth,
                            record.Age,
                            record.Salary,
                            record.Gender
                        );
                        importedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error importing record with ID {record.Id}: {ex.Message}");
                    }
                }

                Console.WriteLine($"{importedCount} records were imported from {path}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import failed: {ex.Message}");
            }
        }

        private void Remove(string parameters)
        {
            if (int.TryParse(parameters, out int id))
            {
                try
                {
                    Program.fileCabinetService.RemoveRecord(id);
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

        private void Purge(string parameters)
        {
            if (Program.fileCabinetService is FileCabinetFilesystemService)
            {
                int purgedCount = Program.fileCabinetService.Purge();
                Console.WriteLine($"Data file processing is completed: {purgedCount} of {Program.fileCabinetService.GetStat()} records were purged.");
            }
            else
            {
                Console.WriteLine("Purge command is only applicable for file storage.");
            }
        }

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}
