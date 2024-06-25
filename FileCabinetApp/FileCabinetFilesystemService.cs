using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Provides file system based storage for File Cabinet records.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int RecordSize = 278;
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream to use for storing records.</param>
        /// <param name="validator">The validator to validate record parameters.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

        /// <summary>
        /// Creates a new record in the file system.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="age">The age.</param>
        /// <param name="salary">The salary.</param>
        /// <param name="gender">The gender.</param>
        /// <returns>The ID of the created record.</returns>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            var record = new FileCabinetRecord
            {
                Id = (int)(this.fileStream.Length / RecordSize) + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Age = age,
                Salary = salary,
                Gender = gender,
            };

            this.validator.ValidateParameters(firstName, lastName, dateOfBirth, age, salary, gender);

            using (BinaryWriter writer = new BinaryWriter(this.fileStream, System.Text.Encoding.UTF8, true))
            {
                writer.Seek(0, SeekOrigin.End);
                writer.Write((short)0); // Status
                writer.Write(record.Id);
                writer.Write(PadRight(record.FirstName, 60));
                writer.Write(PadRight(record.LastName, 60));
                writer.Write(record.DateOfBirth.Year);
                writer.Write(record.DateOfBirth.Month);
                writer.Write(record.DateOfBirth.Day);
                writer.Write(record.Age);
                writer.Write(record.Salary);
                writer.Write(record.Gender);
            }

            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all records from the file system.
        /// </summary>
        /// <returns>A read-only collection of file cabinet records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var records = new List<FileCabinetRecord>();

            this.fileStream.Seek(0, SeekOrigin.Begin);

            using (BinaryReader reader = new BinaryReader(this.fileStream, Encoding.UTF8, true))
            {
                while (this.fileStream.Position < this.fileStream.Length)
                {
                    var status = reader.ReadInt16();
                    var id = reader.ReadInt32();
                    var firstName = new string(reader.ReadChars(60)).Trim();
                    var lastName = new string(reader.ReadChars(60)).Trim();
                    var year = reader.ReadInt32();
                    var month = reader.ReadInt32();
                    var day = reader.ReadInt32();
                    var age = reader.ReadInt16();
                    var salary = reader.ReadDecimal();
                    var gender = reader.ReadChar();

                    var record = new FileCabinetRecord
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = new DateTime(year, month, day),
                        Age = age,
                        Salary = salary,
                        Gender = gender,
                    };

                    records.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        private static char[] PadRight(string value, int length)
        {
            return value.PadRight(length).ToCharArray();
        }
    }
}
