using System;
using System.Data.SQLite;
using Xunit;
using Useful.Sqlite.Extensions;

namespace Useful.Sqlite.Extensions.Tests
{
    public class SQLiteDataReaderExtensions_Tests
    {
        [Theory]
        [InlineData("05")]
        [InlineData("03:01")]
        [InlineData("05:03:01")]
        [InlineData("11:05:03:01")]
        [InlineData("1:07:08:11.1634")]
        public void GetTimeSpanFromString_Test(string timeSpan)
        {
            #region Setup
            SQLiteConnection connection = new SQLiteConnection("Data Source = :memory:");
            connection.Open();
            string createTableStatement = "CREATE TABLE test (TimeSpan nvarchar(10))";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableStatement, connection);
            createTableCommand.ExecuteNonQuery();
            string insertStatement = $"INSERT INTO test VALUES ('{timeSpan}')";
            SQLiteCommand insertCommand = new SQLiteCommand(insertStatement, connection);
            insertCommand.ExecuteNonQuery();
            #endregion Setup

            #region Execution
            string selectStatement = "SELECT * FROM test";
            SQLiteCommand selectCommand = new SQLiteCommand(selectStatement, connection);
            SQLiteDataReader reader = selectCommand.ExecuteReader();
            reader.Read();
            TimeSpan actual = reader.GetTimeSpanFromString(0);
            TimeSpan expected = TimeSpan.Parse(timeSpan);

            Assert.Equal(expected, actual);
            #endregion Execution

            #region Teardown
            connection.Dispose();
            #endregion Teardown
        }

        [Theory]
        [InlineData(13624)]
        [InlineData(246246)]
        [InlineData(234)]
        [InlineData(247536)]
        [InlineData(32875227)]
        public void GetTimeSpanTicksFromLong_Test(long ticks)
        {
            #region Setup
            SQLiteConnection connection = new SQLiteConnection("Data Source = :memory:");
            connection.Open();
            string createTableStatement = "CREATE TABLE test (TimeSpan integer)";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableStatement, connection);
            createTableCommand.ExecuteNonQuery();
            string insertStatement = $"INSERT INTO test VALUES ('{ticks}')";
            SQLiteCommand insertCommand = new SQLiteCommand(insertStatement, connection);
            insertCommand.ExecuteNonQuery();
            #endregion Setup

            #region Execution
            string selectStatement = "SELECT * FROM test";
            SQLiteCommand selectCommand = new SQLiteCommand(selectStatement, connection);
            SQLiteDataReader reader = selectCommand.ExecuteReader();
            reader.Read();
            TimeSpan actual = reader.GetTimeSpanTicksFromLong(0);
            TimeSpan expected = new TimeSpan(ticks);

            Assert.Equal(expected, actual);
            #endregion Execution

            #region Teardown
            connection.Dispose();
            #endregion Teardown
        }
    }
}
