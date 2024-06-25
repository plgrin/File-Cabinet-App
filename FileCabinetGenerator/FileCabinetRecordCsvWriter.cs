using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetGenerator
{
    public static class FileCabinetRecordCsvWriter
    {
        public static void SaveToCsv(List<FileCabinetRecord> records, string output)
        {
            using (var writer = new StreamWriter(output))
            {
                writer.WriteLine("Id,FirstName,LastName,DateOfBirth,Age,Salary,Gender");

                foreach (var record in records)
                {
                    writer.WriteLine($"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth:MM/dd/yyyy},{record.Age},{record.Salary},{record.Gender}");
                }
            }
        }
    }
}
