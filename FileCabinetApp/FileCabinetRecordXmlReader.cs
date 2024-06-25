using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlReader
    {
        private readonly XmlSerializer serializer;

        public FileCabinetRecordXmlReader()
        {
            this.serializer = new XmlSerializer(typeof(FileCabinetRecord[]), new XmlRootAttribute("records"));
        }

        public IEnumerable<FileCabinetRecord> ReadAll(StreamReader reader)
        {
            var records = (FileCabinetRecord[])this.serializer.Deserialize(reader);
            return records;
        }
    }
}
