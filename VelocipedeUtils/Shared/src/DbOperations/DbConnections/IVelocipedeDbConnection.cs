using System.Data;
using System.Data.Common;
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
        /// <remarks>By default, equals to <c>null</c>.</remarks>
        string? ConnectionString { get; set; }

        /// <summary>
        /// Database type.
        /// </summary>
        /// <remarks>By default, equals to <see cref="DatabaseType.None"/>.</remarks>
        DatabaseType DatabaseType { get; }

        /// <summary>
        /// Database name.
        /// </summary>
        /// <remarks>By default, equals to <c>null</c>.</remarks>
        string? DatabaseName { get; }

        /// <summary>
        /// Whether the instance is connected to the specified database.
        /// </summary>
        /// <remarks>By default, equals to <c>false</c>.</remarks>
        bool IsConnected { get; }

        /// <summary>
        /// Current connection to database.
        /// </summary>
        /// <remarks>By default, equals to <c>null</c>.</remarks>
        DbConnection? Connection { get; }

        /// <summary>
        /// Create a new connection to database.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns>Instance of <see cref="IDbConnection"/>.</returns>
        DbConnection CreateConnection(
            string connectionString);

        /// <summary>
        /// Whether database, specified in the <see cref="ConnectionString"/> property, exists.
        /// </summary>
        /// <returns><c>true</c> database exists; otherwise, <c>false</c>.</returns>
        bool DbExists();

        /// <summary>
        /// Create database.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection CreateDb();

        /// <summary>
        /// Connect to the specified database.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection OpenDb();

        /// <summary>
        /// Switch to the specified database and get new connection string.
        /// </summary>
        /// <param name="dbName">Database name.</param>
        /// <param name="connectionString">Resulting connection string.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection SwitchDb(
            string? dbName,
            out string connectionString);

        /// <summary>
        /// Disconnect from the specified database.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection CloseDb();

        /// <summary>
        /// Get tables in the current database.
        /// </summary>
        /// <param name="tables">Resulting <see cref="List{T}"/> of <see cref="string"/> that contains table names.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection GetTablesInDb(
            out List<string> tables);

        /// <summary>
        /// Asynchronously get tables in the current database.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is a <see cref="List{T}"/> of <see cref="string"/> that contains table names.
        /// </returns>
        Task<List<string>> GetTablesInDbAsync();

        /// <summary>
        /// Get columns of the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="columnInfo">Resulting <see cref="List{T}"/> of <see cref="VelocipedeColumnInfo"/> that contains info about table columns.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection GetColumns(
            string tableName,
            out List<VelocipedeColumnInfo> columnInfo);

        /// <summary>
        /// Asynchronously get columns of the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is a <see cref="List{T}"/> of <see cref="VelocipedeColumnInfo"/> that contains info about table columns.
        /// </returns>
        Task<List<VelocipedeColumnInfo>> GetColumnsAsync(
            string tableName);

        /// <summary>
        /// Get foreign keys of the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="foreignKeyInfo">Resulting <see cref="List{T}"/> of <see cref="VelocipedeForeignKeyInfo"/> that contains info about table foreign keys.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection GetForeignKeys(
            string tableName,
            out List<VelocipedeForeignKeyInfo> foreignKeyInfo);

        /// <summary>
        /// Asynchronously get foreign keys of the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is a <see cref="List{T}"/> of <see cref="VelocipedeForeignKeyInfo"/> that contains info about table foreign keys.
        /// </returns>
        Task<List<VelocipedeForeignKeyInfo>> GetForeignKeysAsync(
            string tableName);

        /// <summary>
        /// Get triggers of the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="triggerInfo">Resulting <see cref="List{T}"/> of <see cref="VelocipedeTriggerInfo"/> that contains info about table triggers.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection GetTriggers(
            string tableName,
            out List<VelocipedeTriggerInfo> triggerInfo);

        /// <summary>
        /// Asynchronously get triggers of the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is a <see cref="List{T}"/> of <see cref="VelocipedeTriggerInfo"/> that contains info about table triggers.
        /// </returns>
        Task<List<VelocipedeTriggerInfo>> GetTriggersAsync(
            string tableName);

        /// <summary>
        /// Get SQL definition of the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="sqlDefinition">Resulting SQL definition of the specified table.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection GetSqlDefinition(
            string tableName,
            out string? sqlDefinition);

        /// <summary>
        /// Asynchronously get SQL definition of the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is a <see cref="string"/> that contains SQL definition of the specified table.
        /// </returns>
        Task<string?> GetSqlDefinitionAsync(
            string tableName);

        /// <summary>
        /// Create temporary table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection CreateTemporaryTable(
            string tableName);

        /// <summary>
        /// Clear temporary table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection ClearTemporaryTable(
            string tableName);

        /// <summary>
        /// Execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="dtResult">Resulting object of <see cref="DataTable"/>.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection QueryDataTable(
            string sqlRequest,
            out DataTable dtResult);

        /// <summary>
        /// Execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="dtResult">Resulting object of <see cref="DataTable"/>.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection QueryDataTable(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out DataTable dtResult);

        /// <summary>
        /// Execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="predicate">Predicate that is executed strictly after the data has already been retrieved from the database.</param>
        /// <param name="dtResult">Resulting object of <see cref="DataTable"/>.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection QueryDataTable(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<dynamic, bool>? predicate,
            out DataTable dtResult);

        /// <summary>
        /// Asynchronously execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        /// <param name="sqlRequest">SQL query.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is an object of <see cref="DataTable"/>.
        /// </returns>
        Task<DataTable> QueryDataTableAsync(
            string sqlRequest);

        /// <summary>
        /// Asynchronously execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is an object of <see cref="DataTable"/>.
        /// </returns>
        Task<DataTable> QueryDataTableAsync(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters);

        /// <summary>
        /// Asynchronously execute SQL command and get <see cref="DataTable"/> object as a result.
        /// </summary>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="predicate">Predicate that is executed strictly after the data has already been retrieved from the database.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is an object of <see cref="DataTable"/>.
        /// </returns>
        Task<DataTable> QueryDataTableAsync(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<dynamic, bool>? predicate);

        /// <summary>
        /// Execute SQL command.
        /// </summary>
        /// <param name="sqlRequest">SQL query to execute.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection Execute(
            string sqlRequest);

        /// <summary>
        /// Execute SQL command.
        /// </summary>
        /// <param name="sqlRequest">SQL query to execute.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection Execute(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters);

        /// <summary>
        /// Asynchronously execute SQL command.
        /// </summary>
        /// <param name="sqlRequest">SQL query.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task ExecuteAsync(
            string sqlRequest);

        /// <summary>
        /// Asynchronously execute SQL command.
        /// </summary>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task ExecuteAsync(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters);

        /// <summary>
        /// Query to get <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="result"><see cref="List{T}"/> that contains the result of the executed query.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            out List<T> result);

        /// <summary>
        /// Query to get <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="result"><see cref="List{T}"/> that contains the result of the executed query.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out List<T> result);

        /// <summary>
        /// Query to get <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="predicate">Predicate that is executed strictly after the data has already been retrieved from the database.</param>
        /// <param name="result"><see cref="List{T}"/> that contains the result of the executed query.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate,
            out List<T> result);

        /// <summary>
        /// Asynchronously query to get <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is a <see cref="List{T}"/> that contains the result of the executed query.
        /// </returns>
        Task<List<T>> QueryAsync<T>(
            string sqlRequest);

        /// <summary>
        /// Asynchronously query to get <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is a <see cref="List{T}"/> that contains the result of the executed query.
        /// </returns>
        Task<List<T>> QueryAsync<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters);

        /// <summary>
        /// Asynchronously query to get <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="predicate">Predicate that is executed strictly after the data has already been retrieved from the database.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The task result is a <see cref="List{T}"/> that contains the result of the executed query.
        /// </returns>
        Task<List<T>> QueryAsync<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate);

        /// <summary>
        /// Query first or default object.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="result">Result of the query.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            out T? result);

        /// <summary>
        /// Query first or default object.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="result">Result of the query.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out T? result);

        /// <summary>
        /// Query first or default object.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="predicate">Predicate that is executed strictly after the data has already been retrieved from the database.</param>
        /// <param name="result">Result of the query.</param>
        /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
        IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate,
            out T? result);

        /// <summary>
        /// Asynchronously query first or default object.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation that contains the result of the executed query.
        /// </returns>
        Task<T?> QueryFirstOrDefaultAsync<T>(
            string sqlRequest);

        /// <summary>
        /// Asynchronously query first or default object.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation that contains the result of the executed query.
        /// </returns>
        Task<T?> QueryFirstOrDefaultAsync<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters);

        /// <summary>
        /// Asynchronously query first or default object.
        /// </summary>
        /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
        /// <param name="sqlRequest">SQL query.</param>
        /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
        /// <param name="predicate">Predicate that is executed strictly after the data has already been retrieved from the database.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation that contains the result of the executed query.
        /// </returns>
        Task<T?> QueryFirstOrDefaultAsync<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate);

        /// <summary>
        /// Initialize <c>foreach</c> operation for the specified tables.
        /// </summary>
        /// <param name="tables">Table names.</param>
        /// <returns>The created <see cref="IVelocipedeForeachTableIterator"/> instance for performing <c>foreach</c> operations.</returns>
        IVelocipedeForeachTableIterator WithForeachTableIterator(
            List<string> tables);
    }
}
