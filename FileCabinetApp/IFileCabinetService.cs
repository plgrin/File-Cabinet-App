using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Provides methods for managing file cabinet records.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates a new record in the file cabinet.
        /// </summary>
        /// <param name="firstName">The first name of the record.</param>
        /// <param name="lastName">The last name of the record.</param>
        /// <param name="dateOfBirth">The date of birth of the record.</param>
        /// <param name="age">The age of the record.</param>
        /// <param name="salary">The salary of the record.</param>
        /// <param name="gender">The gender of the record.</param>
        /// <returns>The ID of the created record.</returns>
        int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender);

        /// <summary>
        /// Edits an existing record in the file cabinet.
        /// </summary>
        /// <param name="id">The ID of the record to edit.</param>
        /// <param name="firstName">The first name of the record.</param>
        /// <param name="lastName">The last name of the record.</param>
        /// <param name="dateOfBirth">The date of birth of the record.</param>
        /// <param name="age">The age of the record.</param>
        /// <param name="salary">The salary of the record.</param>
        /// <param name="gender">The gender of the record.</param>
        void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char gender);

        /// <summary>
        /// Gets all records from the file cabinet.
        /// </summary>
        /// <returns>A read-only collection of file cabinet records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the number of records in the file cabinet.
        /// </summary>
        /// <returns>The number of records in the file cabinet.</returns>
        int GetStat();

        /// <summary>
        /// Finds records by first name.
        /// </summary>
        /// <param name="firstName">The first name to search for.</param>
        /// <returns>A read-only collection of records with the specified first name.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds records by last name.
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>A read-only collection of records with the specified last name.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>A read-only collection of records with the specified date of birth.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// Make a shot.
        /// </summary>
        /// <returns>Shot.</returns>
        FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Removes the record with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the record to remove.</param>
        void RemoveRecord(int id);
    }
}
