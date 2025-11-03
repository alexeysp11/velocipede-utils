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
    .QueryDataTable(sqlQuery, out DataTable dtResult)
    .CloseDb();
```

Метаданные о базе данных, с которой установлено активное подключение, а также о таблицах в ней можно получить следующим образом:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);

dbConnection
    .OpenDb()
    .GetAllDataFromTable(tableName, out DataTable dtData)       // You can get result as DataTable,
    .GetAllDataFromTable(tableName, out List<Table1> listData)  // or as List<T>.
    .GetColumnsOfTable(tableName, out List<VelocipedeColumnInfo> columnInfo)
    .GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
    .GetTriggers(tableName, out List<VelocipedeTriggerInfo> triggerInfo)
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
- [ ] [Firebird](https://github.com/FirebirdSQL/firebird)

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

### Циклические операции

Также возможно выполнять циклические операции внутри БД в рамках активного подключения:
```C#
using IVelocipedeDbConnection dbConnection
    = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);

dbConnection
    .SetConnectionString(connectionString)
    .OpenDb()
    .GetTablesInDb(out List<string> tables)
    .WithForeachTableIterator(tables)
    .BeginForeach()
        .GetAllDataFromTable()
        .GetColumns()
        .GetForeignKeys()
        .GetTriggers()
        .GetSqlDefinition()
    .EndForeach()
    .GetForeachResult(out VelocipedeForeachResult foreachResult)
    .CloseDb();
```

Этот паттерн для итерации по всем таблицам и сбора их данных/метаданных может быть адаптирован для задач, связанных с интроспекцией БД, аудитом, бэкапом или синхронизацией данных.

### Фильтрация данных

Библиотека поддерживает параметризированные запросы и фильтрацию с помощью лямбда-выражений:
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

Данный функционал подходит для ситуаций, когда:
- Вы получаете большой набор данных из БД (например, для кэширования), а затем хотите многократно фильтровать его в памяти с различными условиями.
- Часть фильтрации должна быть выполнена на сервере БД (для производительности), а другая часть — на клиенте (для сложной логики).
- Вам нужна "пост-обработка" результатов SQL-запроса, которая включает фильтрацию.

**Важно**: `predicate` не транслируется в SQL, а выполняется строго после того, как данные уже получены из базы данных. Если `sql` возвращает очень много записей, а `predicate` отфильтровывает подавляющее большинство из них, то вы перетаскиваете по сети и загружаете в память гораздо больше данных, чем необходимо, что может быть очень неэффективно.
