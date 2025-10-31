using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// MS SQL database connection.
    /// </summary>
    public sealed class MssqlDbConnection : IVelocipedeDbConnection
    {
        public string? ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.MSSQL;
        public string DatabaseName => GetDatabaseName(ConnectionString);
        public bool IsConnected => _connection != null;

        private SqlConnection? _connection;

        public MssqlDbConnection(string? connectionString = null)
        {
            ConnectionString = connectionString;
        }

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

        public IVelocipedeDbConnection OpenDb()
        {
            OpenDb(ConnectionString);
            return this;
        }

        /// <summary>
        /// Open database with the specified connection string.
        /// </summary>
        private IVelocipedeDbConnection OpenDb(string? connectionString)
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
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Try to reconnect to the previous established connection.
        /// </summary>
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
        /// Switch to the specified database and get new connection string.
        /// </summary>
        public IVelocipedeDbConnection SwitchDb(string? dbName, out string connectionString)
        {
            if (string.IsNullOrEmpty(dbName))
                throw new VelocipedeDbNameException();

            try
            {
                // Change connection string.
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = ConnectionString;
                connectionStringBuilder.InitialCatalog = dbName;
                connectionString = connectionStringBuilder.ConnectionString;
                ConnectionString = connectionString;

                // Connect to the new database.
                OpenDb();

                return this;
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

        public IVelocipedeDbConnection GetTablesInDb(out List<string> tables)
        {
            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            Query(sql, out tables);
            return this;
        }

        public IVelocipedeDbConnection GetColumns(string tableName, out List<VelocipedeColumnInfo> columnInfo)
        {
            tableName = tableName.Trim('"');
            string sql = $@"
SELECT
    COLUMN_NAME as ColumnName,
    DATA_TYPE as ColumnType,
    COLUMN_DEFAULT as DefaultValue,
    case when IS_NULLABLE = 'YES' then 1 else 0 end as IsNullable
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{tableName}';";
            Query(sql, out columnInfo);
            return this;
        }

        public IVelocipedeDbConnection GetForeignKeys(string tableName, out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
        {
            tableName = tableName.Trim('"');
            string sql = $@"
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
WHERE OBJECT_NAME(fk.parent_object_id) = '{tableName}'";
            Query(sql, out foreignKeyInfo);
            return this;
        }

        public IVelocipedeDbConnection GetTriggers(string tableName, out List<VelocipedeTriggerInfo> triggerInfo)
        {
            tableName = tableName.Trim('"');
            string sql = $@"
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
WHERE s.type = 'TR' and object_name(parent_obj) = '{tableName}'";
            Query(sql, out triggerInfo);
            return this;
        }

        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string? sqlDefinition)
        {
            tableName = tableName.Trim('"');
            string sql = $"SELECT OBJECT_DEFINITION(OBJECT_ID('{tableName}')) AS ObjectDefinition";
            QueryFirstOrDefault(sql, out sqlDefinition);
            return this;
        }

        public IVelocipedeDbConnection CreateTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection ClearTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection QueryDataTable(string sqlRequest, out DataTable dtResult)
        {
            return QueryDataTable(
                sqlRequest,
                parameters: null,
                dtResult: out dtResult);
        }

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

        public IVelocipedeDbConnection Execute(string sqlRequest)
        {
            return Execute(sqlRequest, null);
        }

        public IVelocipedeDbConnection Execute(string sqlRequest, List<VelocipedeCommandParameter>? parameters)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqlConnection? localConnection = null;
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
                    localConnection = new SqlConnection(ConnectionString);
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
                    localConnection = null;
                }
            }
            return this;
        }

        public IVelocipedeDbConnection Query<T>(string sqlRequest, out List<T> result)
        {
            return Query(
                sqlRequest,
                parameters: null,
                result: out result);
        }

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

        public IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate,
            out List<T> result)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqlConnection? localConnection = null;
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
                    localConnection = new SqlConnection(ConnectionString);
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
                    localConnection = null;
                }
            }
            return this;
        }

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(string sqlRequest, out T? result)
        {
            return QueryFirstOrDefault(
                sqlRequest,
                parameters: null,
                result: out result);
        }

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out T? result)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqlConnection? localConnection = null;
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
                    localConnection = new SqlConnection(ConnectionString);
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
                    localConnection = null;
                }
            }
            return this;
        }

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
        /// Get database name by connection string.
        /// </summary>
        public static string GetDatabaseName(string? connectionString)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
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
        public static string GetConnectionString(string? connectionString, string databaseName)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
                connectionStringBuilder.InitialCatalog = databaseName;
                return connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbNameException(ex);
            }
        }

        /// <summary>
        /// Get connection string by database name.
        /// </summary>
        private string UsePersistSecurityInfo(string connectionString)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
                connectionStringBuilder.PersistSecurityInfo = true;
                return connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbNameException(ex);
            }
        }

        public void Dispose()
        {
            CloseDb();
        }

        public IVelocipedeForeachTableIterator ForeachTable(List<string> tables)
        {
            return new VelocipedeForeachTableIterator(this, tables);
        }
    }
}
