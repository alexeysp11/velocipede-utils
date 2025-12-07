using System.Data;
using Microsoft.Data.Sqlite;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using System.Data.Common;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;
using VelocipedeUtils.Shared.DbOperations.Models.QueryParameters;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections;

/// <summary>
/// Class for using SQLite database.
/// </summary>
public sealed class SqliteDbConnection : BaseVelocipedeDbConnection, IVelocipedeDbConnection
{
    /// <inheritdoc/>
    public string? ConnectionString { get; set; }

    /// <inheritdoc/>
    public VelocipedeDatabaseType DatabaseType => VelocipedeDatabaseType.SQLite;

    /// <inheritdoc/>
    public string DatabaseName => GetDatabaseName(ConnectionString);

    /// <inheritdoc/>
    public bool IsConnected => Connection != null;

    /// <inheritdoc/>
    public DbConnection? Connection => _connection;

    /// <inheritdoc/>
    public DbTransaction? Transaction => _transaction;

    private SqliteConnection? _connection;
    private SqliteTransaction? _transaction;

    private readonly string _getTablesInDbSql;
    private readonly string _getColumnsSql;
    private readonly string _getTriggersSql;
    private readonly string _getSqlDefinitionSql;

    /// <summary>
    /// Creates an instance of <see cref="SqliteDbConnection"/> by connection string.
    /// </summary>
    /// <param name="connectionString">Specified connection string.</param>
    public SqliteDbConnection(string? connectionString = null)
    {
        ConnectionString = connectionString;

        _getTablesInDbSql = @"
SELECT name FROM sqlite_master
WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%'
UNION ALL
SELECT name FROM sqlite_temp_master
WHERE type IN ('table','view')
ORDER BY 1";

        _getColumnsSql = @"
SELECT
    2 as DatabaseType,
    name as ColumnName,
    type as NativeColumnType,
    dflt_value as DefaultValue,
    pk as IsPrimaryKey,
    not ""notnull"" as IsNullable
FROM pragma_table_info(@TableName)";

        _getTriggersSql = @"
SELECT
    --type,
    name AS TriggerName,
    tbl_name AS EventObjectTable,
    rootpage AS RootPage,
    sql AS SqlDefinition
FROM sqlite_master
WHERE type = 'trigger' AND lower(tbl_name) = lower(@TableName)";

        _getSqlDefinitionSql = "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = @TableName";
    }

    /// <inheritdoc/>
    public DbConnection CreateConnection(string connectionString)
    {
        return new SqliteConnection(connectionString);
    }

    /// <inheritdoc/>
    public bool DbExists()
    {
        if (string.IsNullOrEmpty(ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        if (_connection != null && _connection.ConnectionString == ConnectionString)
            return true;

        try
        {
            return File.Exists(DatabaseName);
        }
        catch (VelocipedeDbConnectParamsException)
        {
            throw;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection CreateDb()
    {
        if (DbExists())
            throw new InvalidOperationException(ErrorMessageConstants.DatabaseAlreadyExists);

        try
        {
            using FileStream fs = File.Create(DatabaseName);
            fs.Close();
        }
        catch (VelocipedeDbConnectParamsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbCreateException(ex);
        }
        
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection OpenDb()
    {
        if (string.IsNullOrEmpty(ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        if (!DbExists())
            throw new VelocipedeDbConnectParamsException();

        try
        {
            CloseDb();
            _connection = new SqliteConnection(ConnectionString);
            _connection.Open();
            return this;
        }
        catch (ArgumentException ex)
        {
            throw new VelocipedeConnectionStringException(ex);
        }
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection SwitchDb(
        string? dbName,
        out string connectionString)
    {
        if (string.IsNullOrEmpty(dbName))
            throw new VelocipedeDbNameException();

        try
        {
            // Change connection string.
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                ConnectionString = ConnectionString ?? "",
                DataSource = dbName
            };
            connectionString = connectionStringBuilder.ConnectionString;
            ConnectionString = connectionString;

            // Connect to the new database.
            return OpenDb();
        }
        catch (VelocipedeDbConnectParamsException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            throw new VelocipedeConnectionStringException(ex);
        }
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection CloseDb()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection BeginTransaction()
    {
        if (_connection == null)
        {
            OpenDb();
        }
        _transaction = _connection!.BeginTransaction();
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection CommitTransaction()
    {
        if (!IsConnected || _transaction == null)
            throw new InvalidOperationException(ErrorMessageConstants.UnableToCommitNotOpenTransaction);

        _transaction.Commit();
        _transaction.Dispose();
        _transaction = null;
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection RollbackTransaction()
    {
        if (!IsConnected || _transaction == null)
            throw new InvalidOperationException(ErrorMessageConstants.UnableToRollbackNotOpenTransaction);

        _transaction.Rollback();
        _transaction.Dispose();
        _transaction = null;
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetTablesInDb(out List<string> tables)
    {
        return Query(_getTablesInDbSql, out tables);
    }

    /// <inheritdoc/>
    public Task<List<string>> GetTablesInDbAsync()
    {
        return QueryAsync<string>(_getTablesInDbSql);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetColumns(
        string tableName,
        out List<VelocipedeNativeColumnInfo> columnInfo)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return Query(_getColumnsSql, parameters, out columnInfo);
    }

    /// <inheritdoc/>
    public Task<List<VelocipedeNativeColumnInfo>> GetColumnsAsync(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return QueryAsync<VelocipedeNativeColumnInfo>(_getColumnsSql, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetForeignKeys(
        string tableName,
        out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        tableName = tableName.Trim('"');

        // Get list of dynamic objects.
        // In some versions of SQLite, PRAGMA statements could not support paramters.
        string sql = $"PRAGMA foreign_key_list('{tableName}')";
        Query(sql, out List<dynamic> foreignKeyInfoDynamic);

        // Wrap the result.
        foreignKeyInfo = GetForeignKeyListFromDynamic(foreignKeyInfoDynamic);

        return this;
    }

    /// <inheritdoc/>
    public async Task<List<VelocipedeForeignKeyInfo>> GetForeignKeysAsync(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        tableName = tableName.Trim('"');

        // Get list of dynamic objects.
        // In some versions of SQLite, PRAGMA statements could not support paramters.
        string sql = $"PRAGMA foreign_key_list('{tableName}')";
        List<dynamic> foreignKeyInfoDynamic = await QueryAsync<dynamic>(sql);

        // Wrap the result.
        return GetForeignKeyListFromDynamic(foreignKeyInfoDynamic);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetTriggers(string tableName, out List<VelocipedeTriggerInfo> triggerInfo)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return Query(_getTriggersSql, parameters, out triggerInfo);
    }

    /// <inheritdoc/>
    public Task<List<VelocipedeTriggerInfo>> GetTriggersAsync(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return QueryAsync<VelocipedeTriggerInfo>(_getTriggersSql, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string? sqlDefinition)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return QueryFirstOrDefault(_getSqlDefinitionSql, parameters, out sqlDefinition);
    }

    /// <inheritdoc/>
    public Task<string?> GetSqlDefinitionAsync(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return QueryFirstOrDefaultAsync<string>(_getSqlDefinitionSql, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection CreateTemporaryTable(string tableName)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection ClearTemporaryTable(string tableName)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection QueryDataTable(string sqlRequest, out DataTable dtResult)
    {
        return QueryDataTable(
            sqlRequest,
            parameters: null,
            dtResult: out dtResult);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection QueryDataTable(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        out DataTable dtResult)
    {
        return QueryDataTable(
            sqlRequest,
            parameters,
            predicate: null,
            dtResult: out dtResult);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection QueryDataTable(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<dynamic, bool>? predicate,
        out DataTable dtResult)
    {
        Query(sqlRequest, parameters, predicate, out List<dynamic> dynamicList);
        dtResult = dynamicList.ToDataTable();
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection QueryDataTable(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<dynamic, bool>? predicate,
        VelocipedePaginationInfo paginationInfo,
        out DataTable dtResult)
    {
        string sqlPaginated = GetPaginatedSql(sqlRequest, paginationInfo);
        return QueryDataTable(sqlPaginated, parameters, predicate, out dtResult);
    }

    /// <inheritdoc/>
    public Task<DataTable> QueryDataTableAsync(string sqlRequest)
    {
        return QueryDataTableAsync(sqlRequest, parameters: null);
    }

    /// <inheritdoc/>
    public Task<DataTable> QueryDataTableAsync(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        return QueryDataTableAsync(sqlRequest, parameters, predicate: null);
    }

    /// <inheritdoc/>
    public async Task<DataTable> QueryDataTableAsync(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<dynamic, bool>? predicate)
    {
        List<dynamic> dynamicList = await QueryAsync(sqlRequest, parameters, predicate);
        return dynamicList.ToDataTable();
    }

    /// <inheritdoc/>
    public Task<DataTable> QueryDataTableAsync(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<dynamic, bool>? predicate,
        VelocipedePaginationInfo paginationInfo)
    {
        string sqlPaginated = GetPaginatedSql(sqlRequest, paginationInfo);
        return QueryDataTableAsync(sqlPaginated, parameters, predicate);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection Execute(string sqlRequest)
    {
        return Execute(sqlRequest, parameters: null);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection Execute(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        InternalExecute(this, sqlRequest, parameters);
        return this;
    }

    /// <inheritdoc/>
    public Task ExecuteAsync(string sqlRequest)
    {
        return ExecuteAsync(sqlRequest, parameters: null);
    }

    /// <inheritdoc/>
    public Task ExecuteAsync(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        return InternalExecuteAsync(this, sqlRequest, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection Query<T>(
        string sqlRequest,
        out List<T> result)
    {
        return Query(sqlRequest, null, out result);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection Query<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        out List<T> result)
    {
        return Query(
            sqlRequest,
            parameters,
            predicate: null,
            result: out result);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection Query<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate,
        out List<T> result)
    {
        result = InternalQuery(this, sqlRequest, parameters, predicate);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection Query<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate,
        VelocipedePaginationInfo paginationInfo,
        out List<T> result)
    {
        string sqlPaginated = GetPaginatedSql(sqlRequest, paginationInfo);
        return Query(sqlPaginated, parameters, predicate, out result);
    }

    /// <inheritdoc/>
    public Task<List<T>> QueryAsync<T>(string sqlRequest)
    {
        return QueryAsync<T>(sqlRequest, parameters: null);
    }

    /// <inheritdoc/>
    public Task<List<T>> QueryAsync<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        return QueryAsync<T>(sqlRequest, parameters, predicate: null);
    }

    /// <inheritdoc/>
    public Task<List<T>> QueryAsync<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate)
    {
        return InternalQueryAsync(this, sqlRequest, parameters, predicate);
    }

    /// <inheritdoc/>
    public Task<List<T>> QueryAsync<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate,
        VelocipedePaginationInfo paginationInfo)
    {
        string sqlPaginated = GetPaginatedSql(sqlRequest, paginationInfo);
        return QueryAsync(sqlPaginated, parameters, predicate);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection QueryFirstOrDefault<T>(
        string sqlRequest,
        out T? result)
    {
        return QueryFirstOrDefault(
            sqlRequest,
            parameters: null,
            result: out result);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection QueryFirstOrDefault<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        out T? result)
    {
        result = InternalQueryFirstOrDefault<T>(this, sqlRequest, parameters);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection QueryFirstOrDefault<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate,
        out T? result)
    {
        if (predicate != null)
        {
            Query(sqlRequest, parameters, out List<T> list);
            result = list.FirstOrDefault(predicate);
            return this;
        }
        return QueryFirstOrDefault(sqlRequest, parameters, out result);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection QueryFirstOrDefault<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate,
        VelocipedePaginationInfo paginationInfo,
        out T? result)
    {
        string sqlPaginated = GetPaginatedSql(sqlRequest, paginationInfo);
        return QueryFirstOrDefault(sqlPaginated, parameters, predicate, out result);
    }

    /// <inheritdoc/>
    public Task<T?> QueryFirstOrDefaultAsync<T>(string sqlRequest)
    {
        return QueryFirstOrDefaultAsync<T>(sqlRequest, parameters: null);
    }

    /// <inheritdoc/>
    public Task<T?> QueryFirstOrDefaultAsync<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        return InternalQueryFirstOrDefaultAsync<T>(this, sqlRequest, parameters);
    }

    /// <inheritdoc/>
    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate)
    {
        if (predicate != null)
        {
            List<T> list = await QueryAsync<T>(sqlRequest, parameters);
            return list.FirstOrDefault(predicate);
        }
        return await QueryFirstOrDefaultAsync<T>(sqlRequest, parameters);
    }

    /// <inheritdoc/>
    public Task<T?> QueryFirstOrDefaultAsync<T>(
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate,
        VelocipedePaginationInfo paginationInfo)
    {
        string sqlPaginated = GetPaginatedSql(sqlRequest, paginationInfo);
        return QueryFirstOrDefaultAsync(sqlPaginated, parameters, predicate);
    }

    /// <inheritdoc/>
    public IVelocipedeForeachTableIterator WithForeachTableIterator(List<string> tableNames)
    {
        return new VelocipedeForeachTableIterator(this, tableNames);
    }

    /// <inheritdoc/>
    public IVelocipedeAsyncForeachIterator WithAsyncForeachIterator(List<string> tableNames)
    {
        return new VelocipedeAsyncForeachIterator(this, tableNames);
    }

    /// <summary>
    /// Check if the specified database file path is valid.
    /// </summary>
    /// <param name="path">Database file path.</param>
    /// <returns><c>true</c> if database file exists; otherwise, <c>false</c>.</returns>
    public static bool IsDatabaseFilePathValid(string path) => File.Exists(path);

    /// <summary>
    /// Get database file path by connection string.
    /// </summary>
    /// <param name="connectionString">Connection string.</param>
    /// <returns>Database file path.</returns>
    public static string GetDatabaseName(string? connectionString)
    {
        try
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            return connectionStringBuilder.DataSource;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbNameException(ex);
        }
    }

    /// <summary>
    /// Get connection string by database file path.
    /// </summary>
    /// <param name="path">Database file path.</param>
    /// <returns>New connection string.</returns>
    public static string GetConnectionString(string path)
    {
        try
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = path
            };
            return connectionStringBuilder.ConnectionString;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbNameException(ex);
        }
    }

    /// <summary>
    /// Get connection string by database file path.
    /// </summary>
    /// <param name="connectionString">Old connection string.</param>
    /// <param name="path">Database file path.</param>
    /// <returns>New connection string.</returns>
    public static string GetConnectionString(string connectionString, string path)
    {
        try
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                ConnectionString = connectionString,
                DataSource = path
            };
            return connectionStringBuilder.ConnectionString;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbNameException(ex);
        }
    }

    /// <inheritdoc/>
    public string GetPaginatedSql(string sqlRequest, VelocipedePaginationInfo paginationInfo)
    {
        if (string.IsNullOrEmpty(paginationInfo.OrderingFieldName))
        {
            throw new InvalidOperationException(ErrorMessageConstants.OrderingFieldNameCouldNotBeNullOrEmpty);
        }
        return paginationInfo.PaginationType == VelocipedePaginationType.KeysetById
            ? $"SELECT t.* FROM ({sqlRequest}) t WHERE {paginationInfo.OrderingFieldName} > {paginationInfo.Offset} ORDER BY {paginationInfo.OrderingFieldName} LIMIT {paginationInfo.Limit}"
            : $"SELECT t.* FROM ({sqlRequest}) t ORDER BY {paginationInfo.OrderingFieldName} LIMIT {paginationInfo.Limit} OFFSET {paginationInfo.Offset}";
    }

    /// <inheritdoc/>
    public IVelocipedeQueryBuilder GetQueryBuilder()
    {
        return new VelocipedeQueryBuilder(DatabaseType, this);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        CloseDb();
    }

    /// <summary>
    /// Get foreign key list from <see cref="List{T}"/> of <c>dynamic</c>.
    /// </summary>
    /// <param name="foreignKeyInfoDynamic"><see cref="List{T}"/> of <c>dynamic</c> that contins info about foreign keys.</param>
    /// <returns><see cref="List{T}"/> of <see cref="VelocipedeForeignKeyInfo"/>.</returns>
    private static List<VelocipedeForeignKeyInfo> GetForeignKeyListFromDynamic(List<dynamic> foreignKeyInfoDynamic)
    {
        return foreignKeyInfoDynamic
            .Select(keyInfo => new VelocipedeForeignKeyInfo
            {
                ForeignKeyId = keyInfo.id,
                SequenceNumber = keyInfo.seq,
                ToTableName = keyInfo.table,
                FromColumn = keyInfo.from,
                ToColumn = keyInfo.to,
                OnUpdate = keyInfo.on_update,
                OnDelete = keyInfo.on_delete,
                MatchingClause = keyInfo.match,
            })
            .ToList();
    }
}
