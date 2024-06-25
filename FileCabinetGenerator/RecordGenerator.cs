using System;
using System.Collections.Generic;

namespace FileCabinetGenerator
{
    public static class RecordGenerator
    {
        public static List<FileCabinetRecord> GenerateRecords(int amount, int startId)
        {
            var records = new List<FileCabinetRecord>();
            var random = new Random();

            for (int i = 0; i < amount; i++)
            {
                var record = new FileCabinetRecord
                {
                    Id = startId + i,
                    FirstName = GetRandomString(random, 5, 10),
                    LastName = GetRandomString(random, 5, 10),
                    DateOfBirth = GetRandomDate(random, new DateTime(1950, 1, 1), DateTime.Now),
                    Age = (short)random.Next(1, 121),
                    Salary = (decimal)(random.Next(1000, 10001) + random.NextDouble()),
                    Gender = random.Next(0, 2) == 0 ? 'M' : 'F'
                };
                records.Add(record);
            }

            return records;
        }

        private static string GetRandomString(Random random, int minLength, int maxLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int length = random.Next(minLength, maxLength + 1);
            var stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        private static DateTime GetRandomDate(Random random, DateTime startDate, DateTime endDate)
        {
            int range = (endDate - startDate).Days;
            return startDate.AddDays(random.Next(range));
        }
    }
}
