using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Models;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Interface for printing records.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints the specified records.
        /// </summary>
        /// <param name="records">The records to print.</param>
        void Print(IEnumerable<FileCabinetRecord> records);
    }
}
