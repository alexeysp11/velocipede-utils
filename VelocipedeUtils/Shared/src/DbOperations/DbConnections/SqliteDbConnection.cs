using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Class for using SQLite database
    /// </summary>
    public class SqliteDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.SQLite;

        /// <summary>
        /// Database file path.
        /// </summary>
        public string DatabaseFilePath => GetDatabaseFilePath(ConnectionString);

        public SqliteDbConnection(string connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public void CreateDb()
        {
            throw new System.NotImplementedException();
        }

        public void OpenDb()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayTablesInDb()
        {
            throw new System.NotImplementedException();
        }

        public void GetAllDataFromTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void GetColumnsOfTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void GetForeignKeys(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void GetTriggers(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void GetSqlDefinition(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void CreateTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void ClearTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Executes SQL string and returns DataTable
        /// </summary>
        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable();
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = sqlRequest;
                using (var reader = selectCmd.ExecuteReader())
                {
                    table.Load(reader);
                }
            }
            return table;
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
