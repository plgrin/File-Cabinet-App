using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Models;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handles the select command.
    /// </summary>
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to perform select operations on.</param>
        public SelectCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the select command request.
        /// </summary>
        /// <param name="request">The command request containing the command and parameters.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("select", StringComparison.OrdinalIgnoreCase))
            {
                this.Select(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Applies the specified criteria to filter the records.
        /// </summary>
        /// <param name="records">The records to filter.</param>
        /// <param name="criteriaPart">The criteria to apply.</param>
        /// <returns>The filtered records.</returns>
        private static ReadOnlyCollection<FileCabinetRecord> ApplyCriteria(ReadOnlyCollection<FileCabinetRecord> records, string criteriaPart)
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
                        "firstname" => record.FirstName.Equals(value, StringComparison.OrdinalIgnoreCase),
                        "lastname" => record.LastName.Equals(value, StringComparison.OrdinalIgnoreCase),
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

        /// <summary>
        /// Prints the records in a tabular format.
        /// </summary>
        /// <param name="records">The records to print.</param>
        /// <param name="fields">The fields to include in the output.</param>
        private static void PrintRecords(ReadOnlyCollection<FileCabinetRecord> records, string[] fields)
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

        /// <summary>
        /// Selects records based on the specified parameters and prints them.
        /// </summary>
        /// <param name="parameters">The parameters for the select command.</param>
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
    }
}
