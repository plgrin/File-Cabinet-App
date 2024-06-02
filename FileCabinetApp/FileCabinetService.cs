using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Provides methods to manage file cabinet records.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

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
            ValidateParameters(firstName, lastName, dateOfBirth, age, salary, gender);

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

            ValidateParameters(firstName, lastName, dateOfBirth, age, salary, gender);

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
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return this.firstNameDictionary[firstName].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Finds records by last name.
        /// </summary>
        /// <param name="lastName">Last name to search for.</param>
        /// <returns>An array of records with the specified last name.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return this.lastNameDictionary[lastName].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Finds records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search for.</param>
        /// <returns>An array of records with the specified date of birth.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            DateTime dob = DateTime.Parse(dateOfBirth);
            if (this.dateOfBirthDictionary.ContainsKey(dob))
            {
                return this.dateOfBirthDictionary[dob].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>An array of all records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// Gets the total number of records.
        /// </summary>
        /// <returns>The total number of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        protected abstract void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender);

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
    }
}
