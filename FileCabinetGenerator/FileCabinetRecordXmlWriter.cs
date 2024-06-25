using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    public static class FileCabinetRecordXmlWriter
    {
        public static void SaveToXml(List<FileCabinetRecord> records, string output)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<FileCabinetRecord>));

            using (var writer = new StreamWriter(output))
            {
                xmlSerializer.Serialize(writer, records);
            }
        }
    }
}
