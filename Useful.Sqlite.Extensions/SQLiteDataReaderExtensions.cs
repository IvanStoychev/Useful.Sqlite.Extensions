using System;
using System.Data.SQLite;

namespace Useful.Sqlite.Extensions
{
    public static class SQLiteDataReaderExtensions
    {
        public static TimeSpan GetTimeSpanFromString(this SQLiteDataReader reader, int columnNumber)
        {
            string timeSpanString = reader.GetString(columnNumber);
            TimeSpan result = 
        }

        #region GetDateTime
        /// <summary>
        /// Parses the value of the column, with the given number, as a DateTime using the provided format.
        /// </summary>
        /// <param name="columnNumber">The number of the column whose data to parse.</param>
        /// <param name="format">A format specifier that defines how the column's value will be parsed to a DateTime.</param>
        /// <returns>A DateTime representation of the column value.</returns>
        public static DateTime GetDateTime(this SQLiteDataReader reader, int columnNumber, string format)
        {
            string date = reader.GetString(columnNumber);
            DateTime dateTime = DateTime.ParseExact(date, format, null);

            return dateTime;
        }

        /// <summary>
        /// Parses the value of the column, with the given number, as a DateTime using the provided format specifiers.
        /// </summary>
        /// <param name="columnNumber">The number of the column whose data to parse.</param>
        /// <param name="format">A format specifier that defines how the column's value will be parsed to a DateTime.</param>
        /// <param name="formatProvider">An object that supplies culture-specific format information about the column's value.</param>
        /// <returns>A DateTime representation of the column value.</returns>
        public static DateTime GetDateTime(this SQLiteDataReader reader, int columnNumber, string format, IFormatProvider formatProvider)
        {
            string date = reader.GetString(columnNumber);
            DateTime dateTime = DateTime.ParseExact(date, format, formatProvider);

            return dateTime;
        }
        #endregion GetDateTime

        #region GetDateTimeNullable
        /// <summary>
        /// Checks if the value of the given column number is null. If it is - returns null,
        /// otherwise parses it as a DateTime.
        /// </summary>
        /// <param name="columnNumber">The number of the column whose data to get.</param>
        /// <returns>"null" if the column has no value or its value as a DateTime.</returns>
        public static DateTime? GetDateTimeNullable(this SQLiteDataReader reader, int columnNumber)
        {
            if (reader.IsDBNull(columnNumber))
                return null;
            else
                return reader.GetDateTime(columnNumber);
        }

        /// <summary>
        /// Checks if the value of the given column number is null. If it is - returns null,
        /// otherwise parses it as a DateTime using the provided format.
        /// </summary>
        /// <param name="columnNumber">The number of the column whose data to get.</param>
        /// <param name="format">A format specifier that defines how the column's value will be parsed to a DateTime.</param>
        /// <returns>"null" if the column has no value or its value as a DateTime.</returns>
        public static DateTime? GetDateTimeNullable(this SQLiteDataReader reader, int columnNumber, string format)
        {
            if (reader.IsDBNull(columnNumber)) return null;

            string date = reader.GetString(columnNumber);
            DateTime dateTime = DateTime.ParseExact(date, format, null);

            return dateTime;
        }

        /// <summary>
        /// Checks if the value of the given column number is null. If it is - returns null,
        /// otherwise parses it as a DateTime, using the provided date format and IFormatProvider.
        /// </summary>
        /// <param name="columnNumber">The number of the column whose data to get.</param>
        /// <param name="format">A format specifier that defines how the column's value will be parsed to a DateTime.</param>
        /// <param name="formatProvider">An object that supplies culture-specific format information about the column's value.</param>
        /// <returns>"null" if the column has no value or its value as a DateTime.</returns>
        public static DateTime? GetDateTimeNullable(this SQLiteDataReader reader, int columnNumber, string format, IFormatProvider formatProvider)
        {
            if (reader.IsDBNull(columnNumber)) return null;

            string date = reader.GetString(columnNumber);
            DateTime dateTime = DateTime.ParseExact(date, format, formatProvider);

            return dateTime;
        }
        #endregion GetDateTimeNullable
    }
}
