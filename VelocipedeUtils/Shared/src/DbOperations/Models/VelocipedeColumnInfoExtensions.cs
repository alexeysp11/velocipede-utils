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
    /// Result of converting native database type into <see cref="DbType"/> with length, precision and scale.
    /// </summary>
    private readonly record struct DbTypeConvertResult
    {
        /// <summary>
        /// Database type.
        /// </summary>
        internal DbType DbType { get; init; }

        /// <summary>
        /// Length.
        /// </summary>
        internal int? Length { get; init; }

        /// <summary>
        /// Precision.
        /// </summary>
        internal int? Precision { get; init; }

        /// <summary>
        /// Scale.
        /// </summary>
        internal int? Scale { get; init; }
    }

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
    public static string? GetSqliteNativeType(this VelocipedeColumnInfo columnInfo)
    {
        if (!columnInfo.DbType.HasValue)
            return null;

        return columnInfo.DbType switch
        {
            DbType.SByte or DbType.Byte => "tinyint",
            DbType.Int16 or DbType.UInt16 => "smallint",
            DbType.Int32 or DbType.UInt32 => "integer",
            DbType.Int64 or DbType.UInt64 => "bigint",
            DbType.String or DbType.AnsiString => (columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0)
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
    /// Map <see cref="DbType"/> to SQL Server native type.
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
    /// <returns>Column data type used in .NET environment.</returns>
    public static DbType? GetSqliteDbType(this VelocipedeColumnInfo columnInfo)
    {
        static DbTypeConvertResult? ParseDbType(string nativeTypeLower, string pattern, DbType expectedType)
        {
            Match matchNumeric = Regex.Match(nativeTypeLower, pattern);
            if (matchNumeric.Success)
            {
                if (matchNumeric.Groups.Count == 3)
                {
                    if (int.TryParse(matchNumeric.Groups[2].Value, out int length))
                    {
                        return new DbTypeConvertResult { DbType = expectedType, Length = length };
                    }
                }
                if (matchNumeric.Groups.Count == 4)
                {
                    int? precision = null;
                    int? scale = null;
                    if (int.TryParse(matchNumeric.Groups[2].Value, out int precisionConverted))
                    {
                        precision = precisionConverted;
                    }
                    if (int.TryParse(matchNumeric.Groups[3].Value, out int scaleConverted))
                    {
                        scale = scaleConverted;
                    }
                    return new DbTypeConvertResult { DbType = expectedType, Precision = precision, Scale = scale };
                }
            }
            return null;
        }

        if (string.IsNullOrEmpty(columnInfo.NativeColumnType))
            return null;

        DbTypeConvertResult? dbTypeConvertResult;
        string nativeTypeLower = columnInfo.NativeColumnType.ToLower();

        // 1. Using Regex to find string types with a specified size.
        dbTypeConvertResult = ParseDbType(nativeTypeLower, @"(varchar|char|character|character varying|nvarchar|nchar)\s*\((\d+)\)", DbType.String);
        if (dbTypeConvertResult.HasValue)
        {
            columnInfo.CharMaxLength = dbTypeConvertResult.Value.Length;
            return dbTypeConvertResult.Value.DbType;
        }

        // 2. Using Regex to find numeric types with a specified size.
        dbTypeConvertResult = ParseDbType(nativeTypeLower, @"(numeric)\s*\((\d+)\)", DbType.VarNumeric);
        if (dbTypeConvertResult.HasValue)
        {
            columnInfo.NumericPrecision = dbTypeConvertResult.Value.Length;
            return dbTypeConvertResult.Value.DbType;
        }
        dbTypeConvertResult = ParseDbType(nativeTypeLower, @"(numeric)\s*\((\d+),(\d+)\)", DbType.VarNumeric);
        if (dbTypeConvertResult.HasValue)
        {
            columnInfo.NumericPrecision = dbTypeConvertResult.Value.Precision;
            columnInfo.NumericScale = dbTypeConvertResult.Value.Scale;
            return dbTypeConvertResult.Value.DbType;
        }
        dbTypeConvertResult = ParseDbType(nativeTypeLower, @"(decimal)\s*\((\d+)\)", DbType.Decimal);
        if (dbTypeConvertResult.HasValue)
        {
            columnInfo.NumericPrecision = dbTypeConvertResult.Value.Length;
            return dbTypeConvertResult.Value.DbType;
        }
        dbTypeConvertResult = ParseDbType(nativeTypeLower, @"(decimal)\s*\((\d+),(\d+)\)", DbType.Decimal);
        if (dbTypeConvertResult.HasValue)
        {
            columnInfo.NumericPrecision = dbTypeConvertResult.Value.Precision;
            columnInfo.NumericScale = dbTypeConvertResult.Value.Scale;
            return dbTypeConvertResult.Value.DbType;
        }

        // 3. Processing standard types (without specifying the length).
        DbType result = nativeTypeLower switch
        {
            "tinyint" => DbType.SByte,
            "smallint" or "int2" => DbType.Int16,
            "int" or "integer" or "int4" or "mediumint" => DbType.Int32,
            "bigint" or "int8" => DbType.Int64,
            "unsigned integer" => DbType.UInt32,
            "unsigned big int" or "unsigned bigint" => DbType.UInt64,
            "text" or "varchar" or "char" or "character" or "varying character" or "character varying" or "native character" or "nvarchar" or "nchar" or "clob" => DbType.String,
            "numeric" => DbType.VarNumeric,
            "decimal" => DbType.Decimal,
            "real" or "double" or "double precision" or "float" => DbType.Double,
            "boolean" or "bool" or "bit" => DbType.Boolean,
            "blob" => DbType.Binary,
            "datetime" or "timestamp" => DbType.DateTime,
            "datetime2" => DbType.DateTime2,
            "time" => DbType.Time,
            "date" => DbType.Date,
            _ => DbType.Object
        };
        if (result == DbType.String)
        {
            columnInfo.CharMaxLength = -1;
        }
        return result;
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
