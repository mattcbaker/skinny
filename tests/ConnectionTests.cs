using Xunit;
using System.Linq;

// Ref https://xunit.github.io/docs/running-tests-in-parallel
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace Skinny
{
  public class when_executing_a_command_that_does_not_alter_rows
  {
    public when_executing_a_command_that_does_not_alter_rows()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand);

      var command = "CREATE TABLE skinny_testing (title varchar(100))";

      actual = connection.Command(command);
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
      connection.Command(dropTableCommand);

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand);

      var insertCommand = "INSERT INTO skinny_testing (title) VALUES ('some testing')";

      actual = connection.Command(insertCommand);
    }

    [Fact]
    public void should_return_one() => Assert.Equal(1, actual);

    static int actual;
  }

  public class when_executing_a_query_that_returns_a_single_row
  {
    public when_executing_a_query_that_returns_a_single_row()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand);

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand);

      var insertCommand = "INSERT INTO skinny_testing (title) VALUES ('some testing')";

      connection.Command(insertCommand);

      var query = "SELECT * FROM skinny_testing";

      actual = connection.Query<SkinnyTestingDatabaseRecord>(query);
    }

    [Fact]
    public void should_return_a_single_record() => Assert.Equal(1, actual.Length);

    [Fact]
    public void should_return_mapped_database_record() => Assert.Equal("some testing", actual[0].title);

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title;
    }
  }

    public class when_executing_a_query_that_returns_multiple_rows
  {
    public when_executing_a_query_that_returns_multiple_rows()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand);

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand);

      connection.Command("INSERT INTO skinny_testing (title) VALUES ('some testing')");
      connection.Command("INSERT INTO skinny_testing (title) VALUES ('other testing')");

      var query = "SELECT * FROM skinny_testing";

      actual = connection.Query<SkinnyTestingDatabaseRecord>(query);
    }

    [Fact]
    public void should_return_two_rows() => Assert.Equal(2, actual.Length);

    [Fact]
    public void should_return_first_record() => Assert.True(actual.Any(x => x.title == "some testing") );
    public void should_return_second_record() => Assert.True(actual.Any(x => x.title == "other testing") );

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title;
    }
  }
}