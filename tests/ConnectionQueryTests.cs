using Xunit;
using System.Linq;

namespace Skinny
{
  public class when_mapping_results_to_field
  {
    public when_mapping_results_to_field()
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
    public void should_map_first_record() => Assert.True(actual.Any(x => x.title == "some testing"));
    public void should_map_second_record() => Assert.True(actual.Any(x => x.title == "other testing"));

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title;
    }
  }

  public class when_mapping_results_to_property
  {
    public when_mapping_results_to_property()
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
    public void should_map_first_record() => Assert.True(actual.Any(x => x.title == "some testing"));

    [Fact]
    public void should_map_second_record() => Assert.True(actual.Any(x => x.title == "other testing"));

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title { get; set; }
    }
  }
}