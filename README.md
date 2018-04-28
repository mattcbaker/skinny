# Skinny
Skinny is a fast, lightweight Postgres ORM written on .Net Core.

The only [dependency](lib/skinny.csproj) that Skinny has taken is the [npgsql driver](http://www.npgsql.org/).

## Installing
`dotnet add package Skinny`

## Executing a command
```
connection.Command("CREATE TABLE skinny_testing (title varchar(100))", new Dictionary<string, object>());
```

## Executing a query
```
connection.Query<Person>("SELECT * FROM people", new Dictionary<string, object>());
```

## Executing a query with parameters
```
var parameters = new Dictionary<string, object>() { { "name", "jane" } };

connection.Query<Person>("SELECT * FROM people where name = @name", parameters);
```

## Why would I use this?
You might like Skinny if:
* You are looking for a Postgres api with a small footprint.
* You prefer to write SQL.

You might not like Skinny if:
* You prefer to not write SQL.
* You like to create smart mappings between your model and your database (other than just by name).

## Skinny Api
The Skinny Api exists in [Connection.cs](lib/Connection.cs).

**Constructor**

`Connection(string connectionString)`

The constructor accepts a Postgres connection string.

**Query**

`T[] Query<T>(string query, IDictionary<string, object> parameters)`

The query api accepts a query to be ran and a dictionary of parameters, and it will return an array of mapped results. The mapping is based on the column names in the result of the query. Skinny will attempt to map each column name to a property or field _of the same name_ on the result type.

**Command**

`int Command(string command, IDictionary<string, object> parameters)`

The command api accepts a command to be ran and a dictionary of parameters, and it will return an `int` indicating the number of affected rows. The return will be `-1` if no rows are affected.

## Performance
Skinny's performance characteristics can be seen in the [performance test file](tests/PerformanceTests.cs).

## Running the tests
Copy `tests/.env.example` to  `tests/.env`. Edit the file to include your settings values and then run the tests with `dotnet test`.

## License
Skinny is licensed under the Apache 2 license. The license [can be viewed here](LICENSE).
