using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Models;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// Reads FileCabinetRecord objects from a CSV file.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private readonly TextReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">The text reader to read from.</param>
        public FileCabinetRecordCsvReader(TextReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records from the CSV file.
        /// </summary>
        /// <returns>A list of FileCabinetRecord objects.</returns>
        public List<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();

            // Skip the header line
            _ = this.reader.ReadLine();
            string line;
            while ((line = this.reader.ReadLine()) != null)
            {
                var fields = line.Split(',');

                try
                {
                    var record = new FileCabinetRecord
                    {
                        Id = int.Parse(fields[0], CultureInfo.InvariantCulture),
                        FirstName = fields[1],
                        LastName = fields[2],
                        DateOfBirth = DateTime.Parse(fields[3], CultureInfo.InvariantCulture),
                        Age = short.Parse(fields[4], CultureInfo.InvariantCulture),
                        Salary = decimal.Parse(fields[5], CultureInfo.InvariantCulture),
                        Gender = char.Parse(fields[6]),
                    };
                    records.Add(record);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading record: {ex.Message}");
                }
            }

            return records;
        }
    }
}
