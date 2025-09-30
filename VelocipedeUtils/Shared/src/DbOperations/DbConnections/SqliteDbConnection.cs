using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using VelocipedeUtils.Shared.DbOperations.Enums;
using System.Linq;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Class for using SQLite database
    /// </summary>
    public sealed class SqliteDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.SQLite;
        public string DatabaseName => GetDatabaseFilePath(ConnectionString);
        public bool IsConnected => _connection != null;

        private SqliteConnection _connection;

        public SqliteDbConnection(string connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(DatabaseName))
                throw new InvalidOperationException($"Database file path is not specified in connection string: {ConnectionString}");

            return File.Exists(DatabaseName);
        }

        public ICommonDbConnection CreateDb()
        {
            if (DbExists())
                throw new InvalidOperationException($"Database file already exists");

            File.Create(DatabaseName);
            return this;
        }

        public ICommonDbConnection OpenDb()
        {
            CloseDb();
            _connection = new SqliteConnection(ConnectionString);
            _connection.Open();
            return this;
        }

        public ICommonDbConnection CloseDb()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
            return this;
        }

        public ICommonDbConnection GetTablesInDb(out List<string> tables)
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

        public ICommonDbConnection GetColumns(string tableName, out DataTable dtResult)
        {
            string sql = $"PRAGMA table_info({tableName});";
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public ICommonDbConnection GetForeignKeys(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetTriggers(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetSqlDefinition(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection CreateTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection ClearTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Executes SQL string and returns DataTable.
        /// </summary>
        public ICommonDbConnection ExecuteSqlCommand(string sqlRequest, out DataTable dtResult)
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

        public ICommonDbConnection Query<T>(string sqlRequest, out List<T> result)
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

        /// <summary>
        /// Check if the specified database file path is valid.
        /// </summary>
        public static bool IsDatabaseFilePathValid(string path) => File.Exists(path);

        /// <summary>
        /// Get database file path by connection string.
        /// </summary>
        public static string GetDatabaseFilePath(string connectionString)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.ConnectionString = connectionString;
            return connectionStringBuilder.DataSource;
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
    }
}
