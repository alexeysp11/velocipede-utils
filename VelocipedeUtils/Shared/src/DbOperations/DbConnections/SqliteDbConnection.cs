using System.Data;
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

        private string AbsolutePathToDb { get; set; }

        public SqliteDbConnection() : this(string.Empty)
        {
        }
        
        public SqliteDbConnection(string path)
        {
            try
            {
                SetPathToDb(path);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void SetPathToDb(string path)
        {
            if (!System.IO.File.Exists(path)) throw new System.Exception($"Database file '{path}' does not exists");
            
            AbsolutePathToDb = path;
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
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = AbsolutePathToDb;
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var selectCmd = connection.CreateCommand();
                    selectCmd.CommandText = sqlRequest;
                    using (var reader = selectCmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
            return table;
        }
    }
}
