using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;

        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

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
