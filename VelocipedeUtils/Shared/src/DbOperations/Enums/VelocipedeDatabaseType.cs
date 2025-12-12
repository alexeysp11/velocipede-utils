namespace VelocipedeUtils.Shared.DbOperations.Enums;

/// <summary>
/// Database type.
/// </summary>
public enum VelocipedeDatabaseType
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
    /// SQLite.
    /// </summary>
    SQLite = 2,

    /// <summary>
    /// PostgreSQL.
    /// </summary>
    PostgreSQL = 3,

    /// <summary>
    /// MS SQL Server.
    /// </summary>
    MSSQL = 4,

    /// <summary>
    /// MySQL.
    /// </summary>
    MySQL = 5,

    /// <summary>
    /// MariaDB.
    /// </summary>
    MariaDB = 6,

    /// <summary>
    /// HSQLDB.
    /// </summary>
    HSQLDB = 7,

    /// <summary>
    /// Oracle.
    /// </summary>
    Oracle = 8,

    /// <summary>
    /// Clickhouse.
    /// </summary>
    Clickhouse = 9,

    /// <summary>
    /// Firebird.
    /// </summary>
    Firebird = 10,
}
