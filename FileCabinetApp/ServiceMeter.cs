using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            stopwatch.Stop();
            Console.WriteLine($"Create method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            var stopwatch = Stopwatch.StartNew();
            this.service.EditRecord(id, firstName, lastName, dateOfBirth, age, salary, gender);
            stopwatch.Stop();
            Console.WriteLine($"Edit method execution duration is {stopwatch.ElapsedTicks} ticks.");
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.GetRecords();
            stopwatch.Stop();
            Console.WriteLine($"GetRecords method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public (int Total, int Deleted) GetStat()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.GetStat();
            stopwatch.Stop();
            Console.WriteLine($"GetStat method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.FindByFirstName(firstName);
            stopwatch.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.FindByLastName(lastName);
            stopwatch.Stop();
            Console.WriteLine($"FindByLastName method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            stopwatch.Stop();
            Console.WriteLine($"FindByDateOfBirth method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.MakeSnapshot();
            stopwatch.Stop();
            Console.WriteLine($"MakeSnapshot method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public void RemoveRecord(int id)
        {
            var stopwatch = Stopwatch.StartNew();
            this.service.RemoveRecord(id);
            stopwatch.Stop();
            Console.WriteLine($"RemoveRecord method execution duration is {stopwatch.ElapsedTicks} ticks.");
        }

        public int Purge()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.Purge();
            stopwatch.Stop();
            Console.WriteLine($"Purge method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }
    }
}
