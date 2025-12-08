using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Models.Metadata;

/// <summary>
/// Extension methods for <see cref="VelocipedeColumnInfo"/>.
/// </summary>
public static class VelocipedeColumnInfoExtensions
{
    /// <summary>
    /// Get native type of the column, getting <see cref="VelocipedeColumnInfo.NativeColumnType"/> by <see cref="VelocipedeColumnInfo.DbType"/>.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Thrown if <see cref="VelocipedeColumnInfo.DatabaseType"/> is not supported.</exception>
    public static string GetNativeType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DatabaseType switch
        {
            VelocipedeDatabaseType.SQLite => columnInfo.GetSqliteNativeType(),
            VelocipedeDatabaseType.PostgreSQL => columnInfo.GetPostgresNativeType(),
            VelocipedeDatabaseType.MSSQL => columnInfo.GetMssqlNativeType(),
            _ => throw new NotSupportedException($"Unsupported database type: {columnInfo.DatabaseType}")
        };
    }

    /// <summary>
    /// Map <see cref="DbType"/> to SQLite native type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that SQLite uses a more general dynamic type system, which makes it backwards compatible
    /// with the more common static type systems of other database engines (e.g. PostgreSQL, MS SQL, etc).
    /// All data types specified by the user are converted to one of the following types: 
    /// <c>NULL</c>, <c>INTEGER</c>, <c>REAL</c>, <c>TEXT</c>, <c>BLOB</c>.
    /// </para>
    /// <para>
    /// So numeric arguments in parentheses that following the type name (ex: <c>VARCHAR(255)</c>) are ignored by SQLite under the hood,
    /// because SQLite does not impose any length restrictions (other than the large global <c>SQLITE_MAX_LENGTH</c> limit) 
    /// on the length of strings, BLOBs or numeric values.
    /// </para>
    /// <para>
    /// As of version 3.37.0 (2021-11-27), SQLite provides <c>STRICT</c> tables that do rigid type enforcement.
    /// </para>
    /// <para>
    /// However, if you need to transfer data from one database to another, the dynamic type system can ensure type compatibility.
    /// This method returns types taking into account the possible transfer of data from one database to another.
    /// </para>
    /// </remarks>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for SQLite.</exception>
    public static string GetSqliteNativeType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DbType switch
        {
            DbType.SByte or DbType.Byte => "tinyint",
            DbType.Int16 or DbType.UInt16 => "smallint",
            DbType.Int32 or DbType.UInt32 => "integer",
            DbType.Int64 or DbType.UInt64 => "bigint",
            DbType.String or DbType.AnsiString or DbType.AnsiStringFixedLength
                => columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0
                    ? $"varchar({columnInfo.CharMaxLength})"
                    : "text",
            DbType.DateTime => "datetime",
            DbType.Boolean => "boolean",
            DbType.Double => "real",
            DbType.VarNumeric or DbType.Decimal => "numeric",
            _ => throw new NotSupportedException($"Unsupported DbType for SQLite: {columnInfo.DbType}")
        };
    }

    /// <summary>
    /// Map <see cref="DbType"/> to PostgreSQL native type.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for PostgreSQL.</exception>
    public static string GetPostgresNativeType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DbType switch
        {
            DbType.SByte or DbType.Byte or DbType.Int16 or DbType.UInt16 => "smallint",
            DbType.Int32 or DbType.UInt32 => "integer",
            DbType.Int64 or DbType.UInt64 => "bigint",
            DbType.String or DbType.AnsiString => columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0
                ? $"varchar({columnInfo.CharMaxLength})"
                : "text",
            DbType.DateTime => "timestamp",
            DbType.Boolean => "boolean",
            DbType.Decimal => columnInfo.NumericPrecision.HasValue && columnInfo.NumericScale.HasValue
                ? $"decimal({columnInfo.NumericPrecision.Value}, {columnInfo.NumericScale.Value})"
                : "decimal(18, 4)",
            _ => throw new NotSupportedException($"Unsupported DbType for PostgreSQL: {columnInfo.DbType}")
        };
    }

    /// <summary>
    /// Map <see cref="DbType"/> to SQL Server native type.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for SQL Server.</exception>
    public static string GetMssqlNativeType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DbType switch
        {
            DbType.SByte or DbType.Byte => "tinyint",
            DbType.Int16 or DbType.UInt16 => "smallint",
            DbType.Int32 or DbType.UInt32 => "int",
            DbType.Int64 or DbType.UInt64 => "bigint",
            DbType.String => columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0
                ? $"nvarchar({columnInfo.CharMaxLength})"
                : "nvarchar(max)",
            DbType.AnsiString => columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0
                ? $"varchar({columnInfo.CharMaxLength})"
                : "varchar(max)",
            DbType.DateTime => "datetime2",
            DbType.Boolean => "bit",
            DbType.Decimal => columnInfo.NumericPrecision.HasValue && columnInfo.NumericScale.HasValue
                ? $"decimal({columnInfo.NumericPrecision.Value}, {columnInfo.NumericScale.Value})"
                : "decimal(18, 4)",
            _ => throw new NotSupportedException($"Unsupported DbType for SQL Server: {columnInfo.DbType}")
        };
    }
}
