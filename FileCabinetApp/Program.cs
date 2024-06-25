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
        private static IFileCabinetService? fileCabinetService;

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] сommands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
        };

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
        };

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

                var index = Array.FindIndex(сommands, 0, сommands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int ParametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[ParametersIndex] : string.Empty;
                    сommands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
        }

        private static void Edit(string parameters)
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

                    fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, age, salary, gender);
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

        private static void Create(string parameters)
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

            int recordId = fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            Console.WriteLine($"Record #{recordId} is created.");
        }

        private static void Find(string parameters)
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
                var records = fileCabinetService.FindByFirstName(value);
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            }
            else if (property.Equals("lastname", StringComparison.OrdinalIgnoreCase))
            {
                var records = fileCabinetService.FindByLastName(value);
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            }
            else if (property.Equals("dateofbirth", StringComparison.OrdinalIgnoreCase))
            {
                var records = fileCabinetService.FindByDateOfBirth(value);
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

        private static void PrintHelp(string parameters)
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

        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Age}, {record.Salary}, {record.Gender}");
            }
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

        private static void Export(string parameters)
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

        private static void ExportCsv(string path)
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
                var snapshot = fileCabinetService.MakeSnapshot();
                snapshot.SaveToCsv(writer);
                Console.WriteLine($"All records are exported to file {path}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }

        private static void ExportXml(string path)
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
                var snapshot = fileCabinetService.MakeSnapshot();
                snapshot.SaveToXml(writer);
                Console.WriteLine($"All records are exported to file {path}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }

        private static void Import(string parameters)
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
            else
            {
                Console.WriteLine($"Import in {format} format is not supported.");
            }
        }

        private static void ImportCsv(string path)
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
                        fileCabinetService.CreateRecord(record.FirstName, record.LastName, record.DateOfBirth, record.Age, record.Salary, record.Gender);
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

    }
}
