using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public void SaveToXml(StreamWriter writer)
        {
            using var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });
            var recordWriter = new FileCabinetRecordXmlWriter(xmlWriter);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("records");

            foreach (var record in this.list)
            {
                recordWriter.Write(record);
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
        }
    }

}
