using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Factories;

/// <summary>
/// Factory for creating database connections.
/// </summary>
public static class VelocipedeDbConnectionFactory
{
    /// <summary>
    /// Initialize database connection.
    /// </summary>
    /// <param name="databaseType">Database type (currently, accepts only <see cref="VelocipedeDatabaseType.SQLite"/>, <see cref="VelocipedeDatabaseType.PostgreSQL"/> and <see cref="VelocipedeDatabaseType.MSSQL"/>).</param>
    /// <param name="connectionString">Specified connection string.</param>
    /// <returns>A new instance of <see cref="IVelocipedeDbConnection"/>.</returns>
    public static IVelocipedeDbConnection InitializeDbConnection(VelocipedeDatabaseType databaseType, string? connectionString = null)
    {
        return databaseType switch
        {
            VelocipedeDatabaseType.SQLite => new SqliteDbConnection(connectionString),
            VelocipedeDatabaseType.PostgreSQL => new PgDbConnection(connectionString),
            VelocipedeDatabaseType.MSSQL => new MssqlDbConnection(connectionString),
            VelocipedeDatabaseType.MySQL => throw new NotImplementedException("This class is corrently excluded due to .NET MySQL errors \"The given key was not present in the dictionary\""),
            VelocipedeDatabaseType.Oracle => throw new NotImplementedException("The Oracle provider is not implemented yet"),
            _ => throw new ArgumentException("Specified database type is not valid", nameof(databaseType))
        };
    }
}
