namespace VelocipedeUtils.Shared.DbOperations.Enums
{
    /// <summary>
    /// Database type.
    /// </summary>
    public enum DatabaseType
    {
        None = 0,
        InMemory = 1,
        SQLite = 2,
        PostgreSQL = 3,
        MSSQL = 4,
        MySQL = 5,
        MariaDB = 6,
        HSQLDB = 7,
        Oracle = 8,
        Clickhouse = 9,
        Firebird = 10,
    }
}
