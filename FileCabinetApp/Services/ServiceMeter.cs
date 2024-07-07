using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// The ServiceMeter class is a decorator that measures the execution time of methods in the IFileCabinetService.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">The file cabinet service to decorate.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Measures the execution time of the CreateRecord method.
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
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.CreateRecord(firstName, lastName, dateOfBirth, age, salary, gender);
            stopwatch.Stop();
            Console.WriteLine($"Create method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Measures the execution time of the EditRecord method.
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
            var stopwatch = Stopwatch.StartNew();
            this.service.EditRecord(id, firstName, lastName, dateOfBirth, age, salary, gender);
            stopwatch.Stop();
            Console.WriteLine($"Edit method execution duration is {stopwatch.ElapsedTicks} ticks.");
        }

        /// <summary>
        /// Measures the execution time of the GetRecords method.
        /// </summary>
        /// <returns>A read-only collection of file cabinet records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.GetRecords();
            stopwatch.Stop();
            Console.WriteLine($"GetRecords method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Measures the execution time of the GetStat method.
        /// </summary>
        /// <returns>A tuple containing the total number of records and the number of deleted records.</returns>
        public (int Total, int Deleted) GetStat()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.GetStat();
            stopwatch.Stop();
            Console.WriteLine($"GetStat method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Measures the execution time of the FindByFirstName method.
        /// </summary>
        /// <param name="firstName">The first name to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.FindByFirstName(firstName);
            stopwatch.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Measures the execution time of the FindByLastName method.
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.FindByLastName(lastName);
            stopwatch.Stop();
            Console.WriteLine($"FindByLastName method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Measures the execution time of the FindByDateOfBirth method.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            stopwatch.Stop();
            Console.WriteLine($"FindByDateOfBirth method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Measures the execution time of the MakeSnapshot method.
        /// </summary>
        /// <returns>A snapshot of the current state of the records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = this.service.MakeSnapshot();
            stopwatch.Stop();
            Console.WriteLine($"MakeSnapshot method execution duration is {stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Measures the execution time of the RemoveRecord method.
        /// </summary>
        /// <param name="id">The ID of the record to remove.</param>
        public void RemoveRecord(int id)
        {
            var stopwatch = Stopwatch.StartNew();
            this.service.RemoveRecord(id);
            stopwatch.Stop();
            Console.WriteLine($"RemoveRecord method execution duration is {stopwatch.ElapsedTicks} ticks.");
        }

        /// <summary>
        /// Measures the execution time of the Purge method.
        /// </summary>
        /// <returns>The number of purged records.</returns>
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
