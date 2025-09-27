using System.Data;
using Microsoft.Data.Sqlite;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Class for using SQLite database
    /// </summary>
    public class SqliteDbConnection : BaseDbConnection, ICommonDbConnection 
    {
        private string ConnectionString { get; set; }
        private string AbsolutePathToDb { get; set; }

        public SqliteDbConnection() 
            : this(string.Empty)
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

        public ICommonDbConnection SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }

        public void SetPathToDb(string path)
        {
            if (!System.IO.File.Exists(path)) throw new System.Exception($"Database file '{path}' does not exists");
            
            AbsolutePathToDb = path;
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

        public new string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return base.GetSqlFromDataTable(dt, tableName);
        }
    }
}
