using System.Collections.Generic;
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
        /// Database name.
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// Whether the instance is connected to the specified database.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Whether database specified in the connection string exists.
        /// </summary>
        bool DbExists();

        /// <summary>
        /// Create database.
        /// </summary>
        ICommonDbConnection CreateDb();

        /// <summary>
        /// Connect to the specified database.
        /// </summary>
        ICommonDbConnection OpenDb();

        /// <summary>
        /// Disconnect from the specified database.
        /// </summary>
        ICommonDbConnection CloseDb();

        /// <summary>
        /// Get tables in the current database.
        /// </summary>
        ICommonDbConnection GetTablesInDb(out List<string> tables);

        /// <summary>
        /// Get columns of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        ICommonDbConnection GetColumnsOfTable(string tableName);

        /// <summary>
        /// Get foreign keys of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        ICommonDbConnection GetForeignKeys(string tableName);

        /// <summary>
        /// Get triggers of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        ICommonDbConnection GetTriggers(string tableName);

        /// <summary>
        /// Get SQL definition of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        ICommonDbConnection GetSqlDefinition(string tableName);

        /// <summary>
        /// Create temporary table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        ICommonDbConnection CreateTemporaryTable(string tableName);

        /// <summary>
        /// Clear temporary table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        ICommonDbConnection ClearTemporaryTable(string tableName);

        /// <summary>
        /// Execute SQL command.
        /// </summary>
        ICommonDbConnection ExecuteSqlCommand(string sqlRequest, out DataTable dtResult);

        /// <summary>
        /// Query using Dapper.
        /// </summary>
        ICommonDbConnection Query<T>(string sqlRequest, out List<T> result);
    }
}
