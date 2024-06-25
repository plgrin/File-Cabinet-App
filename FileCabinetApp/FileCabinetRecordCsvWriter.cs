using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void WriteHeader()
        {
            this.writer.WriteLine("Id,First Name,Last Name,Date of Birth,Age,Salary,Gender");
        }

        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine($"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth:MM/dd/yyyy},{record.Age},{record.Salary},{record.Gender}");
        }
    }
}
