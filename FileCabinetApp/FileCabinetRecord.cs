﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        required public string FirstName { get; set; }

        required public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Age { get; set; }

        public decimal Salary { get; set; }

        public char Gender { get; set; }
    }
}
