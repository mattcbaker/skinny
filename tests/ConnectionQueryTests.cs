using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Skinny
{
  public class when_mapping_results_to_field
  {
    public when_mapping_results_to_field()
    {

      TestingUtils.PostgresCommand("DROP TABLE IF EXISTS skinny_testing");
      TestingUtils.PostgresCommand("CREATE TABLE skinny_testing (title varchar(100))");
      TestingUtils.PostgresCommand("INSERT INTO skinny_testing (title) VALUES ('some testing')");
      TestingUtils.PostgresCommand("INSERT INTO skinny_testing (title) VALUES ('other testing')");

      var connection = new Connection(Settings.ConnectionString);

      var query = "SELECT * FROM skinny_testing";
      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, new Dictionary<string, object>());
    }

    [Fact]
    public void should_return_two_rows() => Assert.Equal(2, actual.Length);

    [Fact]
    public void should_map_first_record() => Assert.Contains(actual, x => x.title == "some testing");

    [Fact]
    public void should_map_second_record() => Assert.Contains(actual, x => x.title == "other testing");

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title = string.Empty;
    }
  }

  public class when_mapping_results_to_property
  {
    public when_mapping_results_to_property()
    {
      TestingUtils.PostgresCommand("DROP TABLE IF EXISTS skinny_testing");
      TestingUtils.PostgresCommand("CREATE TABLE skinny_testing (title varchar(100))");
      TestingUtils.PostgresCommand("INSERT INTO skinny_testing (title) VALUES ('some testing')");
      TestingUtils.PostgresCommand("INSERT INTO skinny_testing (title) VALUES ('other testing')");

      var connection = new Connection(Settings.ConnectionString);

      var query = "SELECT * FROM skinny_testing";
      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, new Dictionary<string, object>());
    }

    [Fact]
    public void should_map_first_record() => Assert.Contains(actual, x => x.title == "some testing");

    [Fact]
    public void should_map_second_record() => Assert.Contains(actual, x => x.title == "other testing");

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
      TestingUtils.PostgresCommand("DROP TABLE IF EXISTS skinny_testing");
      TestingUtils.PostgresCommand("CREATE TABLE skinny_testing (title varchar(100), description varchar(100))");
      TestingUtils.PostgresCommand("INSERT INTO skinny_testing (title, description) VALUES ('some testing', 'some description')");
      TestingUtils.PostgresCommand("INSERT INTO skinny_testing (title, description) VALUES ('other testing', 'another description')");

      var connection = new Connection(Settings.ConnectionString);

      var query = "SELECT * FROM skinny_testing";
      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, new Dictionary<string, object>());
    }

    [Fact]
    public void should_map_first_record_title() => Assert.Contains(actual, x => x.title == "some testing");

    [Fact]
    public void should_map_first_record_description() => Assert.Contains(actual, x => x.description == "some description");

    [Fact]
    public void should_map_second_record_title() => Assert.Contains(actual, x => x.title == "other testing");

    [Fact]
    public void should_map_second_record_description() => Assert.Contains(actual, x => x.description == "another description");

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

      TestingUtils.PostgresCommand("DROP TABLE IF EXISTS skinny_testing");
      TestingUtils.PostgresCommand("CREATE TABLE skinny_testing (title varchar(100))");
      TestingUtils.PostgresCommand("INSERT INTO skinny_testing (title) VALUES ('some testing')");

      var connection = new Connection(Settings.ConnectionString);

      var query = "SELECT * FROM skinny_testing where title = @title";

      var parameters = new Dictionary<string, object>() { { "title", "some testing" } };

      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, parameters);
    }

    [Fact]
    public void should_map_first_record() => Assert.Contains(actual, x => x.title == "some testing");

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title = string.Empty;
    }
  }

    public class when_querying_with_more_columns_than_what_is_being_mapped
  {
    public when_querying_with_more_columns_than_what_is_being_mapped()
    {

      TestingUtils.PostgresCommand("DROP TABLE IF EXISTS skinny_testing");
      TestingUtils.PostgresCommand("CREATE TABLE skinny_testing (title varchar(100), not_being_mapped varchar(200))");
      TestingUtils.PostgresCommand("INSERT INTO skinny_testing (title) VALUES ('some testing')");

      var connection = new Connection(Settings.ConnectionString);

      var query = "SELECT * FROM skinny_testing where title = @title";

      var parameters = new Dictionary<string, object>() { { "title", "some testing" } };

      actual = connection.Query<SkinnyTestingDatabaseRecord>(query, parameters);
    }

    [Fact]
    public void should_map_first_record() => Assert.Contains(actual, x => x.title == "some testing");

    static SkinnyTestingDatabaseRecord[] actual;

    class SkinnyTestingDatabaseRecord
    {
      public string title = string.Empty;
    }
  }
}