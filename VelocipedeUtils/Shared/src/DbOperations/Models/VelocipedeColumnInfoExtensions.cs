using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Models;

/// <summary>
/// Extension methods for <see cref="VelocipedeColumnInfo"/>.
/// </summary>
public static class VelocipedeColumnInfoExtensions
{
    /// <summary>
    /// Get native type of the column.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Thrown if <see cref="VelocipedeColumnInfo.DatabaseType"/> is not supported.</exception>
    public static string GetNativeType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DatabaseType switch
        {
            DatabaseType.SQLite => columnInfo.GetSqliteNativeType(),
            DatabaseType.PostgreSQL => columnInfo.GetPostgresNativeType(),
            DatabaseType.MSSQL => columnInfo.GetMssqlNativeType(),
            _ => throw new NotSupportedException($"Unsupported database type: {columnInfo.DatabaseType}")
        };
    }

    /// <summary>
    /// Map <see cref="DbType"/> to SQLite alternative.
    /// </summary>
    /// <remarks>
    /// Note that numeric arguments in parentheses that following the type name (ex: <c>VARCHAR(255)</c>) are ignored by SQLite,
    /// because SQLite does not impose any length restrictions (other than the large global <c>SQLITE_MAX_LENGTH</c> limit) 
    /// on the length of strings, BLOBs or numeric values.
    /// </remarks>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for SQLite.</exception>
    public static string GetSqliteNativeType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DbType switch
        {
            DbType.Int32 => "INTEGER",
            DbType.String => "TEXT",
            DbType.DateTime => "DATETIME",
            DbType.Boolean => "INTEGER",
            DbType.Decimal => "REAL",
            _ => throw new NotSupportedException($"Unsupported DbType for SQLite: {columnInfo.DbType}")
        };
    }

    /// <summary>
    /// Map <see cref="DbType"/> to PostgreSQL alternative.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for PostgreSQL.</exception>
    public static string GetPostgresNativeType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DbType switch
        {
            DbType.Int32 => "INTEGER",
            DbType.String => (columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0)
                ? $"VARCHAR({columnInfo.CharMaxLength})"
                : "TEXT",
            DbType.DateTime => "TIMESTAMP",
            DbType.Boolean => "BOOLEAN",
            DbType.Decimal => (columnInfo.NumericPrecision.HasValue && columnInfo.NumericScale.HasValue)
                ? $"DECIMAL({columnInfo.NumericPrecision.Value}, {columnInfo.NumericScale.Value})"
                : "DECIMAL(18, 4)",
            _ => throw new NotSupportedException($"Unsupported DbType for PostgreSQL: {columnInfo.DbType}")
        };
    }

    /// <summary>
    /// Map <see cref="DbType"/> to SQL Server alternative.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for SQL Server.</exception>
    public static string GetMssqlNativeType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DbType switch
        {
            DbType.Int32 => "INT",
            DbType.String => (columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0)
                ? $"NVARCHAR({columnInfo.CharMaxLength})"
                : "NVARCHAR(MAX)",
            DbType.DateTime => "DATETIME2",
            DbType.Boolean => "BIT",
            DbType.Decimal => (columnInfo.NumericPrecision.HasValue && columnInfo.NumericScale.HasValue)
                ? $"DECIMAL({columnInfo.NumericPrecision.Value}, {columnInfo.NumericScale.Value})"
                : "DECIMAL(18, 4)",
            _ => throw new NotSupportedException($"Unsupported DbType for SQL Server: {columnInfo.DbType}")
        };
    }
}
