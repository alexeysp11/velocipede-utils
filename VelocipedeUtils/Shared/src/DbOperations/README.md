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
    .GetAllDataFromTable(tableName, out DataTable dtData)
    .GetColumnsOfTable(tableName, out List<VelocipedeColumnInfo> columnInfo)
    .GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
    .GetTriggers(tableName, out List<VelocipedeTriggerInfo> triggerInfo)
    .GetSqlDefinition(tableName, out string sqlDefinition)
    .CloseDb();
```

This library provides functionality for communicating with relational databases using ADO.NET and Dapper. Information on currently supported database types:
- [x] [SQLite](https://sqlite.org/)
- [x] [PostgreSQL](https://www.postgresql.org/)
- [x] [MS SQL](https://www.microsoft.com/en-us/sql-server)
- [ ] [MySQL](https://www.mysql.com/)
- [ ] [Oracle](https://www.oracle.com/database/)
- [ ] [Clickhouse](https://clickhouse.com/)

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
    .ForeachTable(tables)
        .GetAllDataFromTable()
        .GetColumns()
        .GetForeignKeys()
        .GetTriggers()
        .GetSqlDefinition()
    .EndForeach()
    .GetForeachResult(out VelocipedeForeachResult foreachResult)
    .CloseDb();
```
