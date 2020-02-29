using System;
using System.Data.SQLite;
using Xunit;
using Useful.Sqlite.Extensions;
using System.Globalization;

namespace Useful.Sqlite.Extensions.Tests
{
    public class SQLiteDataReaderExtensions_Tests
    {
        SQLiteConnection connection = new SQLiteConnection("Data Source = :memory:");

        [Theory]
        [InlineData("05")]
        [InlineData("03:01")]
        [InlineData("05:03:01")]
        [InlineData("11:05:03:01")]
        [InlineData("1:07:08:11.1634")]
        public void GetTimeSpanFromString_Test_Pass(string testTimeSpan)
        {
            SQLiteDatabaseSetup("nvarchar(10)", $"'{testTimeSpan}'");

            SQLiteDataReader reader = SQLiteSelectDataReader();
            TimeSpan actual = reader.GetTimeSpanFromString(0);
            TimeSpan expected = TimeSpan.Parse(testTimeSpan);
            Assert.Equal(expected, actual);

            SQLiteDatabaseTearDown();
        }

        #region GetTimeSpanTicksFromLong Tests
        [Theory]
        [InlineData(13624)]
        [InlineData(246246)]
        [InlineData(234)]
        [InlineData(247536)]
        [InlineData(32875227)]
        public void GetTimeSpanTicksFromLong_Test_Pass(long testTicks)
        {
            SQLiteDatabaseSetup("integer", testTicks.ToString());

            SQLiteDataReader reader = SQLiteSelectDataReader();
            TimeSpan actual = reader.GetTimeSpanTicksFromLong(0);
            TimeSpan expected = new TimeSpan(testTicks);
            Assert.Equal(expected, actual);

            SQLiteDatabaseTearDown();
        }

        [Fact]
        public void GetTimeSpanTicksFromLong_Test_Fail()
        {
            SQLiteDatabaseSetup("integer", "'asd'");

            SQLiteDataReader reader = SQLiteSelectDataReader();
            int columnNumber = 0;
            string expectedMessage = $"The value in column {columnNumber} could not be cast to long.";
            Action action = () => reader.GetTimeSpanTicksFromLong(columnNumber);
            Exception ex = Assert.Throws<InvalidCastException>(action);
            Assert.Equal(expectedMessage, ex.Message);

            SQLiteDatabaseTearDown();
        }
        #endregion GetTimeSpanTicksFromLong Tests

        #region GetDateTime Tests
        [Theory]
        [InlineData("2012-05-18", "yyyy-MM-dd")]
        [InlineData("2013-17-08", "yyyy-dd-MM")]
        [InlineData("03-19-2011", "MM-dd-yyyy")]
        [InlineData("20-05-2018", "dd-MM-yyyy")]
        public void GetDateTime_Format_Test_Pass(string testDateTime, string format)
        {
            SQLiteDatabaseSetup("DateTime", $"'{testDateTime}'");

            SQLiteDataReader reader = SQLiteSelectDataReader();
            DateTime actual = reader.GetDateTime(0, format);
            DateTime expected = DateTime.ParseExact(testDateTime, format, null);

            Assert.Equal(expected, actual);

            SQLiteDatabaseTearDown();
        }

        [Theory]
        [InlineData("2012-05-18", "yyyy-MM-dd", "en-US")]
        [InlineData("2013-17-08", "yyyy-dd-MM", "es-ES")]
        [InlineData("03-19-2011", "MM-dd-yyyy", "it-IT")]
        [InlineData("20-05-2018", "dd-MM-yyyy", "fr-FR")]
        public void GetDateTime_IFormatProvider_Test_Pass(string testDateTime, string format, string culture)
        {
            CultureInfo cultureInfo = new CultureInfo(culture);
            SQLiteDatabaseSetup("DateTime", $"'{testDateTime}'");

            SQLiteDataReader reader = SQLiteSelectDataReader();
            DateTime actual = reader.GetDateTime(0, format, cultureInfo);
            DateTime expected = DateTime.ParseExact(testDateTime, format, cultureInfo);

            Assert.Equal(expected, actual);

            SQLiteDatabaseTearDown();
        }
        #endregion GetDateTime Tests

        #region GetDateTimeNullable Tests
        [Theory]
        [InlineData("'2012-05-18'", "yyyy-MM-dd")]
        [InlineData("'2013-17-08'", "yyyy-dd-MM")]
        [InlineData("null", "MM-dd-yyyy")]
        [InlineData("'20-05-2018'", "dd-MM-yyyy")]
        public void GetDateTimeNullable_Format_Test_Pass(string testDateTime, string format)
        {
            SQLiteDatabaseSetup("DateTime", $"{testDateTime}");

            SQLiteDataReader reader = SQLiteSelectDataReader();
            DateTime? actual = reader.GetDateTimeNullable(0, format);
            DateTime? expected;

            if (testDateTime == "null")
                expected = null;
            else
                expected = DateTime.ParseExact(testDateTime.Replace("'", ""), format, null);

            Assert.Equal(expected, actual);

            SQLiteDatabaseTearDown();
        }

        [Theory]
        [InlineData("'2012-05-18'", "yyyy-MM-dd", "en-US")]
        [InlineData("'2013-17-08'", "yyyy-dd-MM", "es-ES")]
        [InlineData("null", "MM-dd-yyyy", "it-IT")]
        [InlineData("'20-05-2018'", "dd-MM-yyyy", "fr-FR")]
        public void GetDateTimeNullable_IFormatProvider_Test_Pass(string testDateTime, string format, string culture)
        {
            CultureInfo cultureInfo = new CultureInfo(culture);
            SQLiteDatabaseSetup("DateTime", $"{testDateTime}");

            SQLiteDataReader reader = SQLiteSelectDataReader();
            DateTime? actual = reader.GetDateTimeNullable(0, format, cultureInfo);
            DateTime? expected;

            if (testDateTime == "null")
                expected = null;
            else
                expected = DateTime.ParseExact(testDateTime.Replace("'", ""), format, null);

            Assert.Equal(expected, actual);

            SQLiteDatabaseTearDown();
        }
        #endregion GetDateTimeNullable Tests

        /// <summary>
        /// Opens the connection to the in-memory database, creates a table and inserts a value into it.
        /// </summary>
        /// <param name="testValueType">The type of the table column that will hold the test value.</param>
        /// <param name="testValue">The value to insert into the test table.</param>
        void SQLiteDatabaseSetup(string testValueType, string testValue)
        {
            connection.Open();
            string createTableStatement = $"CREATE TABLE test (testValue {testValueType})";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableStatement, connection);
            createTableCommand.ExecuteNonQuery();
            string insertStatement = $"INSERT INTO test VALUES ({testValue})";
            SQLiteCommand insertCommand = new SQLiteCommand(insertStatement, connection);
            insertCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets an SQLiteDataReader with loaded the test value from the database.
        /// </summary>
        /// <returns>An SQLiteDataReader already having read the test value from the database.</returns>
        SQLiteDataReader SQLiteSelectDataReader()
        {
            string selectStatement = "SELECT * FROM test";
            SQLiteCommand selectCommand = new SQLiteCommand(selectStatement, connection);
            SQLiteDataReader reader = selectCommand.ExecuteReader();
            reader.Read();

            return reader;
        }

        /// <summary>
        /// Disposes of the SQLiteConnection.
        /// </summary>
        void SQLiteDatabaseTearDown()
        {
            connection.Dispose();
        }
    }
}
