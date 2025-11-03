using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Interface for database connections.
    /// </summary>
    public interface IVelocipedeDbConnection : IDisposable
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        string? ConnectionString { get; set; }

        /// <summary>
        /// Database type.
        /// </summary>
        DatabaseType DatabaseType { get; }

        /// <summary>
        /// Database name.
        /// </summary>
        string? DatabaseName { get; }

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
        /// Switch to the specified database and get new connection string.
        /// </summary>
        IVelocipedeDbConnection SwitchDb(string? dbName, out string connectionString);

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
        IVelocipedeDbConnection GetColumns(string tableName, out List<VelocipedeColumnInfo> columnInfo);

        /// <summary>
        /// Get foreign keys of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection GetForeignKeys(string tableName, out List<VelocipedeForeignKeyInfo> foreignKeyInfo);

        /// <summary>
        /// Get triggers of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection GetTriggers(string tableName, out List<VelocipedeTriggerInfo> triggerInfo);

        /// <summary>
        /// Get SQL definition of the specified table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        IVelocipedeDbConnection GetSqlDefinition(string tableName, out string? sqlDefinition);

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
        /// Execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        IVelocipedeDbConnection QueryDataTable(string sqlRequest, out DataTable dtResult);

        /// <summary>
        /// Execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        IVelocipedeDbConnection QueryDataTable(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out DataTable dtResult);

        /// <summary>
        /// Execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        public IVelocipedeDbConnection QueryDataTable(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<dynamic, bool>? predicate,
            out DataTable dtResult);

        /// <summary>
        /// Execute SQL command.
        /// </summary>
        IVelocipedeDbConnection Execute(string sqlRequest);

        /// <summary>
        /// Execute SQL command.
        /// </summary>
        IVelocipedeDbConnection Execute(string sqlRequest, List<VelocipedeCommandParameter>? parameters);

        /// <summary>
        /// Query to get <see cref="List{T}"/>.
        /// </summary>
        IVelocipedeDbConnection Query<T>(string sqlRequest, out List<T> result);

        /// <summary>
        /// Query to get <see cref="List{T}"/>.
        /// </summary>
        IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out List<T> result);

        /// <summary>
        /// Query to get <see cref="List{T}"/>.
        /// </summary>
        public IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate,
            out List<T> result);

        /// <summary>
        /// Query first or default object.
        /// </summary>
        IVelocipedeDbConnection QueryFirstOrDefault<T>(string sqlRequest, out T? result);

        /// <summary>
        /// Query first or default object.
        /// </summary>
        IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out T? result);

        /// <summary>
        /// Query first or default object.
        /// </summary>
        IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate,
            out T? result);

        /// <summary>
        /// Initialize <c>foreach</c> operation for the specified tables.
        /// </summary>
        IVelocipedeForeachTableIterator WithForeachTableIterator(List<string> tables);
    }
}
