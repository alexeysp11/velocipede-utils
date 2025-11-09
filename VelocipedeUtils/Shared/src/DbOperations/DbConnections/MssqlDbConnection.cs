using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections;

/// <summary>
/// MS SQL database connection.
/// </summary>
public sealed class MssqlDbConnection : BaseVelocipedeDbConnection, IVelocipedeDbConnection
{
    /// <inheritdoc/>
    public string? ConnectionString { get; set; }

    /// <inheritdoc/>
    public DatabaseType DatabaseType => DatabaseType.MSSQL;

    /// <inheritdoc/>
    public string DatabaseName => GetDatabaseName(ConnectionString);

    /// <inheritdoc/>
    public bool IsConnected => Connection != null;

    /// <inheritdoc/>
    public DbConnection? Connection => _connection;

    private SqlConnection? _connection;

    private readonly string _getTablesInDbSql;
    private readonly string _getColumnsSql;
    private readonly string _getForeignKeysSql;
    private readonly string _getTriggersSql;
    private readonly string _getSqlDefinitionSql;

    /// <summary>
    /// Creates an instance of <see cref="MssqlDbConnection"/> by connection string.
    /// </summary>
    /// <param name="connectionString">Specified connection string.</param>
    public MssqlDbConnection(string? connectionString = null)
    {
        ConnectionString = connectionString;

        _getTablesInDbSql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

        _getColumnsSql = @"
SELECT
    COLUMN_NAME as ColumnName,
    DATA_TYPE as ColumnType,
    COLUMN_DEFAULT as DefaultValue,
    case when IS_NULLABLE = 'YES' then 1 else 0 end as IsNullable
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = @TableName";

        _getForeignKeysSql = @"
SELECT
    fk.name AS ConstraintName,
    OBJECT_NAME(fk.parent_object_id) AS FromTableName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS FromColumn,
    OBJECT_NAME(fk.referenced_object_id) AS ToTableName,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ToColumn,
    fk.delete_referential_action_desc AS OnDelete,
    fk.update_referential_action_desc AS OnUpdate
FROM
    sys.foreign_keys AS fk
INNER JOIN
    sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
WHERE OBJECT_NAME(fk.parent_object_id) = @TableName";

        _getTriggersSql = @"
SELECT 
    name as [TriggerName],
    object_name(parent_obj) as [EventObjectTable],
    object_schema_name(parent_obj) as [EventObjectSchema],
    --OBJECTPROPERTY(id, 'ExecIsUpdateTrigger') AS isupdate, 
    --OBJECTPROPERTY(id, 'ExecIsDeleteTrigger') AS isdelete, 
    --OBJECTPROPERTY(id, 'ExecIsInsertTrigger') AS isinsert, 
    --OBJECTPROPERTY(id, 'ExecIsAfterTrigger') AS isafter, 
    --OBJECTPROPERTY(id, 'ExecIsInsteadOfTrigger') AS isinsteadof,
    case when OBJECTPROPERTY(id, 'ExecIsTriggerDisabled') = 1 then 0 else 1 end AS [IsActive]
FROM sysobjects s
WHERE s.type = 'TR' and object_name(parent_obj) = @TableName";

        _getSqlDefinitionSql = $"SELECT OBJECT_DEFINITION(OBJECT_ID(@TableName)) AS ObjectDefinition";
    }

    /// <inheritdoc/>
    public DbConnection CreateConnection(string connectionString)
    {
        return new SqlConnection(connectionString);
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

    /// <summary>
    /// Open database with the specified connection string.
    /// </summary>
    /// <param name="connectionString">Connection string.</param>
    /// <returns>Instance of <see cref="MssqlDbConnection"/>.</returns>
    private MssqlDbConnection OpenDb(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        try
        {
            if (IsConnected)
            {
                CloseDb();

                // TODO:
                // During the tests it turned out that MS SQL can throw an error
                // when there are a large number of repeated connections for the same user in a short period of time.
                // Therefore, from a performance and reliability perspective,
                // it's better to consider connecting to another DB within an existing connection if it's active.
                Thread.Sleep(5000);
            }
            connectionString = UsePersistSecurityInfo(connectionString);
            _connection = new SqlConnection(connectionString);
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

    /// <inheritdoc/>
    public IVelocipedeDbConnection SwitchDb(string? dbName, out string connectionString)
    {
        if (string.IsNullOrEmpty(dbName))
            throw new VelocipedeDbNameException();

        try
        {
            // Change connection string.
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                ConnectionString = ConnectionString ?? "",
                InitialCatalog = dbName
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
        catch (SqlException ex)
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
        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
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
        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return Query(_getColumnsSql, parameters, out columnInfo);
    }

    /// <inheritdoc/>
    public Task<List<VelocipedeColumnInfo>> GetColumnsAsync(string tableName)
    {
        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return QueryAsync<VelocipedeColumnInfo>(_getColumnsSql, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetForeignKeys(
        string tableName,
        out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
    {
        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return Query(_getForeignKeysSql, parameters, out foreignKeyInfo);
    }

    /// <inheritdoc/>
    public Task<List<VelocipedeForeignKeyInfo>> GetForeignKeysAsync(string tableName)
    {
        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return QueryAsync<VelocipedeForeignKeyInfo>(_getForeignKeysSql, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetTriggers(
        string tableName,
        out List<VelocipedeTriggerInfo> triggerInfo)
    {
        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return Query(_getTriggersSql, parameters, out triggerInfo);
    }

    /// <inheritdoc/>
    public Task<List<VelocipedeTriggerInfo>> GetTriggersAsync(string tableName)
    {
        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return QueryAsync<VelocipedeTriggerInfo>(_getTriggersSql, parameters);
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetSqlDefinition(
        string tableName,
        out string? sqlDefinition)
    {
        tableName = tableName.Trim('"');
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
        return QueryFirstOrDefault(_getSqlDefinitionSql, parameters, out sqlDefinition);
    }

    /// <inheritdoc/>
    public Task<string?> GetSqlDefinitionAsync(string tableName)
    {
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

    /// <summary>
    /// Get database name by connection string.
    /// </summary>
    /// <param name="connectionString">Connection string.</param>
    /// <returns>Database name.</returns>
    public static string GetDatabaseName(string? connectionString)
    {
        try
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            return connectionStringBuilder.InitialCatalog;
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
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                ConnectionString = connectionString ?? "",
                InitialCatalog = databaseName
            };
            return connectionStringBuilder.ConnectionString;
        }
        catch (Exception ex)
        {
            throw new VelocipedeDbNameException(ex);
        }
    }

    /// <summary>
    /// Get connection string adding <see cref="SqlConnectionStringBuilder.PersistSecurityInfo"/>.
    /// </summary>
    /// <param name="connectionString">Old connection string.</param>
    /// <returns>New connection string.</returns>
    private static string UsePersistSecurityInfo(string connectionString)
    {
        try
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
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
    public void Dispose()
    {
        CloseDb();
    }
}
