using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Provides methods for converting strings to various data types.
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Converts a string to a string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A tuple containing a boolean indicating success, an error message if applicable, and the converted value.</returns>

        public static Tuple<bool, string, string> StringConverter(string input)
        {
            return Tuple.Create(true, string.Empty, input);
        }

        /// <summary>
        /// Converts a string to a DateTime.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A tuple containing a boolean indicating success, an error message if applicable, and the converted value.</returns>
        public static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            if (DateTime.TryParse(input, out var date))
            {
                return Tuple.Create(true, string.Empty, date);
            }

            return Tuple.Create(false, "Invalid date format", DateTime.MinValue);
        }

        /// <summary>
        /// Converts a string to a short.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A tuple containing a boolean indicating success, an error message if applicable, and the converted value.</returns>
        public static Tuple<bool, string, short> ShortConverter(string input)
        {
            if (short.TryParse(input, out var value))
            {
                return Tuple.Create(true, string.Empty, value);
            }

            return Tuple.Create(false, "Invalid number format", (short)0);
        }

        /// <summary>
        /// Converts a string to a decimal.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A tuple containing a boolean indicating success, an error message if applicable, and the converted value.</returns>
        public static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            if (decimal.TryParse(input, out var value))
            {
                return Tuple.Create(true, string.Empty, value);
            }

            return Tuple.Create(false, "Invalid number format", 0m);
        }

        /// <summary>
        /// Converts a string to a char.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A tuple containing a boolean indicating success, an error message if applicable, and the converted value.</returns>
        public static Tuple<bool, string, char> CharConverter(string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Length == 1)
            {
                return Tuple.Create(true, string.Empty, input[0]);
            }

            return Tuple.Create(false, "Invalid character format", '\0');
        }
    }
}
