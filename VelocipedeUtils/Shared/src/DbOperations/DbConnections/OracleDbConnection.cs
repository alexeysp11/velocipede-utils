using System.Data;
using Oracle.ManagedDataAccess.Client;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Oracle database connection.
    /// </summary>
    public class OracleDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.Oracle;

        private string DataSource { get; set; }

        public OracleDbConnection() { }

        public OracleDbConnection(string dataSource)
        {
            DataSource = dataSource;
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

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable();
            using (OracleConnection con = new OracleConnection(string.IsNullOrEmpty(DataSource) ? ConnectionString : DataSource))
            {
                using (OracleCommand cmd = new OracleCommand(sqlRequest, con))
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        table.Load(dr);
                    }
                }
            }
            return table;
        }
    }
}
