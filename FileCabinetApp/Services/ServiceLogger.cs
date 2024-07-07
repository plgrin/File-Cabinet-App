using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Logs the operations performed on a file cabinet service.
    /// </summary>
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService service;
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">The file cabinet service to log operations for.</param>
        /// <param name="writer">The text writer to log to.</param>
        public ServiceLogger(IFileCabinetService service, TextWriter writer)
        {
            this.service = service;
            this.writer = writer;
        }

        /// <summary>
        /// Logs the creation of a record.
        /// </summary>
        /// <param name="firstName">The first name of the record.</param>
        /// <param name="lastName">The last name of the record.</param>
        /// <param name="dateOfBirth">The date of birth of the record.</param>
        /// <param name="age">The age of the record.</param>
        /// <param name="salary">The salary of the record.</param>
        /// <param name="gender">The gender of the record.</param>
        /// <returns>The ID of the created record.</returns>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling CreateRecord() with FirstName = '{firstName}', LastName = '{lastName}', DateOfBirth = '{dateOfBirth}', Age = '{age}', Salary = '{salary}', Gender = '{gender}'");
            int recordId = this.service.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            this.writer.WriteLine($"{DateTime.Now} - CreateRecord() returned '{recordId}'");
            return recordId;
        }

        /// <summary>
        /// Logs the editing of a record.
        /// </summary>
        /// <param name="id">The ID of the record to edit.</param>
        /// <param name="firstName">The first name of the record.</param>
        /// <param name="lastName">The last name of the record.</param>
        /// <param name="dateOfBirth">The date of birth of the record.</param>
        /// <param name="age">The age of the record.</param>
        /// <param name="salary">The salary of the record.</param>
        /// <param name="gender">The gender of the record.</param>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling EditRecord() with Id = '{id}', FirstName = '{firstName}', LastName = '{lastName}', DateOfBirth = '{dateOfBirth}', Age = '{age}', Salary = '{salary}', Gender = '{gender}'");
            this.service.EditRecord(id, firstName, lastName, dateOfBirth, age, salary, gender);
            this.writer.WriteLine($"{DateTime.Now} - EditRecord() completed.");
        }

        /// <summary>
        /// Logs the retrieval of all records.
        /// </summary>
        /// <returns>A read-only collection of file cabinet records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling GetRecords()");
            var records = this.service.GetRecords();
            this.writer.WriteLine($"{DateTime.Now} - GetRecords() returned '{records.Count}' records");
            return records;
        }

        /// <summary>
        /// Logs the retrieval of record statistics.
        /// </summary>
        /// <returns>A tuple containing the total and deleted records count.</returns>
        public (int Total, int Deleted) GetStat()
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling GetStat()");
            var stat = this.service.GetStat();
            this.writer.WriteLine($"{DateTime.Now} - GetStat() returned Total = '{stat.Total}', Deleted = '{stat.Deleted}'");
            return stat;
        }

        /// <summary>
        /// Logs the search for records by first name.
        /// </summary>
        /// <param name="firstName">The first name to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByFirstName() with FirstName = '{firstName}'");
            var records = this.service.FindByFirstName(firstName);
            this.writer.WriteLine($"{DateTime.Now} - FindByFirstName() returned '{records.Count}' records");
            return records;
        }

        /// <summary>
        /// Logs the search for records by last name.
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByLastName() with LastName = '{lastName}'");
            var records = this.service.FindByLastName(lastName);
            this.writer.WriteLine($"{DateTime.Now} - FindByLastName() returned '{records.Count}' records");
            return records;
        }

        /// <summary>
        /// Logs the search for records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByDateOfBirth() with DateOfBirth = '{dateOfBirth}'");
            var records = this.service.FindByDateOfBirth(dateOfBirth);
            this.writer.WriteLine($"{DateTime.Now} - FindByDateOfBirth() returned '{records.Count}' records");
            return records;
        }

        /// <summary>
        /// Logs the creation of a snapshot of the current state of the records.
        /// </summary>
        /// <returns>A snapshot of the current state of the records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling MakeSnapshot()");
            var snapshot = this.service.MakeSnapshot();
            this.writer.WriteLine($"{DateTime.Now} - MakeSnapshot() completed");
            return snapshot;
        }

        /// <summary>
        /// Logs the removal of a record.
        /// </summary>
        /// <param name="id">The ID of the record to remove.</param>
        public void RemoveRecord(int id)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling RemoveRecord() with Id = '{id}'");
            this.service.RemoveRecord(id);
            this.writer.WriteLine($"{DateTime.Now} - RemoveRecord() completed.");
        }

        /// <summary>
        /// Logs the purging of deleted records from the file cabinet.
        /// </summary>
        /// <returns>The number of purged records.</returns>
        public int Purge()
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling Purge()");
            int purged = this.service.Purge();
            this.writer.WriteLine($"{DateTime.Now} - Purge() removed '{purged}' records");
            return purged;
        }
    }
}
