using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;

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
        /// Database type.
        /// </summary>
        DatabaseType DatabaseType { get; }

        /// <summary>
        /// Whether database specified in the connection string exists.
        /// </summary>
        bool DbExists();

        /// <summary>
        /// Create database.
        /// </summary>
        void CreateDb();

        /// <summary>
        /// Open the specified database.
        /// </summary>
        void OpenDb();

        /// <summary>
        /// Display tables in the database.
        /// </summary>
        void DisplayTablesInDb();

        /// <summary>
        /// Get all data from the table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void GetAllDataFromTable(string tableName);

        /// <summary>
        /// Get columns of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void GetColumnsOfTable(string tableName);

        /// <summary>
        /// Get foreign keys of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void GetForeignKeys(string tableName);

        /// <summary>
        /// Get triggers of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void GetTriggers(string tableName);

        /// <summary>
        /// Get SQL definition of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void GetSqlDefinition(string tableName);

        /// <summary>
        /// Create temporary table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void CreateTemporaryTable(string tableName);

        /// <summary>
        /// Clear temporary table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void ClearTemporaryTable(string tableName);

        /// <summary>
        /// Execute SQL command.
        /// </summary>
        DataTable ExecuteSqlCommand(string sqlRequest);
    }
}
