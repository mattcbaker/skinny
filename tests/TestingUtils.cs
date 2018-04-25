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

    static NpgsqlConnection OpenPostgresConnection()
    {
      var connection = new NpgsqlConnection(Settings.ConnectionString);
      connection.Open();
      return connection;
    }
  }
}