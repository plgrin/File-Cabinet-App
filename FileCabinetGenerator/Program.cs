using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace FileCabinetGenerator
{
    static class Program
    {
        static void Main(string[] args)
        {
            var outputTypeOption = new Option<string>(
                new[] { "--output-type", "-t" },
                getDefaultValue: () => "csv",
                description: "Output format type (csv, xml)");

            var outputOption = new Option<string>(
                new[] { "--output", "-o" },
                description: "Output file name.");

            var recordsAmountOption = new Option<int>(
                new[] { "--records-amount", "-a" },
                getDefaultValue: () => 100,
                description: "Amount of generated records.");

            var startIdOption = new Option<int>(
                new[] { "--start-id", "-i" },
                getDefaultValue: () => 1,
                description: "ID value to start.");

            var rootCommand = new RootCommand
            {
                outputTypeOption,
                outputOption,
                recordsAmountOption,
                startIdOption
            };

            rootCommand.Description = "FileCabinetGenerator - generates records for File Cabinet";

            rootCommand.SetHandler((string outputType, string output, int recordsAmount, int startId) =>
            {
                Console.WriteLine($"Output Type: {outputType}");
                Console.WriteLine($"Output: {output}");
                Console.WriteLine($"Records Amount: {recordsAmount}");
                Console.WriteLine($"Start ID: {startId}");

                var records = RecordGenerator.GenerateRecords(startId, recordsAmount);
                foreach (var record in records)
                {
                    Console.WriteLine($"{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth}, {record.Age}, {record.Salary}, {record.Gender}");
                }
            },
            outputTypeOption, outputOption, recordsAmountOption, startIdOption);

            rootCommand.InvokeAsync(args).Wait();
        }
    }
}
