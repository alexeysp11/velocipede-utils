using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.DbOperations.Iterators;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Class for using SQLite database.
    /// </summary>
    public sealed class SqliteDbConnection : IVelocipedeDbConnection
    {
        /// <inheritdoc/>
        public string? ConnectionString { get; set; }

        private readonly string _getTablesInDbSql;
        private readonly string _getColumnsSql;
        private readonly string _getTriggersSql;
        private readonly string _getSqlDefinitionSql;

        /// <inheritdoc/>
        public DatabaseType DatabaseType => DatabaseType.SQLite;

        /// <inheritdoc/>
        public string DatabaseName => GetDatabaseName(ConnectionString);

        /// <inheritdoc/>
        public bool IsConnected => _connection != null;

        private SqliteConnection? _connection;

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
    cid as ColumnId,
    name as ColumnName,
    type as ColumnType,
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
WHERE type = 'trigger' AND tbl_name = @TableName";

            _getSqlDefinitionSql = "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = @TableName";
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
            catch (Exception)
            {
                throw;
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
                OpenDb();

                return this;
            }
            catch (VelocipedeDbConnectParamsException)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
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

            // Get list of dynamic objects.
            // In some versions of SQLite, PRAGMA statements could not support paramters.
            string sql = $"PRAGMA foreign_key_list('{tableName}');";
            Query(sql, out List<dynamic> foreignKeyInfoDynamic);

            // Wrap the result.
            foreignKeyInfo = foreignKeyInfoDynamic
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

            return this;
        }

        /// <inheritdoc/>
        public async Task<List<VelocipedeForeignKeyInfo>> GetForeignKeysAsync(string tableName)
        {
            tableName = tableName.Trim('"');

            // Get list of dynamic objects.
            // In some versions of SQLite, PRAGMA statements could not support paramters.
            string sql = $"PRAGMA foreign_key_list('{tableName}');";
            List<dynamic> foreignKeyInfoDynamic = await QueryAsync<dynamic>(sql);

            // Wrap the result.
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

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetTriggers(string tableName, out List<VelocipedeTriggerInfo> triggerInfo)
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
        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string? sqlDefinition)
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
            return Execute(sqlRequest, parameters: null);
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection Execute(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqliteConnection? localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqliteConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                localConnection.Execute(sqlRequest, parameters?.ToDapperParameters());
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                }
            }
            return this;
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(string sqlRequest)
        {
            return ExecuteAsync(sqlRequest, parameters: null);
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            Task<DynamicParameters?>? dapperParamsTask = parameters?.ToDapperParametersAsync();
            SqliteConnection? localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqliteConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    await localConnection.OpenAsync();
                }

                // Execute SQL command and dispose connection if necessary.
                DynamicParameters? dynamicParameters = dapperParamsTask == null ? null : await dapperParamsTask;
                await localConnection.ExecuteAsync(sqlRequest, dynamicParameters);
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    await localConnection.CloseAsync();
                    localConnection.Dispose();
                }
            }
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
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqliteConnection? localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqliteConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                IEnumerable<T> queryResult = localConnection.Query<T>(sqlRequest, parameters?.ToDapperParameters());
                if (predicate != null)
                    queryResult = queryResult.Where(predicate);
                result = queryResult.ToList();
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                }
            }
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
        public async Task<List<T>> QueryAsync<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            Task<DynamicParameters?>? dapperParamsTask = parameters?.ToDapperParametersAsync();
            SqliteConnection? localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqliteConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    await localConnection.OpenAsync();
                }

                // Execute SQL command and dispose connection if necessary.
                DynamicParameters? dynamicParameters = dapperParamsTask == null ? null : await dapperParamsTask;
                IEnumerable<T> queryResult = await localConnection.QueryAsync<T>(sqlRequest, dynamicParameters);
                if (predicate != null)
                    queryResult = queryResult.Where(predicate);
                return queryResult.ToList();
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    await localConnection.CloseAsync();
                    localConnection.Dispose();
                }
            }
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
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqliteConnection? localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqliteConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                result = localConnection.QueryFirstOrDefault<T>(sqlRequest, parameters?.ToDapperParameters());
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                }
            }
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
        public async Task<T?> QueryFirstOrDefaultAsync<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            Task<DynamicParameters?>? dapperParamsTask = parameters?.ToDapperParametersAsync();
            SqliteConnection? localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqliteConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    await localConnection.OpenAsync();
                }

                // Execute SQL command and dispose connection if necessary.
                DynamicParameters? dynamicParameters = dapperParamsTask == null ? null : await dapperParamsTask;
                return await localConnection.QueryFirstOrDefaultAsync<T>(sqlRequest, dynamicParameters);
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    await localConnection.CloseAsync();
                    localConnection.Dispose();
                }
            }
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
        public IVelocipedeForeachTableIterator WithForeachTableIterator(List<string> tables)
        {
            return new VelocipedeForeachTableIterator(this, tables);
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
        public void Dispose()
        {
            CloseDb();
        }
    }
}
