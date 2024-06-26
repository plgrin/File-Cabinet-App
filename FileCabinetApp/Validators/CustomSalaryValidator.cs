using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class CustomSalaryValidator
    {
        public void Validate(decimal salary)
        {
            if (salary < 1000 || salary > 1000000)
            {
                throw new ArgumentException("Salary must be between 1,000 and 1,000,000.");
            }
        }
    }
}
