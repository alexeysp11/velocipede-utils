using System.Data;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Interface for database connections.
    /// </summary>
    public interface ICommonDbConnection
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Execute SQL command.
        /// </summary>
        DataTable ExecuteSqlCommand(string sqlRequest);
        
        /// <summary>
        /// Gets SQL definition of the DataTable object.
        /// </summary>
        string GetSqlFromDataTable(DataTable dt, string tableName);
    }
}
