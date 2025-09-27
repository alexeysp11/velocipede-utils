using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Oracle database connection.
    /// </summary>
    public class OracleDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        private string DataSource { get; set; }
        
        public OracleDbConnection() { }

        public OracleDbConnection(string dataSource)
        {
            DataSource = dataSource;
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

        public new string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return GetSqlFromDataTable(dt, tableName);
        }
    }
}
