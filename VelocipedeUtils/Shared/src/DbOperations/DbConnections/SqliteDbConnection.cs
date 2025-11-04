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
            string sql = @"
SELECT name FROM sqlite_master
WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%'
UNION ALL
SELECT name FROM sqlite_temp_master
WHERE type IN ('table','view')
ORDER BY 1";
            Query(sql, out tables);
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetColumns(
            string tableName,
            out List<VelocipedeColumnInfo> columnInfo)
        {
            tableName = tableName.Trim('"');
            string sql = @$"
SELECT
    cid as ColumnId,
    name as ColumnName,
    type as ColumnType,
    dflt_value as DefaultValue,
    pk as IsPrimaryKey,
    not ""notnull"" as IsNullable
FROM pragma_table_info('{tableName}');";
            Query(sql, out columnInfo);
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetForeignKeys(
            string tableName,
            out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
        {
            tableName = tableName.Trim('"');

            // Get list of dynamic objects.
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
        public IVelocipedeDbConnection GetTriggers(string tableName, out List<VelocipedeTriggerInfo> triggerInfo)
        {
            tableName = tableName.Trim('"');
            string sql = $@"
SELECT
    --type,
    name AS TriggerName,
    tbl_name AS EventObjectTable,
    rootpage AS RootPage,
    sql AS SqlDefinition
FROM sqlite_master
WHERE type = 'trigger' AND tbl_name = '{tableName}';";
            Query(sql, out triggerInfo);
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string? sqlDefinition)
        {
            tableName = tableName.Trim('"');
            string sql = string.Format(@"SELECT sql FROM sqlite_master WHERE type='table' AND name LIKE '{0}'", tableName);
            return QueryFirstOrDefault(sql, out sqlDefinition);
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
        public IVelocipedeDbConnection Execute(string sqlRequest)
        {
            return Execute(sqlRequest, null);
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

        /// <inheritdoc/>
        public IVelocipedeForeachTableIterator WithForeachTableIterator(List<string> tables)
        {
            return new VelocipedeForeachTableIterator(this, tables);
        }
    }
}
