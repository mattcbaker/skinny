using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Skinny
{
  public class when_mapping_results_to_field
  {
    public when_mapping_results_to_field()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand, new Dictionary<string, object>());

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand, new Dictionary<string, object>());

      connection.Command("INSERT INTO skinny_testing (title) VALUES ('some testing')", new Dictionary<string, object>());
      connection.Command("INSERT INTO skinny_testing (title) VALUES ('other testing')", new Dictionary<string, object>());

      var query = "SELECT * FROM skinny_testing";

      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, new Dictionary<string, object>());
    }

    [Fact]
    public void should_return_two_rows() => Assert.Equal(2, actual.Length);

    [Fact]
    public void should_map_first_record() => Assert.True(actual.Any(x => x.title == "some testing"));

    [Fact]
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
      connection.Command(dropTableCommand, new Dictionary<string, object>());

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand, new Dictionary<string, object>());

      connection.Command("INSERT INTO skinny_testing (title) VALUES ('some testing')", new Dictionary<string, object>());
      connection.Command("INSERT INTO skinny_testing (title) VALUES ('other testing')", new Dictionary<string, object>());

      var query = "SELECT * FROM skinny_testing";

      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, new Dictionary<string, object>());
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

  public class when_mapping_multiple_columns
  {
    public when_mapping_multiple_columns()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand, new Dictionary<string, object>());

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100), description varchar(100))";

      connection.Command(createTableCommand, new Dictionary<string, object>());

      connection.Command("INSERT INTO skinny_testing (title, description) VALUES ('some testing', 'some description')", new Dictionary<string, object>());
      connection.Command("INSERT INTO skinny_testing (title, description) VALUES ('other testing', 'another description')", new Dictionary<string, object>());

      var query = "SELECT * FROM skinny_testing";

      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, new Dictionary<string, object>());
    }

    [Fact]
    public void should_map_first_record_title() => Assert.True(actual.Any(x => x.title == "some testing"));

    [Fact]
    public void should_map_first_record_description() => Assert.True(actual.Any(x => x.description == "some description"));

    [Fact]
    public void should_map_second_record_title() => Assert.True(actual.Any(x => x.title == "other testing"));

    [Fact]
    public void should_map_second_record_description() => Assert.True(actual.Any(x => x.description == "another description"));

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title { get; set; }
      public string description { get; set; }
    }
  }

  public class when_querying_with_parameters
  {
    public when_querying_with_parameters()
    {
      var connection = new Connection(Settings.ConnectionString);

      var dropTableCommand = "DROP TABLE IF EXISTS skinny_testing";
      connection.Command(dropTableCommand, new Dictionary<string, object>());

      var createTableCommand = "CREATE TABLE skinny_testing (title varchar(100))";

      connection.Command(createTableCommand, new Dictionary<string, object>());

      connection.Command("INSERT INTO skinny_testing (title) VALUES ('some testing')", new Dictionary<string, object>());

      var query = "SELECT * FROM skinny_testing where title = @title";

      var parameters = new Dictionary<string, object>() { { "title", "some testing" } };

      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, parameters);
    }


    [Fact]
    public void should_map_first_record() => Assert.True(actual.Any(x => x.title == "some testing"));

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title;
    }
  }
}