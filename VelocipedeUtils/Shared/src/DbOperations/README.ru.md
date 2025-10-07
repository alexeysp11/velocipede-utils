# VelocipedeUtils.Shared.DbOperations

[English](README.md) | [Русский](README.ru.md)

Библиотека предлагает функционал для унифицированных операций с реляционными базами данных, что упрощает извлечение и вставку данных из разнородных источников. Подобный функционал может быть необходим в таких приложениях как [sqlviewer](https://github.com/alexeysp11/sqlviewer).

Пример выполнения команды `CREATE TABLE IF NOT EXISTS` и заданного SQL-запроса с использованием общего интерфейса `IVelocipedeDbConnection` представлен ниже:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType);

dbConnection
    .SetConnectionString(connectionString)
    .OpenDb()
    .CreateDbIfNotExists(newDatabaseName)
    .SwitchDb(newDatabaseName)
    .ExecuteSqlCommand(sqlQuery, out DataTable dtResult)
    .CloseDb();
```

Метаданные о базе данных, с которой установлено активное подключение, а также о таблицах в ней можно получить следующим образом:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);

dbConnection
    .OpenDb()
    .GetAllDataFromTable(tableName, out DataTable dtData)
    .GetColumnsOfTable(tableName, out DataTable dtColumns)
    .GetForeignKeys(tableName, out DataTable dtForeignKeys)
    .GetTriggers(tableName, out DataTable dtTriggers)
    .GetSqlDefinition(tableName, out string sqlDefinition)
    .CloseDb();
```

Данная библиотека предоставляет функционал для коммуникации с реляционными базами данных с использованием ADO.NET и Dapper. Информация о типах БД, которые поддерживаются на текущий момент:
- [x] [SQLite](https://sqlite.org/)
- [x] [PostgreSQL](https://www.postgresql.org/)
- [x] [MS SQL](https://www.microsoft.com/en-us/sql-server)
- [ ] [MySQL](https://www.mysql.com/)
- [ ] [Oracle](https://www.oracle.com/database/)
- [ ] [Clickhouse](https://clickhouse.com/)

## Основные операции

### Создать базу данных

Создать базу данных можно, подключившись к существующей БД и переопределив строку подключения с использованием имени новой БД:
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

Для данной операции есть упрощенный API:
```C#
dbConnection
    .SetConnectionString(connectionString)
    .OpenDb()
    .CreateDb(dbName)
    .SwitchDb(dbName)
    .CloseDb();
```
