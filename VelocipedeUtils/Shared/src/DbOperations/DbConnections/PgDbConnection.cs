using System.Data;
using System.Data.Common;
using Npgsql;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections;

/// <summary>
/// PostgreSQL database connection.
/// </summary>
public sealed class PgDbConnection : BaseVelocipedeDbConnection, IVelocipedeDbConnection
{
    /// <summary>
    /// Information about table and schema used after parsing table name.
    /// </summary>
    /// <param name="TableName">Table name.</param>
    /// <param name="SchemaName">Schema name.</param>
    private readonly record struct TableAndSchemaInfo(string TableName, string SchemaName);

    /// <inheritdoc/>
    public string? ConnectionString { get; set; }

    /// <inheritdoc/>
    public DatabaseType DatabaseType => DatabaseType.PostgreSQL;

    /// <inheritdoc/>
    public string? DatabaseName => GetDatabaseName(ConnectionString);

    /// <inheritdoc/>
    public bool IsConnected => Connection != null;

    /// <inheritdoc/>
    public DbConnection? Connection => _connection;

    /// <inheritdoc/>
    public DbTransaction? Transaction => _transaction;

    private NpgsqlConnection? _connection;
    private NpgsqlTransaction? _transaction;

    private readonly string _getTablesInDbSql;
    private readonly string _getColumnsSql;
    private readonly string _getForeignKeysSql;
    private readonly string _getTriggersSql;
    private readonly string _getSqlDefinitionSql;

    /// <summary>
    /// Creates an instance of <see cref="PgDbConnection"/> by connection string.
    /// </summary>
    /// <param name="connectionString">Specified connection string.</param>
    public PgDbConnection(string? connectionString = null)
    {
        ConnectionString = connectionString;

        _getTablesInDbSql = "SELECT t.schemaname || '.' || t.relname AS name FROM (SELECT schemaname, relname FROM pg_stat_user_tables) t";

        _getColumnsSql = @"
SELECT
    3 as DatabaseType,
    column_name as ColumnName,
    data_type as NativeColumnType,
    character_maximum_length as CharMaxLength,
    numeric_precision as NumericPrecision,
    numeric_scale as NumericScale,
    column_default as DefaultValue,
    case when is_nullable = 'YES' then true else false end as IsNullable
FROM information_schema.columns
WHERE table_schema = @SchemaName AND lower(table_name) = lower(@TableName)";

        _getForeignKeysSql = @"
SELECT
    tc.constraint_name as ConstraintName,
    tc.table_schema as FromTableSchema,
    tc.table_name as FromTableName,
    kcu.column_name as FromColumn,
    ccu.table_schema AS ToTableSchema,
    ccu.table_name AS ToTableName,
    ccu.column_name AS ToColumn
FROM 
    information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name AND tc.table_schema = kcu.table_schema
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name AND ccu.table_schema = tc.table_schema
WHERE tc.constraint_type = 'FOREIGN KEY' AND lower(tc.table_name) = lower(@TableName)";

        _getTriggersSql = @"
SELECT
    trigger_catalog AS TriggerCatalog,
    trigger_schema AS TriggerSchema,
    trigger_name AS TriggerName,
    event_manipulation AS EventManipulation,
    event_object_catalog AS EventObjectCatalog,
    event_object_schema AS EventObjectSchema,
    event_object_table AS EventObjectTable,
    action_order AS ActionOrder,
    action_condition AS ActionCondition,
    action_statement AS ActionStatement,
    action_orientation AS ActionOrientation,
    action_timing AS ActionTiming,
    action_reference_old_table AS ActionReferenceOldTable,
    action_reference_new_table AS ActionReferenceNewTable
FROM information_schema.triggers
WHERE lower(event_object_table) = lower(@TableName)";

        _getSqlDefinitionSql = @"
CREATE OR REPLACE FUNCTION fGetSqlFromTable(aSchemaName VARCHAR(255), aTableName VARCHAR(255))
    RETURNS TEXT
    LANGUAGE plpgsql AS
$func$
DECLARE
    i INTEGER;
    lNumRec INTEGER;
    rec RECORD;
    lResult text;
BEGIN
    i := 0;
    SELECT COUNT(*) INTO lNumRec FROM information_schema.columns WHERE table_schema LIKE aSchemaName AND table_name LIKE aTableName;
    lResult := 'CREATE TABLE ' || aSchemaName || '.' || aTableName || chr(10) || '(' || chr(10);
    FOR rec IN (
        SELECT 
            column_name, 
            column_default, 
            is_nullable, 
            data_type, 
            character_maximum_length
        FROM information_schema.columns
        WHERE table_schema LIKE aSchemaName AND table_name LIKE aTableName
    )
    LOOP
        i := i + 1;
        lResult := lResult || '    ' || rec.column_name || ' ' || rec.data_type;
        IF UPPER(rec.data_type) LIKE '%CHAR%VAR%' THEN 
            lResult := lResult || '(' || rec.character_maximum_length || ')';
        END IF;
        IF rec.column_default IS NOT NULL AND rec.column_default <> '' THEN
            lResult := lResult || ' DEFAULT ' || rec.column_default;
        END IF;
        IF i = lNumRec THEN 
            lResult := lResult || chr(10) || ');';
        ELSE 
            lResult := lResult || ', ' || chr(10);
        END IF;
    END LOOP;

    RETURN lResult;
END
$func$;

SELECT fGetSqlFromTable(@SchemaName, @TableName) AS sql;";
    }

    /// <inheritdoc/>
    public DbConnection CreateConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }

    /// <inheritdoc/>
    public bool DbExists()
    {
        if (string.IsNullOrEmpty(ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        if (_connection != null && _connection.ConnectionString == ConnectionString)
            return true;

        string? existingConnectionString = _connection?.ConnectionString;
        try
        {
            OpenDb();
            CloseDb();
        }
        catch (VelocipedeDbConnectParamsException)
        {
            throw;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            TryReconnect(existingConnectionString);
        }
        return true;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection CreateDb()
    {
        if (DbExists())
            throw new InvalidOperationException(ErrorMessageConstants.DatabaseAlreadyExists);

        try
        {
            string sql = $"CREATE DATABASE \"{DatabaseName}\"";
            return Execute(sql);
        }
        catch (VelocipedeDbConnectParamsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbCreateException(ex);
        }
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection OpenDb()
    {
        return OpenDb(ConnectionString);
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
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                ConnectionString = ConnectionString ?? "",
                Database = dbName
            };
            connectionString = connectionStringBuilder.ConnectionString;
            ConnectionString = connectionString;

            // Connect to the new database.
            return OpenDb();
        }
        catch (VelocipedeDbConnectParamsException)
        {
            CloseDb();
            throw;
        }
        catch (PostgresException ex)
        {
            CloseDb();
            throw new VelocipedeConnectionStringException(ex);
        }
        catch (ArgumentException ex)
        {
            CloseDb();
            throw new VelocipedeConnectionStringException(ex);
        }
        catch (Exception)
        {
            CloseDb();
            throw;
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
        out List<VelocipedeColumnInfo> columnInfo)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        TableAndSchemaInfo tableAndSchemaInfo = GetTableAndSchemaName(tableName);
        List<VelocipedeCommandParameter> parameters =
        [
            new() { Name = "TableName", Value = tableAndSchemaInfo.TableName },
            new() { Name = "SchemaName", Value = tableAndSchemaInfo.SchemaName }
        ];
        return Query(_getColumnsSql, parameters, out columnInfo);
    }

    /// <inheritdoc/>
    public Task<List<VelocipedeColumnInfo>> GetColumnsAsync(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        TableAndSchemaInfo tableAndSchemaInfo = GetTableAndSchemaName(tableName);
        List<VelocipedeCommandParameter> parameters =
        [
            new() { Name = "TableName", Value = tableAndSchemaInfo.TableName },
            new() { Name = "SchemaName", Value = tableAndSchemaInfo.SchemaName }
        ];
        return QueryAsync<VelocipedeColumnInfo>(_getColumnsSql, parameters);
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

        TableAndSchemaInfo tableAndSchemaInfo = GetTableAndSchemaName(tableName);
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableAndSchemaInfo.TableName }];
        return Query(_getForeignKeysSql, parameters, out foreignKeyInfo);
    }

    /// <inheritdoc/>
    public Task<List<VelocipedeForeignKeyInfo>> GetForeignKeysAsync(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        TableAndSchemaInfo tableAndSchemaInfo = GetTableAndSchemaName(tableName);
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableAndSchemaInfo.TableName }];
        return QueryAsync<VelocipedeForeignKeyInfo>(_getForeignKeysSql, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetTriggers(
        string tableName,
        out List<VelocipedeTriggerInfo> triggerInfo)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        TableAndSchemaInfo tableAndSchemaInfo = GetTableAndSchemaName(tableName);
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableAndSchemaInfo.TableName }];
        return Query(_getTriggersSql, parameters, out triggerInfo);
    }

    /// <inheritdoc/>
    public Task<List<VelocipedeTriggerInfo>> GetTriggersAsync(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        TableAndSchemaInfo tableAndSchemaInfo = GetTableAndSchemaName(tableName);
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableAndSchemaInfo.TableName }];
        return QueryAsync<VelocipedeTriggerInfo>(_getTriggersSql, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetSqlDefinition(
        string tableName,
        out string? sqlDefinition)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        TableAndSchemaInfo tableAndSchemaInfo = GetTableAndSchemaName(tableName);
        List<VelocipedeCommandParameter> parameters =
        [
            new() { Name = "TableName", Value = tableAndSchemaInfo.TableName },
            new() { Name = "SchemaName", Value = tableAndSchemaInfo.SchemaName }
        ];
        return QueryFirstOrDefault(_getSqlDefinitionSql, parameters, out sqlDefinition);
    }

    /// <inheritdoc/>
    public Task<string?> GetSqlDefinitionAsync(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        TableAndSchemaInfo tableAndSchemaInfo = GetTableAndSchemaName(tableName);
        List<VelocipedeCommandParameter> parameters =
        [
            new() { Name = "TableName", Value = tableAndSchemaInfo.TableName },
            new() { Name = "SchemaName", Value = tableAndSchemaInfo.SchemaName }
        ];
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
    public IVelocipedeDbConnection QueryDataTable(
        string sqlRequest,
        out DataTable dtResult)
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
        return Execute(sqlRequest, null);
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
        return Query(
            sqlRequest,
            parameters: null,
            result: out result);
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

    /// <summary>
    /// Get database name by connection string.
    /// </summary>
    /// <param name="connectionString">Connection string.</param>
    /// <returns>Database name.</returns>
    public static string? GetDatabaseName(string? connectionString)
    {
        try
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            return connectionStringBuilder.Database;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbNameException(ex);
        }
    }

    /// <summary>
    /// Get connection string by database name.
    /// </summary>
    /// <param name="connectionString">Old connection string.</param>
    /// <param name="databaseName">Database name.</param>
    /// <returns>New connection string.</returns>
    public static string GetConnectionString(string? connectionString, string databaseName)
    {
        try
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                ConnectionString = connectionString ?? "",
                Database = databaseName,
                PersistSecurityInfo = true
            };
            return connectionStringBuilder.ConnectionString;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbNameException(ex);
        }
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

    /// <inheritdoc/>
    public string GetPaginatedSql(string sqlRequest, VelocipedePaginationInfo paginationInfo)
    {
        if (string.IsNullOrEmpty(paginationInfo.OrderingFieldName))
        {
            throw new InvalidOperationException(ErrorMessageConstants.OrderingFieldNameCouldNotBeNullOrEmpty);
        }
        return paginationInfo.PaginationType == VelocipedePaginationType.KeysetById
            ? $@"SELECT t.* FROM ({sqlRequest}) t WHERE ""{paginationInfo.OrderingFieldName}"" > {paginationInfo.Offset} ORDER BY ""{paginationInfo.OrderingFieldName}"" LIMIT {paginationInfo.Limit}"
            : $@"SELECT t.* FROM ({sqlRequest}) t ORDER BY ""{paginationInfo.OrderingFieldName}"" LIMIT {paginationInfo.Limit} OFFSET {paginationInfo.Offset}";
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        CloseDb();
    }

    /// <summary>
    /// Open database with the specified connection string.
    /// </summary>
    /// <param name="connectionString">Connection string.</param>
    private PgDbConnection OpenDb(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        try
        {
            CloseDb();
            connectionString = UsePersistSecurityInfo(connectionString);
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            return this;
        }
        catch (ArgumentException ex)
        {
            throw new VelocipedeConnectionStringException(ex);
        }
    }

    /// <summary>
    /// Try to reconnect to the previous established connection.
    /// </summary>
    /// <param name="connectionString">Connection string.</param>
    /// <returns><c>true</c> if connected successfully; otherwise, <c>false</c>.</returns>
    private bool TryReconnect(string? connectionString)
    {
        try
        {
            OpenDb(connectionString);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Get connection string adding <see cref="NpgsqlConnectionStringBuilder.PersistSecurityInfo"/>.
    /// </summary>
    /// <param name="connectionString">Old connection string.</param>
    /// <returns>New connection string.</returns>
    private static string UsePersistSecurityInfo(string connectionString)
    {
        try
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                ConnectionString = connectionString,
                PersistSecurityInfo = true
            };
            return connectionStringBuilder.ConnectionString;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbNameException(ex);
        }
    }

    /// <summary>
    /// Parse original table name to separate it from schema name.
    /// </summary>
    /// <param name="tableName">Table name</param>
    /// <returns>Instance of <see cref="TableAndSchemaInfo"/>.</returns>
    private static TableAndSchemaInfo GetTableAndSchemaName(string tableName)
    {
        string schemaName = "public";
        string[] tn = tableName.Split('.');
        if (tn.Length >= 2)
        {
            schemaName = tn.First();
            tableName = tableName.Replace($"{schemaName}.", "");
        }
        tableName = tableName.Trim('"');
        return new TableAndSchemaInfo { TableName = tableName, SchemaName = schemaName };
    }
}
