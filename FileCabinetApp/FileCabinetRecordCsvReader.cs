using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvReader
    {
        private readonly TextReader reader;

        public FileCabinetRecordCsvReader(TextReader reader)
        {
            this.reader = reader;
        }

        public List<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();

            // Skip the header line
            string line = this.reader.ReadLine();
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
                        Gender = char.Parse(fields[6])
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
