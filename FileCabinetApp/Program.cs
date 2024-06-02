namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Grin Polina";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the record statistics", "The 'stat' command prints the record statistics." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "lists all records", "The 'list' command lists all records." },
            new string[] { "edit", "edits an existing record", "The 'edit' command edits an existing record." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
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
                    } while (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60);

                    string lastName;
                    do
                    {
                        Console.Write("Last name: ");
                        lastName = Console.ReadLine();
                    } while (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60);

                    DateTime dateOfBirth;
                    do
                    {
                        Console.Write("Date of birth (mm/dd/yyyy): ");
                    } while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth) || dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now);

                    short age;
                    do
                    {
                        Console.Write("Age: ");
                    } while (!short.TryParse(Console.ReadLine(), out age) || age < 0 || age > 120);

                    decimal salary;
                    do
                    {
                        Console.Write("Salary: ");
                    } while (!decimal.TryParse(Console.ReadLine(), out salary) || salary < 0);

                    char gender;
                    do
                    {
                        Console.Write("Gender (M/F): ");
                    } while (!char.TryParse(Console.ReadLine(), out gender) || !"MF".Contains(gender));

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

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            string firstName;
            do
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60);

            string lastName;
            do
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60);

            DateTime dateOfBirth;
            do
            {
                Console.Write("Date of birth (mm/dd/yyyy): ");
            } while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth) || dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now);

            short age;
            do
            {
                Console.Write("Age: ");
            } while (!short.TryParse(Console.ReadLine(), out age) || age < 0 || age > 120);

            decimal salary;
            do
            {
                Console.Write("Salary: ");
            } while (!decimal.TryParse(Console.ReadLine(), out salary) || salary < 0);

            char gender;
            do
            {
                Console.Write("Gender (M/F): ");
            } while (!char.TryParse(Console.ReadLine(), out gender) || !"MF".Contains(gender));

            int recordId = fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            Console.WriteLine($"Record #{recordId} is created.");
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

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}
