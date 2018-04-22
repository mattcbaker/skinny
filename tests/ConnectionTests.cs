using System;
using Xunit;

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
    public void should_return_negative_one() => Assert.Equal(1, actual);

    static int actual;
  }
}