using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom validator class that uses specific validation rules.
    /// </summary>
    public class CustomValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        public CustomValidator()
            : base(new List<IRecordValidator>
            {
                new FirstNameValidator(3, 50),
                new LastNameValidator(3, 50),
                new DateOfBirthValidator(new DateTime(1960, 1, 1), DateTime.Now.AddYears(-18)),
                new AgeValidator(18, 100),
                new SalaryValidator(1000, 1000000),
                new GenderValidator(new[] { 'M', 'F', 'N', 'B' })
            })
        {
        }
    }
}
