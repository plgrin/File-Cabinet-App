using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Models;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// Writes FileCabinetRecord objects to a CSV file.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">The text writer to write to.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Writes the CSV header to the file.
        /// </summary>
        public void WriteHeader()
        {
            this.writer.WriteLine("Id,First Name,Last Name,Date of Birth,Age,Salary,Gender");
        }

        /// <summary>
        /// Writes a record to the CSV file.
        /// </summary>
        /// <param name="record">The FileCabinetRecord to write.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine($"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth:MM/dd/yyyy},{record.Age},{record.Salary},{record.Gender}");
        }
    }
}
