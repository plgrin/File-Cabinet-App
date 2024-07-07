using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FileCabinetApp.Models;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// Writes FileCabinetRecord objects to an XML file.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">The XmlWriter to use for writing.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Writes a FileCabinetRecord to the XML file.
        /// </summary>
        /// <param name="record">The record to write.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteStartElement("record");
            this.writer.WriteAttributeString("id", record.Id.ToString());

            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("first", record.FirstName);
            this.writer.WriteAttributeString("last", record.LastName);
            this.writer.WriteEndElement();

            this.writer.WriteElementString("dateOfBirth", record.DateOfBirth.ToString("MM/dd/yyyy"));

            this.writer.WriteElementString("age", record.Age.ToString());
            this.writer.WriteElementString("salary", record.Salary.ToString());
            this.writer.WriteElementString("gender", record.Gender.ToString());

            this.writer.WriteEndElement();
        }
    }
}
