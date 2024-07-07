#pragma warning disable CA1062
#pragma warning disable CA1854
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FileCabinetApp.Models;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Provides methods to manage file cabinet records.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly IRecordValidator validator;

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private readonly Dictionary<string, ReadOnlyCollection<FileCabinetRecord>> searchCache = new Dictionary<string, ReadOnlyCollection<FileCabinetRecord>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">The validator to validate record parameters.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Creates a new record.
        /// </summary>
        /// <param name="firstName"> First name.</param>
        /// <param name="lastName"> Last name.</param>
        /// <param name="dateOfBirth"> Date of birth.</param>
        /// <param name="age"> Age.</param>
        /// <param name="salary">Salary.</param>
        /// <param name="gender">Gender.</param>
        /// <returns>The ID of the created record.</returns>
        /// <exception cref="ArgumentException">Thrown when one of the parameters is invalid.</exception>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            this.validator.ValidateParameters(firstName, lastName, dateOfBirth, age, salary, gender);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Age = age,
                Salary = salary,
                Gender = gender,
            };

            this.list.Add(record);
            this.ClearCache();

            this.AddRecordToDictionaries(record);
            return record.Id;
        }

        /// <summary>
        /// Edits an existing record.
        /// </summary>
        /// <param name="id">Record ID.</param>
        /// <param name="firstName"> First name.</param>
        /// <param name="lastName"> Last name.</param>
        /// <param name="dateOfBirth"> Date of birth.</param>
        /// <param name="age"> Age.</param>
        /// <param name="salary">Salary.</param>
        /// <param name="gender">Gender.</param>
        /// <exception cref="ArgumentException">Thrown when one of the parameters is invalid or the record does not exist.</exception>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            var record = this.list.FirstOrDefault(r => r.Id == id);

            if (record == null)
            {
                throw new ArgumentException($"Record with id {id} does not exist.");
            }

            this.validator.ValidateParameters(firstName, lastName, dateOfBirth, age, salary, gender);

            this.RemoveRecordFromDictionaries(record);

            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.Age = age;
            record.Salary = salary;
            record.Gender = gender;
            this.ClearCache();

            this.AddRecordToDictionaries(record);
        }

        /// <summary>
        /// Finds records by first name.
        /// </summary>
        /// <param name="firstName">First name to search for.</param>
        /// <returns>An array of records with the specified first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            string key = $"firstName:{firstName.ToLower(System.Globalization.CultureInfo.CurrentCulture)}";
            if (this.searchCache.ContainsKey(key))
            {
                return this.searchCache[key];
            }

            var result = this.list.FindAll(r => r.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase));
            var readOnlyResult = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.searchCache[key] = readOnlyResult;
            return readOnlyResult;
        }

        /// <summary>
        /// Finds records by last name.
        /// </summary>
        /// <param name="lastName">Last name to search for.</param>
        /// <returns>An array of records with the specified last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            string key = $"lastName:{lastName.ToLower()}";
            if (this.searchCache.ContainsKey(key))
            {
                return this.searchCache[key];
            }

            var result = this.list.FindAll(r => r.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));
            var readOnlyResult = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.searchCache[key] = readOnlyResult;
            return readOnlyResult;
        }

        /// <summary>
        /// Finds records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search for.</param>
        /// <returns>An array of records with the specified date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            string key = $"dateOfBirth:{dateOfBirth.ToLower()}";
            if (this.searchCache.ContainsKey(key))
            {
                return this.searchCache[key];
            }

            var result = this.list.FindAll(r => r.DateOfBirth.ToString("yyyy-MM-dd").Equals(dateOfBirth, StringComparison.OrdinalIgnoreCase));
            var readOnlyResult = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.searchCache[key] = readOnlyResult;
            return readOnlyResult;
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>An array of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list.AsReadOnly();
        }

        /// <summary>
        /// Gets the total number of records.
        /// </summary>
        /// <returns>The total number of records.</returns>
        public (int Total, int Deleted) GetStat()
        {
            return (this.list.Count, 0); // В памяти записи удаляются полностью.
        }

        /// <summary>
        /// Creates a snapshot of the current state of the records in memory.
        /// </summary>
        /// <returns>A snapshot of the current state of the records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>(this.list));
        }

        /// <summary>
        /// Purges the data by removing deleted records. In memory service, purge is not applicable.
        /// </summary>
        /// <returns>The number of purged records, always 0 for memory service.</returns>
        public int Purge()
        {
            // No action needed for memory service
            return 0;
        }

        /// <summary>
        /// Removes the record with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the record to remove.</param>
        /// <exception cref="ArgumentException">Thrown when the record with the specified ID doesn't exist.</exception>
        public void RemoveRecord(int id)
        {
            var record = this.list.FirstOrDefault(r => r.Id == id);
            if (record != null)
            {
                this.list.Remove(record);
                this.ClearCache();
                this.firstNameDictionary[record.FirstName].Remove(record);
                this.lastNameDictionary[record.LastName].Remove(record);
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
            }
            else
            {
                throw new ArgumentException($"Record #{id} doesn't exist.");
            }
        }

        private void RemoveRecordFromDictionaries(FileCabinetRecord record)
        {
            this.firstNameDictionary[record.FirstName].Remove(record);
            if (this.firstNameDictionary[record.FirstName].Count == 0)
            {
                this.firstNameDictionary.Remove(record.FirstName);
            }

            this.lastNameDictionary[record.LastName].Remove(record);
            if (this.lastNameDictionary[record.LastName].Count == 0)
            {
                this.lastNameDictionary.Remove(record.LastName);
            }

            this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
            if (this.dateOfBirthDictionary[record.DateOfBirth].Count == 0)
            {
                this.dateOfBirthDictionary.Remove(record.DateOfBirth);
            }
        }

        private void AddRecordToDictionaries(FileCabinetRecord record)
        {
            if (!this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary[record.FirstName] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[record.FirstName].Add(record);

            if (!this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary[record.LastName] = new List<FileCabinetRecord>();
            }

            this.lastNameDictionary[record.LastName].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth] = new List<FileCabinetRecord>();
            }

            this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
        }

        private void ClearCache()
        {
            this.searchCache.Clear();
        }
    }
}
