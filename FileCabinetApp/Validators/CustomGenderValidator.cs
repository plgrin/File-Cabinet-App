using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class CustomGenderValidator
    {
        public void Validate(char gender)
        {
            if (!"MFNB".Contains(gender))
            {
                throw new ArgumentException("Gender must be 'M', 'F', 'N' or 'B'.");
            }
        }
    }
}
