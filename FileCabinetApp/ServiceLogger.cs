using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService service;
        private readonly TextWriter writer;

        public ServiceLogger(IFileCabinetService service, TextWriter writer)
        {
            this.service = service;
            this.writer = writer;
        }

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling CreateRecord() with FirstName = '{firstName}', LastName = '{lastName}', DateOfBirth = '{dateOfBirth}', Age = '{age}', Salary = '{salary}', Gender = '{gender}'");
            int recordId = this.service.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            this.writer.WriteLine($"{DateTime.Now} - CreateRecord() returned '{recordId}'");
            return recordId;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling EditRecord() with Id = '{id}', FirstName = '{firstName}', LastName = '{lastName}', DateOfBirth = '{dateOfBirth}', Age = '{age}', Salary = '{salary}', Gender = '{gender}'");
            this.service.EditRecord(id, firstName, lastName, dateOfBirth, age, salary, gender);
            this.writer.WriteLine($"{DateTime.Now} - EditRecord() completed.");
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling GetRecords()");
            var records = this.service.GetRecords();
            this.writer.WriteLine($"{DateTime.Now} - GetRecords() returned '{records.Count}' records");
            return records;
        }

        public (int Total, int Deleted) GetStat()
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling GetStat()");
            var stat = this.service.GetStat();
            this.writer.WriteLine($"{DateTime.Now} - GetStat() returned Total = '{stat.Total}', Deleted = '{stat.Deleted}'");
            return stat;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByFirstName() with FirstName = '{firstName}'");
            var records = this.service.FindByFirstName(firstName);
            this.writer.WriteLine($"{DateTime.Now} - FindByFirstName() returned '{records.Count}' records");
            return records;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByLastName() with LastName = '{lastName}'");
            var records = this.service.FindByLastName(lastName);
            this.writer.WriteLine($"{DateTime.Now} - FindByLastName() returned '{records.Count}' records");
            return records;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByDateOfBirth() with DateOfBirth = '{dateOfBirth}'");
            var records = this.service.FindByDateOfBirth(dateOfBirth);
            this.writer.WriteLine($"{DateTime.Now} - FindByDateOfBirth() returned '{records.Count}' records");
            return records;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling MakeSnapshot()");
            var snapshot = this.service.MakeSnapshot();
            this.writer.WriteLine($"{DateTime.Now} - MakeSnapshot() completed");
            return snapshot;
        }

        public void RemoveRecord(int id)
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling RemoveRecord() with Id = '{id}'");
            this.service.RemoveRecord(id);
            this.writer.WriteLine($"{DateTime.Now} - RemoveRecord() completed.");
        }

        public int Purge()
        {
            this.writer.WriteLine($"{DateTime.Now} - Calling Purge()");
            int purged = this.service.Purge();
            this.writer.WriteLine($"{DateTime.Now} - Purge() removed '{purged}' records");
            return purged;
        }
    }
}
