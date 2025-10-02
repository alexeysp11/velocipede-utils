using System.Collections.Generic;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Interface for database connections.
    /// </summary>
    public interface IVelocipedeDbConnection
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
        IVelocipedeDbConnection CreateDb();

        /// <summary>
        /// Connect to the specified database.
        /// </summary>
        IVelocipedeDbConnection OpenDb();

        /// <summary>
        /// Disconnect from the specified database.
        /// </summary>
        IVelocipedeDbConnection CloseDb();

        /// <summary>
        /// Get tables in the current database.
        /// </summary>
        IVelocipedeDbConnection GetTablesInDb(out List<string> tables);

        /// <summary>
        /// Get columns of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection GetColumns(string tableName, out DataTable dtResult);

        /// <summary>
        /// Get foreign keys of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection GetForeignKeys(string tableName, out DataTable dtResult);

        /// <summary>
        /// Get triggers of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection GetTriggers(string tableName, out DataTable dtResult);

        /// <summary>
        /// Get SQL definition of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection GetSqlDefinition(string tableName, out string sqlDefinition);

        /// <summary>
        /// Create temporary table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection CreateTemporaryTable(string tableName);

        /// <summary>
        /// Clear temporary table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection ClearTemporaryTable(string tableName);

        /// <summary>
        /// Execute SQL command.
        /// </summary>
        IVelocipedeDbConnection ExecuteSqlCommand(string sqlRequest, out DataTable dtResult);

        /// <summary>
        /// Query using Dapper.
        /// </summary>
        IVelocipedeDbConnection Query<T>(string sqlRequest, out List<T> result);

        /// <summary>
        /// Query first or default using Dapper.
        /// </summary>
        IVelocipedeDbConnection QueryFirstOrDefault<T>(string sqlRequest, out T result);
    }
}
