﻿#pragma warning disable CA1031

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "import" command.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        private const string ImportCommandText = "import";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service to manage file cabinet records.</param>
        public ImportCommandHandler(IFileCabinetService service)
           : base(service)
        {
        }

        /// <summary>
        /// Handles the "import" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.Equals(ImportCommandText, StringComparison.OrdinalIgnoreCase))
            {
                this.Import(request.Parameters);
            }
            else
            {
                base.Handle(request);
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

            if (format.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                this.ImportCsv(path);
            }
            else if (format.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                this.ImportXml(path);
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
                var csvReader = new Helpers.FileCabinetRecordCsvReader(reader);
                var records = csvReader.ReadAll();

                int importedCount = 0;
                foreach (var record in records)
                {
                    try
                    {
                        this.service.CreateRecord(record.FirstName, record.LastName, record.DateOfBirth, record.Age, record.Salary, record.Gender);
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
                var xmlReader = new Helpers.FileCabinetRecordXmlReader();
                var records = xmlReader.ReadAll(reader);

                int importedCount = 0;
                foreach (var record in records)
                {
                    try
                    {
                        this.service.CreateRecord(
                            record.FirstName,
                            record.LastName,
                            record.DateOfBirth,
                            record.Age,
                            record.Salary,
                            record.Gender);

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
