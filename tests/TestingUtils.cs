using System;
using System.Collections.Generic;
using Npgsql;

namespace Skinny
{
  class TestingUtils
  {
    public static void PostgresCommand(string command)
    {
      var postgresConnection = OpenPostgresConnection();

      var postgresCommand = postgresConnection.CreateCommand();
      postgresCommand.CommandText = command;

      postgresCommand.ExecuteNonQuery();
    }

    public static NpgsqlDataReader PostgresQuery(string query)
    {
      var postgresConnection = OpenPostgresConnection();

      var postgresCommand = postgresConnection.CreateCommand();
      postgresCommand.CommandText = query;

      return postgresCommand.ExecuteReader();
    }

    static NpgsqlConnection OpenPostgresConnection()
    {
      var connection = new NpgsqlConnection(Settings.ConnectionString);
      connection.Open();
      return connection;
    }
  }
}