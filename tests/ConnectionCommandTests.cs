using System.Collections.Generic;
using Xunit;
using System.Linq;
using Npgsql;

namespace Skinny
{
  public class when_executing_a_command_that_does_not_alter_rows
  {
    public when_executing_a_command_that_does_not_alter_rows()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand, new Dictionary<string, object>());

      var command = "CREATE TABLE skinny_testing (title varchar(100))";

      actual = connection.Command(command, new Dictionary<string, object>());
    }

    [Fact]
    public void should_return_negative_one() => Assert.Equal(-1, actual);

    static int actual;
  }

  public class when_executing_a_command_that_does_alter_rows
  {
    public when_executing_a_command_that_does_alter_rows()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand, new Dictionary<string, object>());

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand, new Dictionary<string, object>());

      var insertCommand = "INSERT INTO skinny_testing (title) VALUES ('some testing')";

      actual = connection.Command(insertCommand, new Dictionary<string, object>());
    }

    [Fact]
    public void should_return_one() => Assert.Equal(1, actual);

    static int actual;
  }

  public class when_executing_a_command_with_parameters
  {
    public when_executing_a_command_with_parameters()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand, new Dictionary<string, object>());

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand, new Dictionary<string, object>());

      var insertCommand = "INSERT INTO skinny_testing (title) VALUES (@title)";
      var parameters = new Dictionary<string, object>() { { "title", "some testing" } };

      actual = connection.Command(insertCommand, parameters);

      writtenToDatabase = TestingUtils.PostgresQuery("SELECT * FROM skinny_testing");
    }

    [Fact]
    public void should_return_one() => Assert.Equal(1, actual);

    [Fact]
    public void should_write_to_the_database() => Assert.True(writtenToDatabase.HasRows);

    [Fact]
    public void should_write_expected_record_to_database()
    {
      writtenToDatabase.Read();

      Assert.Equal("some testing", writtenToDatabase.GetString(0));
    }

    static int actual;
    static NpgsqlDataReader writtenToDatabase;

    class SkinnyCommandTesting
    {
      public string title = string.Empty;
    }
  }

  public class when_executing_lots_of_commands
  {
    [Fact]
    public void should_not_exhaust_connection_pool() => TryToExhaustConnectionPool();

    void TryToExhaustConnectionPool()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand, new Dictionary<string, object>());

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand, new Dictionary<string, object>());

      for (var i = 0; i < 500; i++)
      {
        var insertCommand = $"INSERT INTO skinny_testing (title) VALUES ('some testing{i}')";
        connection.Command(insertCommand, new Dictionary<string, object>());
      }
    }
  }
}