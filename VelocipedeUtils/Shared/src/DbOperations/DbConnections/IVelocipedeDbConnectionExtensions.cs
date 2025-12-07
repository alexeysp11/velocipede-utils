using System.Data;
using System.Text;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections;

/// <summary>
/// Extension methods for <see cref="IVelocipedeDbConnection"/>.
/// </summary>
public static class IVelocipedeDbConnectionExtensions
{
    /// <summary>
    /// Get connection string by database name.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="databaseName">Database name.</param>
    /// <param name="connectionString">Resulting connection string.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection GetConnectionString(
        this IVelocipedeDbConnection connection,
        string databaseName,
        out string connectionString)
    {
        connectionString = connection.DatabaseType switch
        {
            VelocipedeDatabaseType.SQLite => SqliteDbConnection.GetConnectionString(databaseName),
            VelocipedeDatabaseType.PostgreSQL => PgDbConnection.GetConnectionString(connection.ConnectionString, databaseName),
            VelocipedeDatabaseType.MSSQL => MssqlDbConnection.GetConnectionString(connection.ConnectionString, databaseName),
            VelocipedeDatabaseType.MySQL or VelocipedeDatabaseType.MariaDB or VelocipedeDatabaseType.HSQLDB or VelocipedeDatabaseType.Oracle => throw new NotSupportedException(ErrorMessageConstants.DatabaseTypeIsNotSupported),
            _ => throw new InvalidOperationException(ErrorMessageConstants.IncorrectDatabaseType),
        };
        return connection;
    }

    /// <summary>
    /// Set connection string.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="connectionString">Connection string to be set.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection SetConnectionString(
        this IVelocipedeDbConnection connection,
        string? connectionString)
    {
        connection.ConnectionString = connectionString;
        return connection;
    }

    /// <summary>
    /// Get database name from the active connection string.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static string? GetActiveDatabaseName(
        this IVelocipedeDbConnection connection)
    {
        connection.GetActiveDatabaseName(out string? dbName);
        return dbName;
    }

    /// <summary>
    /// Get database name from the active connection string.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="dbName">Resulting database name.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection GetActiveDatabaseName(
        this IVelocipedeDbConnection connection,
        out string? dbName)
    {
        dbName = connection.DatabaseType switch
        {
            VelocipedeDatabaseType.SQLite => SqliteDbConnection.GetDatabaseName(connection.ConnectionString),
            VelocipedeDatabaseType.PostgreSQL => PgDbConnection.GetDatabaseName(connection.ConnectionString),
            VelocipedeDatabaseType.MSSQL => MssqlDbConnection.GetDatabaseName(connection.ConnectionString),
            VelocipedeDatabaseType.MySQL or VelocipedeDatabaseType.MariaDB or VelocipedeDatabaseType.HSQLDB or VelocipedeDatabaseType.Oracle => throw new NotSupportedException(ErrorMessageConstants.DatabaseTypeIsNotSupported),
            _ => throw new InvalidOperationException(ErrorMessageConstants.IncorrectDatabaseType),
        };
        return connection;
    }

    /// <summary>
    /// Create database if not exists.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection CreateDbIfNotExists(
        this IVelocipedeDbConnection connection)
    {
        if (!connection.DbExists())
        {
            connection.CreateDb();
        }
        return connection;
    }

    /// <summary>
    /// Create the specified database.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="dbName">Database name.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection CreateDb(
        this IVelocipedeDbConnection connection,
        string dbName)
    {
        return connection
            .OpenDbIfNotConnected()
            .GetConnectionString(dbName, out string newConnectionString)
            .SetConnectionString(newConnectionString)
            .CreateDb();
    }

    /// <summary>
    /// Preserve already existing connection, and open database only if necessary.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection OpenDbIfNotConnected(
        this IVelocipedeDbConnection connection)
    {
        if (!connection.IsConnected)
            connection.OpenDb();
        return connection;
    }

    /// <summary>
    /// Switch to the specified database.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="dbName">Database name.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection SwitchDb(
        this IVelocipedeDbConnection connection,
        string dbName)
    {
        return connection.SwitchDb(dbName, out _);
    }

    /// <summary>
    /// Get all data from the table.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="tableName">Table name.</param>
    /// <param name="dtResult">Resulting object of <see cref="DataTable"/>.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection GetAllData(
        this IVelocipedeDbConnection connection,
        string tableName,
        out DataTable dtResult)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        string sql = $"SELECT * FROM {tableName}";
        return connection.QueryDataTable(sql, out dtResult);
    }

    /// <summary>
    /// Get all data from the table.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="tableName">Table name.</param>
    /// <param name="result">Resulting <see cref="List{T}"/>.</param>
    /// <returns>The current <see cref="IVelocipedeDbConnection"/> instance, allowing for further configuration.</returns>
    public static IVelocipedeDbConnection GetAllData<T>(
        this IVelocipedeDbConnection connection,
        string tableName,
        out List<T> result)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        string sql = $"SELECT * FROM {tableName}";
        return connection.Query(sql, out result);
    }

    /// <summary>
    /// Get all data from the table.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="tableName">Table name.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result is an object of <see cref="DataTable"/>.
    /// </returns>
    public static Task<DataTable> GetAllDataAsync(
        this IVelocipedeDbConnection connection,
        string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        string sql = $"SELECT * FROM {tableName}";
        return connection.QueryDataTableAsync(sql);
    }

    /// <summary>
    /// Get all data from the table.
    /// </summary>
    /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="tableName">Table name.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result is a <see cref="List{T}"/> that contains the result of the executed query.
    /// </returns>
    public static Task<List<T>> GetAllDataAsync<T>(
        this IVelocipedeDbConnection connection,
        string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(tableName, ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
        }

        string sql = $"SELECT * FROM {tableName}";
        return connection.QueryAsync<T>(sql);
    }

    /// <summary>
    /// Gets SQL definition of the specified <see cref="DataTable"/> object.
    /// </summary>
    /// <param name="connection">Valid instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="dt"><see cref="DataTable"/> object which is used to construct SQL query.</param>
    /// <param name="tableName">Expected name of a created table.</param>
    /// <returns>SQL for creating a database table that has the same structure as the specified <see cref="DataTable"/> object.</returns>
    public static string GetSqlFromDataTable(
#pragma warning disable IDE0060 // Remove unused parameter
        this IVelocipedeDbConnection connection,
#pragma warning restore IDE0060 // Remove unused parameter
        DataTable? dt,
        string tableName)
    {
        if (dt == null)
            throw new ArgumentNullException(nameof(dt), "Data table could not be null");
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentNullException(nameof(tableName), "Table name is not assigned");
        if (dt.Columns.Count == 0)
            throw new ArgumentException("Data table could not be empty");

        var sbSqlRequest = new StringBuilder();
        var sbSqlInsert = new StringBuilder();

        int i = 0;
        sbSqlRequest.Append("CREATE TABLE IF NOT EXISTS " + tableName + " (");
        sbSqlInsert.Append("INSERT INTO " + tableName + " (");
        foreach (DataColumn column in dt.Columns)
        {
            sbSqlRequest.Append(column.ColumnName + " TEXT" + (i != dt.Columns.Count - 1 ? "," : ");\r\n"));
            sbSqlInsert.Append(column.ColumnName + (i != dt.Columns.Count - 1 ? "," : ") VALUES ("));
            i += 1;
        }
        foreach (DataRow row in dt.Rows)
        {
            i = 0;
            sbSqlRequest.Append(sbSqlInsert.ToString());
            foreach(DataColumn column in dt.Columns)
            {
                sbSqlRequest.Append("'" + row[column].ToString() + "'" + (i != dt.Columns.Count - 1 ? "," : ");\r\n"));
                i += 1;
            }
        }
        return sbSqlRequest.ToString();
    }
}
