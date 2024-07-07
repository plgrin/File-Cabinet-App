using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp.Models
{
    /// <summary>
    /// Represents a snapshot of the current state of the file cabinet service.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly List<FileCabinetRecord> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="list">The list of file cabinet records.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Gets the records in the snapshot as a read-only collection.
        /// </summary>
        public ReadOnlyCollection<FileCabinetRecord> Records => this.list.AsReadOnly();

        /// <summary>
        /// Saves the snapshot to a CSV file.
        /// </summary>
        /// <param name="writer">The stream writer to write the CSV data to.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            var csvWriter = new Helpers.FileCabinetRecordCsvWriter(writer);
            csvWriter.WriteHeader();
            foreach (var record in this.list)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>
        /// Saves the snapshot to an XML file.
        /// </summary>
        /// <param name="writer">The stream writer to write the XML data to.</param>
        public void SaveToXml(StreamWriter writer)
        {
            using var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });
            var recordWriter = new Helpers.FileCabinetRecordXmlWriter(xmlWriter);

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
