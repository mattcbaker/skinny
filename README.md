# Skinny
Skinny is a fast, lightweight Postgres ORM written on .Net Core.

The only [dependency](lib/skinny.csproj) that Skinny has taken is the [npgsql driver](http://www.npgsql.org/). 

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
