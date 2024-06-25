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
        };

        /// <summary>
        /// Main method for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            string validationRules = "default";
            foreach (string arg in args)
            {
                if (arg.StartsWith("--validation-rules=", StringComparison.InvariantCultureIgnoreCase))
                {
                    validationRules = arg.Substring("--validation-rules=".Length);
                }
                else if (arg.StartsWith("-v ", StringComparison.InvariantCultureIgnoreCase))
                {
                    validationRules = arg.Substring("-v ".Length);
                }
            }

            if (validationRules.Equals("custom", StringComparison.OrdinalIgnoreCase))
            {
                fileCabinetService = new FileCabinetService(new CustomValidator());
                Console.WriteLine("Using custom validation rules.");
            }
            else
            {
                fileCabinetService = new FileCabinetService(new DefaultValidator());
                Console.WriteLine("Using default validation rules.");
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
                    string firstName;
                    do
                    {
                        Console.Write("First name: ");
                        firstName = Console.ReadLine();
                    }
                    while (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60);

                    string lastName;
                    do
                    {
                        Console.Write("Last name: ");
                        lastName = Console.ReadLine();
                    }
                    while (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60);

                    DateTime dateOfBirth;
                    do
                    {
                        Console.Write("Date of birth (dd/mm/yyyy): ");
                    }
                    while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth) || dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now);

                    short age;
                    do
                    {
                        Console.Write("Age: ");
                    }
                    while (!short.TryParse(Console.ReadLine(), out age) || age < 0 || age > 120);

                    decimal salary;
                    do
                    {
                        Console.Write("Salary: ");
                    }
                    while (!decimal.TryParse(Console.ReadLine(), out salary) || salary < 0);

                    char gender;
                    do
                    {
                        Console.Write("Gender (M/F): ");
                    }
                    while (!char.TryParse(Console.ReadLine(), out gender) || !"MF".Contains(gender));

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
            string firstName;
            do
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60);

            string lastName;
            do
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60);

            DateTime dateOfBirth;
            do
            {
                Console.Write("Date of birth (dd/mm/yyyy): ");
            }
            while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth) || dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now);

            short age;
            do
            {
                Console.Write("Age: ");
            }
            while (!short.TryParse(Console.ReadLine(), out age) || age < 0 || age > 120);

            decimal salary;
            do
            {
                Console.Write("Salary: ");
            }
            while (!decimal.TryParse(Console.ReadLine(), out salary) || salary < 0);

            char gender;
            do
            {
                Console.Write("Gender (M/F): ");
            }
            while (!char.TryParse(Console.ReadLine(), out gender) || !"MF".Contains(gender));

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
    }
}
