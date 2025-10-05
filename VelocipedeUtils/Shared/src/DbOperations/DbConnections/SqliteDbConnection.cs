using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using VelocipedeUtils.Shared.DbOperations.Enums;
using System.Linq;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Class for using SQLite database.
    /// </summary>
    public sealed class SqliteDbConnection : IVelocipedeDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.SQLite;
        public string DatabaseName => GetDatabaseName(ConnectionString);
        public bool IsConnected => _connection != null;

        private SqliteConnection _connection;

        public SqliteDbConnection(string connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            if (_connection != null)
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

        public IVelocipedeDbConnection CreateDb()
        {
            if (DbExists())
                throw new InvalidOperationException(ErrorMessageConstants.DatabaseAlreadyExists);

            try
            {
                string dbName = DatabaseName;
                using FileStream fs = File.Create(dbName);
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

        public IVelocipedeDbConnection OpenDb()
        {
            CloseDb();
            _connection = new SqliteConnection(ConnectionString);
            _connection.Open();
            return this;
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

        public IVelocipedeDbConnection GetColumns(string tableName, out DataTable dtResult)
        {
            string sql = $"PRAGMA table_info({tableName});";
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetForeignKeys(string tableName, out DataTable dtResult)
        {
            string sql = $"PRAGMA foreign_key_list('{tableName}');";
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetTriggers(string tableName, out DataTable dtResult)
        {
            string sql = $"SELECT * FROM sqlite_master WHERE type = 'trigger' AND tbl_name LIKE '{tableName}';";
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string sqlDefinition)
        {
            string sql = string.Format(@"SELECT sql FROM sqlite_master WHERE type='table' AND name LIKE '{0}'", tableName);
            return QueryFirstOrDefault(sql, out sqlDefinition);
        }

        public IVelocipedeDbConnection CreateTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection ClearTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Executes SQL string and returns DataTable.
        /// </summary>
        public IVelocipedeDbConnection ExecuteSqlCommand(string sqlRequest, out DataTable dtResult)
        {
            // Initialize connection.
            bool newConnectionUsed = true;
            SqliteConnection localConnection = null;
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
            dtResult = new DataTable();
            try
            {
                var selectCmd = localConnection.CreateCommand();
                selectCmd.CommandText = sqlRequest;
                using (var reader = selectCmd.ExecuteReader())
                {
                    dtResult.Load(reader);
                }
            }
            finally
            {
                if (newConnectionUsed)
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
            // Initialize connection.
            bool newConnectionUsed = true;
            SqliteConnection localConnection = null;
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
            try
            {
                result = localConnection.Query<T>(sqlRequest).ToList();
            }
            finally
            {
                if (newConnectionUsed)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                    localConnection = null;
                }
            }

            return this;
        }

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(string sqlRequest, out T result)
        {
            // Initialize connection.
            bool newConnectionUsed = true;
            SqliteConnection localConnection = null;
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
            try
            {
                result = localConnection.QueryFirstOrDefault<T>(sqlRequest);
            }
            finally
            {
                if (newConnectionUsed)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                    localConnection = null;
                }
            }

            return this;
        }

        /// <summary>
        /// Check if the specified database file path is valid.
        /// </summary>
        public static bool IsDatabaseFilePathValid(string path) => File.Exists(path);

        /// <summary>
        /// Get database file path by connection string.
        /// </summary>
        public static string GetDatabaseName(string connectionString)
        {
            try
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
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
        public static string GetConnectionString(string path)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = path;
            return connectionStringBuilder.ConnectionString;
        }

        public void Dispose()
        {
            CloseDb();
        }
    }
}
