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

    public int Command(string command, IDictionary<string, object> parameters)
    {
      var npgsqlConnection = OpenNpgsqlConnection();

      var npgsqlCommand = npgsqlConnection.CreateCommand();
      npgsqlCommand.CommandText = command;

      AddParametersToNpgsqlCommand(npgsqlCommand, parameters);

      return npgsqlCommand.ExecuteNonQuery();
    }

    public T[] Query<T>(string query, IDictionary<string, object> parameters)
    {
      var npgsqlConnection = OpenNpgsqlConnection();

      var npgsqlCommand = npgsqlConnection.CreateCommand();
      npgsqlCommand.CommandText = query;

      AddParametersToNpgsqlCommand(npgsqlCommand, parameters);

      var queryResults = npgsqlCommand.ExecuteReader();

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
          else if (TypeHasPropertyWithThisName(typeof(T), column.ColumnName))
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
    bool TypeHasPropertyWithThisName(Type type, string propertyName) => type.GetProperty(propertyName) != null;

    NpgsqlConnection OpenNpgsqlConnection()
    {
      var connection = new NpgsqlConnection(connectionString);
      connection.Open();
      return connection;
    }

    void AddParametersToNpgsqlCommand(NpgsqlCommand npgsqlCommand, IDictionary<string, object> parameters)
    {
      foreach (var parameter in parameters)
      {
        var npgsqlParameter = new NpgsqlParameter();
        npgsqlParameter.ParameterName = parameter.Key;
        npgsqlParameter.Value = parameter.Value;

        npgsqlCommand.Parameters.Add(npgsqlParameter);
      }
    }
  }
}
