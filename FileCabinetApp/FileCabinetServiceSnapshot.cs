using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly List<FileCabinetRecord> list;

        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.list = list;
        }

        public ReadOnlyCollection<FileCabinetRecord> Records => this.list.AsReadOnly();

        public void SaveToCsv(StreamWriter writer)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(writer);
            csvWriter.WriteHeader();
            foreach (var record in this.list)
            {
                csvWriter.Write(record);
            }
        }
    }

}
