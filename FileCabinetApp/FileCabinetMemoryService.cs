using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
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

            this.AddRecordToDictionaries(record);
            return record.Id;
        }

        public void CreateRecord(FileCabinetRecord record)
        {
            this.validator.ValidateParameters(record.FirstName, record.LastName, record.DateOfBirth, record.Age, record.Salary, record.Gender);

            var existingRecord = this.list.FirstOrDefault(r => r.Id == record.Id);
            if (existingRecord != null)
            {
                this.list.Remove(existingRecord);
            }

            this.list.Add(record);
            this.AddRecordToDictionaries(record);
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

            this.AddRecordToDictionaries(record);
        }

        /// <summary>
        /// Finds records by first name.
        /// </summary>
        /// <param name="firstName">First name to search for.</param>
        /// <returns>An array of records with the specified first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Finds records by last name.
        /// </summary>
        /// <param name="lastName">Last name to search for.</param>
        /// <returns>An array of records with the specified last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Finds records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search for.</param>
        /// <returns>An array of records with the specified date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            DateTime dob = DateTime.Parse(dateOfBirth);
            if (this.dateOfBirthDictionary.ContainsKey(dob))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dob]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
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
        public int GetStat()
        {
            return this.list.Count;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>(this.list));
        }

        //protected abstract void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender);

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

        public void RemoveRecord(int id)
        {
            var record = this.list.FirstOrDefault(r => r.Id == id);
            if (record != null)
            {
                this.list.Remove(record);
                this.firstNameDictionary[record.FirstName].Remove(record);
                this.lastNameDictionary[record.LastName].Remove(record);
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
            }
            else
            {
                throw new ArgumentException($"Record #{id} doesn't exist.");
            }
        }

    }
}
