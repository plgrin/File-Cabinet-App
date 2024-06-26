using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class DefaultSalaryValidator
    {
        public void Validate(decimal salary)
        {
            if (salary < 0)
            {
                throw new ArgumentException("Salary must be a positive number.");
            }
        }
    }
}
