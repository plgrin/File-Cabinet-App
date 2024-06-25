using System;
using System.Collections.Generic;

namespace FileCabinetGenerator
{
    public static class RecordGenerator
    {
        private static readonly Random random = new Random();

        public static IEnumerable<FileCabinetRecord> GenerateRecords(int startId, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                yield return new FileCabinetRecord
                {
                    Id = startId + i,
                    FirstName = GenerateRandomString(2, 60),
                    LastName = GenerateRandomString(2, 60),
                    DateOfBirth = GenerateDateOfBirth(),
                    Age = GenerateAge(),
                    Salary = GenerateSalary(),
                    Gender = GenerateGender()
                };
            }
        }

        private static string GenerateRandomString(int minLength, int maxLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int length = random.Next(minLength, maxLength + 1);
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        private static DateTime GenerateDateOfBirth()
        {
            var start = new DateTime(1950, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }

        private static short GenerateAge()
        {
            return (short)random.Next(0, 121); // random age between 0 and 120
        }

        private static decimal GenerateSalary()
        {
            return (decimal)(random.Next(0, 1000000) + random.NextDouble()); // random salary
        }

        private static char GenerateGender()
        {
            const string genders = "MF";
            return genders[random.Next(genders.Length)];
        }
    }
}
