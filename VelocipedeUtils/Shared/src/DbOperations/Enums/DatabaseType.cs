namespace VelocipedeUtils.Shared.DbOperations.Enums;

/// <summary>
/// Database type.
/// </summary>
public enum DatabaseType
{
    /// <summary>
    /// Undefined database.
    /// </summary>
    None = 0,

    /// <summary>
    /// In-memory database.
    /// </summary>
    InMemory = 1,

    /// <summary>
    /// SQLite database.
    /// </summary>
    SQLite = 2,

    /// <summary>
    /// PostgreSQL database.
    /// </summary>
    PostgreSQL = 3,

    /// <summary>
    /// MS SQL database.
    /// </summary>
    MSSQL = 4,

    /// <summary>
    /// MySQL database.
    /// </summary>
    MySQL = 5,

    /// <summary>
    /// MariaDB database.
    /// </summary>
    MariaDB = 6,

    /// <summary>
    /// HSQLDB database.
    /// </summary>
    HSQLDB = 7,

    /// <summary>
    /// Oracle database.
    /// </summary>
    Oracle = 8,

    /// <summary>
    /// Clickhouse database.
    /// </summary>
    Clickhouse = 9,

    /// <summary>
    /// Firebird database.
    /// </summary>
    Firebird = 10,
}
