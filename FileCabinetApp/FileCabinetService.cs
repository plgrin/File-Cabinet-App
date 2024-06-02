using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name must be between 2 and 60 characters long and cannot be empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Last name must be between 2 and 60 characters long and cannot be empty or whitespace.");
            }

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Date of birth must be between 01-Jan-1950 and the current date.");
            }

            if (age < 0 || age > 120)
            {
                throw new ArgumentException("Age must be a positive number less than or equal to 120.");
            }

            if (salary < 0)
            {
                throw new ArgumentException("Salary must be a positive number.");
            }

            if (!"MF".Contains(gender))
            {
                throw new ArgumentException("Gender must be 'M' or 'F'.");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Age = age,
                Salary = salary,
                Gender = gender
            };

            this.list.Add(record);

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary[firstName] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[firstName].Add(record);

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary[lastName] = new List<FileCabinetRecord>();
            }

            this.lastNameDictionary[lastName].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth] = new List<FileCabinetRecord>();
            }

            this.dateOfBirthDictionary[dateOfBirth].Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            var record = this.list.FirstOrDefault(r => r.Id == id);

            if (record == null)
            {
                throw new ArgumentException($"Record with id {id} does not exist.");
            }

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

            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name must be between 2 and 60 characters long and cannot be empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Last name must be between 2 and 60 characters long and cannot be empty or whitespace.");
            }

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Date of birth must be between 01-Jan-1950 and the current date.");
            }

            if (age < 0 || age > 120)
            {
                throw new ArgumentException("Age must be a positive number less than or equal to 120.");
            }

            if (salary < 0)
            {
                throw new ArgumentException("Salary must be a positive number.");
            }

            if (!"MF".Contains(gender))
            {
                throw new ArgumentException("Gender must be 'M' or 'F'.");
            }

            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.Age = age;
            record.Salary = salary;
            record.Gender = gender;

            // Добавление записи в словарь после изменения
            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary[firstName] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[firstName].Add(record);

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary[lastName] = new List<FileCabinetRecord>();
            }

            this.lastNameDictionary[lastName].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth] = new List<FileCabinetRecord>();
            }

            this.dateOfBirthDictionary[dateOfBirth].Add(record);
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return this.firstNameDictionary[firstName].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return this.lastNameDictionary[lastName].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            DateTime dob = DateTime.Parse(dateOfBirth);
            if (this.dateOfBirthDictionary.ContainsKey(dob))
            {
                return this.dateOfBirthDictionary[dob].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }
    }
}
