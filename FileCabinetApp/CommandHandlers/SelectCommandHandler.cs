using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileCabinetApp;
using FileCabinetApp.CommandHandlers;

public class SelectCommandHandler : ServiceCommandHandlerBase
{
    public SelectCommandHandler(IFileCabinetService service)
        : base(service)
    {
    }

    public override void Handle(AppCommandRequest commandRequest)
    {
        if (commandRequest.Command.Equals("select", StringComparison.InvariantCultureIgnoreCase))
        {
            this.Select(commandRequest.Parameters);
        }
        else
        {
            base.Handle(commandRequest);
        }
    }

    private void Select(string parameters)
    {
        var parts = parameters.Split(new[] { " where " }, StringSplitOptions.None);
        var fieldsPart = parts[0].Trim();
        var criteriaPart = parts.Length > 1 ? parts[1].Trim() : string.Empty;

        var fields = fieldsPart.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(f => f.Trim())
                               .ToArray();

        var records = this.service.GetRecords();

        if (!string.IsNullOrEmpty(criteriaPart))
        {
            records = ApplyCriteria(records, criteriaPart);
        }

        PrintRecords(records, fields);
    }

    private ReadOnlyCollection<FileCabinetRecord> ApplyCriteria(ReadOnlyCollection<FileCabinetRecord> records, string criteriaPart)
    {
        var criteria = criteriaPart.Split(new[] { " and ", " or " }, StringSplitOptions.None);
        var isAndOperator = criteriaPart.Contains(" and ");

        var filteredRecords = records.Where(record =>
        {
            var isMatch = criteria.Select(criterion =>
            {
                var parts = criterion.Split(new[] { '=' }, 2);
                var field = parts[0].Trim();
                var value = parts[1].Trim(' ', '\'');

                return field switch
                {
                    "id" => record.Id == int.Parse(value),
                    "firstname" => record.FirstName.Equals(value, StringComparison.InvariantCultureIgnoreCase),
                    "lastname" => record.LastName.Equals(value, StringComparison.InvariantCultureIgnoreCase),
                    "dateofbirth" => record.DateOfBirth == DateTime.Parse(value),
                    "age" => record.Age == short.Parse(value),
                    "salary" => record.Salary == decimal.Parse(value),
                    "gender" => record.Gender == char.Parse(value),
                    _ => false
                };
            });

            return isAndOperator ? isMatch.All(match => match) : isMatch.Any(match => match);
        }).ToList();

        return new ReadOnlyCollection<FileCabinetRecord>(filteredRecords);
    }

    private void PrintRecords(ReadOnlyCollection<FileCabinetRecord> records, string[] fields)
    {
        var table = new List<string[]>();

        // Add headers
        table.Add(fields);

        // Add records
        foreach (var record in records)
        {
            var row = fields.Select(field => field switch
            {
                "id" => record.Id.ToString(),
                "firstname" => record.FirstName,
                "lastname" => record.LastName,
                "dateofbirth" => record.DateOfBirth.ToShortDateString(),
                "age" => record.Age.ToString(),
                "salary" => record.Salary.ToString("F2"),
                "gender" => record.Gender.ToString(),
                _ => string.Empty
            }).ToArray();

            table.Add(row);
        }

        // Determine column widths
        var columnWidths = new int[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            columnWidths[i] = table.Max(row => row[i].Length);
        }

        // Print table
        foreach (var row in table)
        {
            for (int i = 0; i < row.Length; i++)
            {
                var cell = row[i];
                var padding = columnWidths[i] - cell.Length;
                var paddedCell = cell.PadRight(columnWidths[i]);
                Console.Write($"| {paddedCell} ");
            }
            Console.WriteLine("|");

            if (row == table.First())
            {
                // Print separator after headers
                foreach (var width in columnWidths)
                {
                    Console.Write($"+-{new string('-', width)}-");
                }
                Console.WriteLine("+");
            }
        }
    }
}
