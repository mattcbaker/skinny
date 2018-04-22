using Npgsql;

namespace Skinny
{
  public class Connection
  {
    readonly string connectionString;

    public Connection(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public int Command(string command)
    {
      var postgresConnection = OpenPostgresConnection();

      var postgresCommand = postgresConnection.CreateCommand();
      postgresCommand.CommandText = command;

      return postgresCommand.ExecuteNonQuery();
    }

    NpgsqlConnection OpenPostgresConnection()
    {
      var connection = new NpgsqlConnection(connectionString);
      connection.Open();
      return connection;
    }
  }
}
