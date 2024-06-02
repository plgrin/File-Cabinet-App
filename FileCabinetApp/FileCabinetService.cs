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
                Gender = gender,
            };

            this.list.Add(record);

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
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.list
                .Where(record => record.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            return this.list
                .Where(record => record.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            DateTime dob = DateTime.Parse(dateOfBirth);
            return this.list
                .Where(record => record.DateOfBirth == dob)
                .ToArray();
        }
    }
}
