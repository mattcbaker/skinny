using System;
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

    public T Query<T>(string query)
    {
      var postgresConnection = OpenPostgresConnection();

      var postgresCommand = postgresConnection.CreateCommand();
      postgresCommand.CommandText = query;

      var queryResults = postgresCommand.ExecuteReader();

      return MapQueryResultToType<T>(queryResults);
    }

    T MapQueryResultToType<T>(NpgsqlDataReader reader)
    {
      var mapped = Activator.CreateInstance<T>();

      var column = reader.GetColumnSchema()[0];

      var field = mapped.GetType().GetField(column.ColumnName);

      reader.Read();

      field.SetValue(mapped, reader.GetValue((int)column.ColumnOrdinal));

      return mapped;
    }

    NpgsqlConnection OpenPostgresConnection()
    {
      var connection = new NpgsqlConnection(connectionString);
      connection.Open();
      return connection;
    }
  }
}
