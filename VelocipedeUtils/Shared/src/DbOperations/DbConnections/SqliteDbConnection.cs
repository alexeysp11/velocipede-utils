using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using VelocipedeUtils.Shared.DbOperations.Enums;

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
        public bool IsConnected { get; private set; }

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
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection CloseDb()
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetTablesInDb()
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetAllDataFromTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetColumnsOfTable(string tableName)
        {
            throw new System.NotImplementedException();
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
        /// Executes SQL string and returns DataTable
        /// </summary>
        public ICommonDbConnection ExecuteSqlCommand(string sqlRequest, out DataTable dtResult)
        {
            dtResult = new DataTable();
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = sqlRequest;
                using (var reader = selectCmd.ExecuteReader())
                {
                    dtResult.Load(reader);
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
