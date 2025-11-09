# VelocipedeUtils.Shared.DbOperations

[English](README.md) | [Русский](README.ru.md)

The library offers functionality for unified operations with relational databases, simplifying the extraction and insertion of data from disparate sources. This functionality may be necessary in applications such as [sqlviewer](https://github.com/alexeysp11/sqlviewer).

An example of executing the `CREATE TABLE IF NOT EXISTS` command and a given SQL query using the common `IVelocipedeDbConnection` interface is shown below:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType);

dbConnection
    .SetConnectionString(connectionString)
    .OpenDb()
    .CreateDbIfNotExists(newDatabaseName)
    .SwitchDb(newDatabaseName)
    .QueryDataTable(sqlQuery, out DataTable dtResult)
    .CloseDb();
```

Metadata about the database to which an active connection is established, as well as about the tables in it, can be obtained as follows:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);

dbConnection
    .OpenDb()
    .GetAllData(tableName, out DataTable dtData)       // You can get result as DataTable,
    .GetAllData(tableName, out List<Table1> listData)  // or as List<T>.
    .GetColumnsOfTable(tableName, out List<VelocipedeColumnInfo> columnInfo)
    .GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
    .GetTriggers(tableName, out List<VelocipedeTriggerInfo> triggerInfo)
    .GetSqlDefinition(tableName, out string sqlDefinition)
    .CloseDb();
```

This library provides functionality for communicating with relational databases using ADO.NET and Dapper under the hood. Information on currently supported database types:
- [x] [SQLite](https://sqlite.org/)
- [x] [PostgreSQL](https://www.postgresql.org/)
- [x] [MS SQL](https://www.microsoft.com/en-us/sql-server)
- [ ] [MySQL](https://www.mysql.com/)
- [ ] [Oracle](https://www.oracle.com/database/)
- [ ] [Clickhouse](https://clickhouse.com/)
- [ ] [Firebird](https://github.com/FirebirdSQL/firebird)

## Basic Operations

### Create a database

You can create a database by connecting to an existing database and redefining the connection string with the name of the new database:
```C#
dbConnection
    .SetConnectionString(connectionString)
    .OpenDb()
    .GetConnectionString(dbName, out string newConnectionString)
    .SetConnectionString(newConnectionString)
    .CreateDb()
    .SwitchDb(dbName)
    .CloseDb();
```

There is a simplified API for this operation:
```C#
dbConnection
    .SetConnectionString(connectionString)
    .OpenDb()
    .CreateDb(dbName)
    .SwitchDb(dbName)
    .CloseDb();
```

### Cyclic operations

It is also possible to perform cyclic operations within the database within an active connection:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);

dbConnection
    .SetConnectionString(connectionString)
    .OpenDb()
    .GetTablesInDb(out List<string> tables)
    .WithForeachTableIterator(tables)
    .BeginForeach()
        .GetAllData()
        .GetColumns()
        .GetForeignKeys()
        .GetTriggers()
        .GetSqlDefinition()
    .EndForeach()
    .GetForeachResult(out VelocipedeForeachResult foreachResult)
    .CloseDb();
```

This pattern for iterating over all tables and collecting their data/metadata can be adapted for tasks related to database introspection, auditing, backup, or data synchronization.

### Filtering data

The library supports parameterized queries and filtering using lambda expressions:
```C#
string sql = @"SELECT ""Id"", ""Name"" FROM ""TestModels"" WHERE ""Id"" >= @TestModelsId";
List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
Func<TestModel, bool> predicate = x => x.Id >= 7;

using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);

// Get a record whose ID is greater than or equal to 7.
dbConnection
    .SetConnectionString(connectionString)
    .OpenDb()
    .QueryFirstOrDefault(sql, parameters, predicate, out TestModel? result)
    .CloseDb();
```

This functionality is suitable for situations where:
- You retrieve a large dataset from the database (for example, for caching) and then want to repeatedly filter it in memory with different conditions.
- Some of the filtering should be performed on the database server (for performance), and some on the client (for complex logic).
- You need to "post-process" the results of an SQL query, which includes filtering.

**Important**: `predicate` is not translated into SQL, but is executed strictly after the data has already been retrieved from the database. If `sql` returns a large number of records, and `predicate` filters out the vast majority of them, then you are dragging over the network and loading much more data into memory than necessary, which can be very inefficient.

### Async operations

This library also supports asynchronous operations. For example, you can asynchronously retrieve data and metadata about a table as follows:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType);

dbConnection
    .SetConnectionString(connectionString)
    .OpenDb();

DataTable dtData = await dbConnection.GetAllDataAsync(tableName);
List<VelocipedeColumnInfo> columnInfo = await dbConnection.GetColumnsAsync(tableName);
List<VelocipedeForeignKeyInfo> foreignKeyInfo = await dbConnection.GetForeignKeysAsync(tableName);
List<VelocipedeTriggerInfo> triggerInfo = await dbConnection.GetTriggersAsync(tableName);
string sqlDefinition = await dbConnection.GetSqlDefinitionAsync(tableName);

await dbConnection.CloseDbAsync();
```

It is possible to use an iterator to asynchronously retrieve metainformation about the specified tables:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType);

dbConnection
    .SetConnectionString(connectionString)
    .OpenDb();

VelocipedeForeachResult? foreachResult = await dbConnection
    .WithAsyncForeachIterator(tableNames)
    .BeginAsyncForeach()
        .GetAllDataAsync()
        .GetColumnsAsync()
        .GetForeignKeysAsync()
        .GetTriggersAsync()
        .GetSqlDefinitionAsync()
    .EndAsyncForeach()
    .GetResultAsync();
```
