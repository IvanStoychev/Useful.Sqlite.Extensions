using System;
using System.Data.SQLite;
using Xunit;
using Useful.Sqlite.Extensions;

namespace Useful.Sqlite.Extensions.Tests
{
    public class SQLiteDataReaderExtensions_Tests
    {
        [Fact]
        public void Test1()
        {
            SQLiteCommand aaa = new SQLiteCommand();
            SQLiteDataReader asd = aaa.ExecuteReader();
            DateTime a = asd.GetDateTime(5);
            DateTime? b = asd.GetDateTime(5);
            var c = asd.GetDateTime(5);
        }
    }
}
