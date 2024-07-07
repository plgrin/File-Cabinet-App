using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FileCabinetApp.Models;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// Reads FileCabinetRecord objects from an XML file.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private readonly XmlSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        public FileCabinetRecordXmlReader()
        {
            this.serializer = new XmlSerializer(typeof(FileCabinetRecord[]), new XmlRootAttribute("records"));
        }

        /// <summary>
        /// Reads all records from the provided StreamReader.
        /// </summary>
        /// <param name="reader">The StreamReader to read from.</param>
        /// <returns>An enumerable collection of FileCabinetRecord objects.</returns>
        public IEnumerable<FileCabinetRecord> ReadAll(StreamReader reader)
        {
            var records = (FileCabinetRecord[])this.serializer.Deserialize(reader);
            return records;
        }
    }
}
