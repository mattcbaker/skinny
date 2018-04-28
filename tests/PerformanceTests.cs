using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace Skinny
{
  public class when_reading_500_rows
  {
    public when_reading_500_rows()
    {

      TestingUtils.PostgresCommand("DROP TABLE IF EXISTS skinny_testing");
      TestingUtils.PostgresCommand("CREATE TABLE skinny_testing (firstname varchar(100), lastname varchar(100), address varchar(500), age int)");

      var sql = new StringBuilder();
      for (var i = 0; i < 500; i++)
      {
        sql.Append($"INSERT INTO skinny_testing (firstname, lastname, address, age) VALUES ('jane', 'doe', '{i} Walnut Street', {i});");
      }

      TestingUtils.PostgresCommand(sql.ToString());

      var connection = new Connection(Settings.ConnectionString);

      var query = "SELECT * FROM skinny_testing";

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      read = connection.Query<SkinnyTestingDatabaseRecord>(query, new Dictionary<string, object>());

      stopwatch.Stop();

      mappingTimeInMilliseconds = stopwatch.ElapsedMilliseconds;
    }

    [Fact]
    public void should_complete_in_under_150_milliseconds() => Assert.InRange(mappingTimeInMilliseconds, 1, 150);

    static SkinnyTestingDatabaseRecord[] read;
    static long mappingTimeInMilliseconds = 0;

    class SkinnyTestingDatabaseRecord
    {
      public string firstname = string.Empty;
      public string lastname = string.Empty;
      public string address = string.Empty;
      public int age = 0;
    }
  }
}