using System.Data;
using System.Text.RegularExpressions;
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
    public static string? GetNativeType(this VelocipedeColumnInfo columnInfo)
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
    public static string? GetSqliteNativeType(this VelocipedeColumnInfo columnInfo)
    {
        if (!columnInfo.DbType.HasValue)
            return null;

        return columnInfo.DbType switch
        {
            DbType.Int32 => "integer",
            DbType.String or DbType.AnsiString => (columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0)
                ? $"varchar({columnInfo.CharMaxLength})"
                : "text",
            DbType.DateTime => "datetime",
            DbType.Boolean => "integer",
            DbType.Double => "real",
            DbType.VarNumeric or DbType.Decimal => "numeric",
            _ => throw new NotSupportedException($"Unsupported DbType for SQLite: {columnInfo.DbType}")
        };
    }

    /// <summary>
    /// Map <see cref="DbType"/> to PostgreSQL alternative.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for PostgreSQL.</exception>
    public static string? GetPostgresNativeType(this VelocipedeColumnInfo columnInfo)
    {
        if (!columnInfo.DbType.HasValue)
            return null;

        return columnInfo.DbType switch
        {
            DbType.Int32 => "integer",
            DbType.String or DbType.AnsiString => (columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0)
                ? $"varchar({columnInfo.CharMaxLength})"
                : "text",
            DbType.DateTime => "timestamp",
            DbType.Boolean => "boolean",
            DbType.Decimal => (columnInfo.NumericPrecision.HasValue && columnInfo.NumericScale.HasValue)
                ? $"decimal({columnInfo.NumericPrecision.Value}, {columnInfo.NumericScale.Value})"
                : "decimal(18, 4)",
            _ => throw new NotSupportedException($"Unsupported DbType for PostgreSQL: {columnInfo.DbType}")
        };
    }

    /// <summary>
    /// Map <see cref="DbType"/> to SQL Server alternative.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for SQL Server.</exception>
    public static string? GetMssqlNativeType(this VelocipedeColumnInfo columnInfo)
    {
        if (!columnInfo.DbType.HasValue)
            return null;

        return columnInfo.DbType switch
        {
            DbType.Int32 => "int",
            DbType.String => (columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0)
                ? $"nvarchar({columnInfo.CharMaxLength})"
                : "nvarchar(max)",
            DbType.AnsiString => (columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0)
                ? $"varchar({columnInfo.CharMaxLength})"
                : "varchar(max)",
            DbType.DateTime => "datetime2",
            DbType.Boolean => "bit",
            DbType.Decimal => (columnInfo.NumericPrecision.HasValue && columnInfo.NumericScale.HasValue)
                ? $"decimal({columnInfo.NumericPrecision.Value}, {columnInfo.NumericScale.Value})"
                : "decimal(18, 4)",
            _ => throw new NotSupportedException($"Unsupported DbType for SQL Server: {columnInfo.DbType}")
        };
    }

    /// <summary>
    /// Get <see cref="DbType"/> by native type.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Thrown if <see cref="VelocipedeColumnInfo.DatabaseType"/> is not supported.</exception>
    public static DbType? GetDbType(this VelocipedeColumnInfo columnInfo)
    {
        return columnInfo.DatabaseType switch
        {
            DatabaseType.SQLite => columnInfo.GetSqliteDbType(),
            DatabaseType.PostgreSQL => columnInfo.GetPostgresDbType(),
            DatabaseType.MSSQL => columnInfo.GetMssqlDbType(),
            _ => throw new NotSupportedException($"Unsupported database type: {columnInfo.DatabaseType}")
        };
    }

    /// <summary>
    /// Get <see cref="DbType"/> by native type for SQLite.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns></returns>
    public static DbType? GetSqliteDbType(this VelocipedeColumnInfo columnInfo)
    {
        if (string.IsNullOrEmpty(columnInfo.NativeColumnType))
            return null;

        string nativeTypeLower = columnInfo.NativeColumnType.ToLower();

        // 1. Using Regex to find string types with a specified size.
        var match = Regex.Match(nativeTypeLower, @"(varchar|char|character|nvarchar|nchar)\s*\((\d+)\)");
        if (match.Success)
        {
            if (int.TryParse(match.Groups[2].Value, out int length))
            {
                columnInfo.CharMaxLength = length;
            }
            return DbType.String;
        }

        // 2. Processing standard types (without specifying the length).
        return nativeTypeLower switch
        {
            "tinyint" => DbType.SByte,
            "smallint" or "int2" => DbType.Int16,
            "int" or "integer" or "int4" or "mediumint" => DbType.Int32,
            "bigint" or "int8" => DbType.Int64,
            "unsigned big int" => DbType.UInt64,
            "text" or "varchar" => DbType.String,
            "numeric" => DbType.Decimal,
            "real" or "double" => DbType.Double,
            "blob" => DbType.Binary,
            _ => DbType.Object
        };
    }

    /// <summary>
    /// Get <see cref="DbType"/> by native type for PostgreSQL.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns></returns>
    public static DbType? GetPostgresDbType(this VelocipedeColumnInfo columnInfo)
    {
        if (string.IsNullOrEmpty(columnInfo.NativeColumnType))
            return null;

        return columnInfo.NativeColumnType.ToLower() switch
        {
            "int" or "integer" => DbType.Int32,
            "text" or "varchar" or "character varying" => DbType.String,
            "decimal" or "numeric" => DbType.Decimal,
            "boolean" => DbType.Boolean,
            _ => DbType.Object
        };
    }

    /// <summary>
    /// Get <see cref="DbType"/> by native type for SQL Server.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns></returns>
    public static DbType? GetMssqlDbType(this VelocipedeColumnInfo columnInfo)
    {
        if (string.IsNullOrEmpty(columnInfo.NativeColumnType))
            return null;

        return columnInfo.NativeColumnType.ToLower() switch
        {
            "int" => DbType.Int32,
            "nvarchar" or "varchar" => DbType.String,
            "decimal" or "numeric" => DbType.Decimal,
            "bit" => DbType.Boolean,
            _ => DbType.Object
        };
    }
}
