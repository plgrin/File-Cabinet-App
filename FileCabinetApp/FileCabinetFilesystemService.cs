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
        private FileStream fileStream;
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

        public void CreateRecord(FileCabinetRecord record)
        {
            this.validator.ValidateParameters(record.FirstName, record.LastName, record.DateOfBirth, record.Age, record.Salary, record.Gender);

            var existingRecord = this.GetRecords().FirstOrDefault(r => r.Id == record.Id);
            if (existingRecord != null)
            {
                this.RemoveRecordFromFile(existingRecord.Id);
            }

            this.SaveRecord(record);
        }

        private void SaveRecord(FileCabinetRecord record)
        {
            using (BinaryWriter writer = new BinaryWriter(this.fileStream, System.Text.Encoding.UTF8, true))
            {
                writer.Seek(0, SeekOrigin.End);
                writer.Write((short)0);
                writer.Write(record.Id);
                writer.Write(record.FirstName.PadRight(60));
                writer.Write(record.LastName.PadRight(60));
                writer.Write(record.DateOfBirth.Year);
                writer.Write(record.DateOfBirth.Month);
                writer.Write(record.DateOfBirth.Day);
                writer.Write(record.Age);
                writer.Write(record.Salary);
                writer.Write(record.Gender);
            }
        }

        private void RemoveRecordFromFile(int id)
        {
            using (BinaryReader reader = new BinaryReader(this.fileStream, System.Text.Encoding.UTF8, true))
            using (BinaryWriter writer = new BinaryWriter(this.fileStream, System.Text.Encoding.UTF8, true))
            {
                this.fileStream.Seek(0, SeekOrigin.Begin);
                while (this.fileStream.Position < this.fileStream.Length)
                {
                    long position = this.fileStream.Position;
                    short status = reader.ReadInt16();
                    int recordId = reader.ReadInt32();

                    if (recordId == id)
                    {
                        writer.Seek((int)position, SeekOrigin.Begin);
                        writer.Write((short)1);
                        break;
                    }

                    this.fileStream.Seek(272, SeekOrigin.Current);
                }
            }
        }

        /// <summary>
        /// Edits an existing record in the file system.
        /// </summary>
        /// <param name="id">The ID of the record to edit.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="age">The age.</param>
        /// <param name="salary">The salary.</param>
        /// <param name="gender">The gender.</param>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender)
        {
            this.validator.ValidateParameters(firstName, lastName, dateOfBirth, age, salary, gender);

            using (BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.UTF8, true))
            using (BinaryReader reader = new BinaryReader(this.fileStream, Encoding.UTF8, true))
            {
                this.fileStream.Seek(0, SeekOrigin.Begin);

                while (this.fileStream.Position < this.fileStream.Length)
                {
                    var status = reader.ReadInt16();
                    var currentId = reader.ReadInt32();

                    if (currentId == id)
                    {
                        this.fileStream.Seek(-6, SeekOrigin.Current); // Move back to the start of the status field
                        writer.Write((short)0); // Status
                        writer.Write(id);
                        writer.Write(PadRight(firstName, 60));
                        writer.Write(PadRight(lastName, 60));
                        writer.Write(dateOfBirth.Year);
                        writer.Write(dateOfBirth.Month);
                        writer.Write(dateOfBirth.Day);
                        writer.Write(age);
                        writer.Write(salary);
                        writer.Write(gender);
                        return;
                    }

                    this.fileStream.Seek(RecordSize - 6, SeekOrigin.Current); // Move to the next record
                }

                throw new ArgumentException($"Record with ID {id} not found.");
            }
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

        /// <summary>
        /// Gets the total number of records in the file system.
        /// </summary>
        /// <returns>The number of records.</returns>
        public (int Total, int Deleted) GetStat()
        {
            int totalCount = 0;
            int deletedCount = 0;
            this.fileStream.Seek(0, SeekOrigin.Begin);

            using (BinaryReader reader = new BinaryReader(this.fileStream, Encoding.UTF8, true))
            {
                while (this.fileStream.Position < this.fileStream.Length)
                {
                    short status = reader.ReadInt16();
                    if ((status & 0b0100) == 0)
                    {
                        totalCount++;
                    }
                    else
                    {
                        deletedCount++;
                    }

                    this.fileStream.Seek(RecordSize - sizeof(short), SeekOrigin.Current);
                }
            }

            return (totalCount, deletedCount);
        }

        /// <summary>
        /// Finds records by first name.
        /// </summary>
        /// <param name="firstName">The first name to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var records = new List<FileCabinetRecord>();

            this.fileStream.Seek(0, SeekOrigin.Begin);

            using (BinaryReader reader = new BinaryReader(this.fileStream, Encoding.UTF8, true))
            {
                while (this.fileStream.Position < this.fileStream.Length)
                {
                    var status = reader.ReadInt16();
                    var id = reader.ReadInt32();
                    var recordFirstName = new string(reader.ReadChars(60)).Trim();
                    var lastName = new string(reader.ReadChars(60)).Trim();
                    var year = reader.ReadInt32();
                    var month = reader.ReadInt32();
                    var day = reader.ReadInt32();
                    var age = reader.ReadInt16();
                    var salary = reader.ReadDecimal();
                    var gender = reader.ReadChar();

                    if (string.Equals(recordFirstName, firstName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var record = new FileCabinetRecord
                        {
                            Id = id,
                            FirstName = recordFirstName,
                            LastName = lastName,
                            DateOfBirth = new DateTime(year, month, day),
                            Age = age,
                            Salary = salary,
                            Gender = gender,
                        };

                        records.Add(record);
                    }
                    else
                    {
                        this.fileStream.Seek(RecordSize - 142, SeekOrigin.Current); // Skip the rest of the record
                    }
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Finds records by last name.
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
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
                    var recordLastName = new string(reader.ReadChars(60)).Trim();
                    var year = reader.ReadInt32();
                    var month = reader.ReadInt32();
                    var day = reader.ReadInt32();
                    var age = reader.ReadInt16();
                    var salary = reader.ReadDecimal();
                    var gender = reader.ReadChar();

                    if (string.Equals(recordLastName, lastName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var record = new FileCabinetRecord
                        {
                            Id = id,
                            FirstName = firstName,
                            LastName = recordLastName,
                            DateOfBirth = new DateTime(year, month, day),
                            Age = age,
                            Salary = salary,
                            Gender = gender,
                        };

                        records.Add(record);
                    }
                    else
                    {
                        this.fileStream.Seek(RecordSize - 142, SeekOrigin.Current); // Skip the rest of the record
                    }
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Finds records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>A read-only collection of matching records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var records = new List<FileCabinetRecord>();

            if (!DateTime.TryParse(dateOfBirth, out var dob))
            {
                throw new ArgumentException("Invalid date format", nameof(dateOfBirth));
            }

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

                    var recordDateOfBirth = new DateTime(year, month, day);

                    if (recordDateOfBirth == dob)
                    {
                        var record = new FileCabinetRecord
                        {
                            Id = id,
                            FirstName = firstName,
                            LastName = lastName,
                            DateOfBirth = recordDateOfBirth,
                            Age = age,
                            Salary = salary,
                            Gender = gender,
                        };

                        records.Add(record);
                    }
                    else
                    {
                        this.fileStream.Seek(RecordSize - 142, SeekOrigin.Current); // Skip the rest of the record
                    }
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Creates a snapshot of the current state of the records in the file system.
        /// </summary>
        /// <returns>A snapshot of the current state of the records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var records = this.GetRecords();
            return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>(records));
        }

        /// <summary>
        /// Removes the record with the specified ID by marking it as deleted.
        /// </summary>
        /// <param name="id">The ID of the record to remove.</param>
        /// <exception cref="ArgumentException">Thrown when the record with the specified ID doesn't exist.</exception>
        public void RemoveRecord(int id)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            using (BinaryReader reader = new BinaryReader(this.fileStream, Encoding.UTF8, true))
            using (BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.UTF8, true))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    long position = reader.BaseStream.Position;
                    short status = reader.ReadInt16();
                    int recordId = reader.ReadInt32();

                    if (recordId == id)
                    {
                        writer.Seek((int)position, SeekOrigin.Begin);
                        writer.Write((short)(status | 0b_0100));
                        return;
                    }

                    reader.BaseStream.Seek(RecordSize - sizeof(short) - sizeof(int), SeekOrigin.Current);
                }
            }
            throw new ArgumentException($"Record #{id} doesn't exist.");
        }

        /// <summary>
        /// Defragments the data file by removing deleted records.
        /// </summary>
        /// <returns>The number of purged records.</returns>
        public int Purge()
        {
            var tempFilePath = Path.GetTempFileName();
            int purgedCount = 0;

            using (var tempFileStream = new FileStream(tempFilePath, FileMode.Create))
            using (var writer = new BinaryWriter(tempFileStream))
            {
                this.fileStream.Seek(0, SeekOrigin.Begin);

                while (this.fileStream.Position < this.fileStream.Length)
                {
                    var recordBytes = new byte[RecordSize];
                    this.fileStream.Read(recordBytes, 0, RecordSize);

                    var isDeleted = BitConverter.ToBoolean(recordBytes, 0);

                    if (!isDeleted)
                    {
                        writer.Write(recordBytes);
                    }
                    else
                    {
                        purgedCount++;
                    }
                }
            }

            this.fileStream.Close();
            File.Delete(this.fileStream.Name);
            File.Move(tempFilePath, this.fileStream.Name);
            this.fileStream = new FileStream(this.fileStream.Name, FileMode.Open);

            return purgedCount;
        }

        private static char[] PadRight(string value, int length)
        {
            return value.PadRight(length).ToCharArray();
        }
    }
}