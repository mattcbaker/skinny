using System;
using System.Collections.Generic;
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

    public int Command(string command, Dictionary<string, string> parameters)
    {
      var postgresConnection = OpenPostgresConnection();

      var postgresCommand = postgresConnection.CreateCommand();
      postgresCommand.CommandText = command;

      foreach (var parameter in parameters)
      {
        var postgresParameter = new NpgsqlParameter();
        postgresParameter.ParameterName = parameter.Key;
        postgresParameter.Value = parameter.Value;

        postgresCommand.Parameters.Add(postgresParameter);
      }

      return postgresCommand.ExecuteNonQuery();
    }

    public T[] Query<T>(string query, IDictionary<string, string> parameters)
    {
      var postgresConnection = OpenPostgresConnection();

      var postgresCommand = postgresConnection.CreateCommand();
      postgresCommand.CommandText = query;

      foreach (var parameter in parameters)
      {
        var postgresParameter = new NpgsqlParameter();
        postgresParameter.ParameterName = parameter.Key;
        postgresParameter.Value = parameter.Value;

        postgresCommand.Parameters.Add(postgresParameter);
      }

      var queryResults = postgresCommand.ExecuteReader();

      return MapQueryResultToType<T>(queryResults);
    }

    T[] MapQueryResultToType<T>(NpgsqlDataReader reader)
    {
      var result = new List<T>();

      if (!reader.HasRows) return result.ToArray();

      while (reader.Read())
      {
        foreach (var column in reader.GetColumnSchema())
        {
          var mapped = Activator.CreateInstance<T>();

          if (TypeHasFieldWithThisName(typeof(T), column.ColumnName))
          {
            var field = mapped.GetType().GetField(column.ColumnName);
            field.SetValue(mapped, reader.GetValue((int)column.ColumnOrdinal));
          }
          else
          {
            var property = mapped.GetType().GetProperty(column.ColumnName);
            property.SetValue(mapped, reader.GetValue((int)column.ColumnOrdinal));
          }

          result.Add(mapped);
        }
      }

      return result.ToArray();
    }

    bool TypeHasFieldWithThisName(Type type, string fieldName) => type.GetField(fieldName) != null;

    NpgsqlConnection OpenPostgresConnection()
    {
      var connection = new NpgsqlConnection(connectionString);
      connection.Open();
      return connection;
    }
  }
}
