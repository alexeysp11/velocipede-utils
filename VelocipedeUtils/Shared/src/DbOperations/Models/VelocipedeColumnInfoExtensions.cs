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
    /// Get native type of the column, getting <see cref="VelocipedeColumnInfo.NativeColumnType"/> by <see cref="VelocipedeColumnInfo.DbType"/>.
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
            columnInfo.NumericScale = 0;
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
            columnInfo.NumericScale = 0;
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
        DbType result;
        switch (nativeTypeLower)
        {
            case "tinyint":
                result = DbType.SByte;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 8;
                columnInfo.NumericScale = 0;
                break;

            case "smallint" or "int2":
                result = DbType.Int16;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 16;
                columnInfo.NumericScale = 0;
                break;

            case "int" or "integer" or "int4" or "mediumint":
                result = DbType.Int32;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 32;
                columnInfo.NumericScale = 0;
                break;

            case "bigint" or "int8":
                result = DbType.Int64;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 64;
                columnInfo.NumericScale = 0;
                break;

            case "unsigned integer":
                result = DbType.UInt32;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 32;
                columnInfo.NumericScale = 0;
                break;

            case "unsigned big int" or "unsigned bigint":
                result = DbType.UInt64;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 64;
                columnInfo.NumericScale = 0;
                break;

            case "text" or "varchar" or "char" or "character" or "varying character" or "character varying" or "native character" or "nvarchar" or "nchar" or "clob":
                result = DbType.String;
                columnInfo.CharMaxLength = -1;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "numeric":
                result = DbType.VarNumeric;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "decimal":
                result = DbType.Decimal;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "real" or "double" or "double precision" or "float":
                result = DbType.Double;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "boolean" or "bool" or "bit":
                result = DbType.Boolean;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "blob":
                result = DbType.Binary;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "datetime" or "timestamp":
                result = DbType.DateTime;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "datetime2":
                result = DbType.DateTime2;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "time":
                result = DbType.Time;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            case "date":
                result = DbType.Date;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;

            default:
                result = DbType.Object;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;
        }
        return result;
    }

    /// <summary>
    /// Get <see cref="DbType"/> by native type for PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// In PostgreSQL, a primary key cannot be nullable because of its fundamental purpose in relational databases:
    /// to uniquely identify each row in a table.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// A <c>SERIAL</c> column is implicitly <c>NOT NULL</c> as well because its primary purpose is to provide a unique identifier for each row.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns></returns>
    public static DbType? GetPostgresDbType(this VelocipedeColumnInfo columnInfo)
    {
        if (string.IsNullOrEmpty(columnInfo.NativeColumnType))
            return null;

        string nativeColumnType = columnInfo.NativeColumnType.ToLower();
        DbType result = nativeColumnType switch
        {
            "smallint" or "smallserial" or "int2" => DbType.Int16,
            "int" or "integer" or "serial" or "int4" => DbType.Int32,
            "bigint" or "bigserial" or "int8" => DbType.Int64,
            "text" or "varchar" or "character varying" or "character" or "char" or "bpchar" => DbType.String,
            "decimal" => DbType.Decimal,
            "numeric" => DbType.VarNumeric,
            "real" or "double precision" or "float4" or "float8" => DbType.Double,
            "boolean" or "bool" => DbType.Boolean,
            "bytea" => DbType.Binary,
            "timestamp" or "timestamp without time zone" => DbType.DateTime,
            "timestamp with time zone" or "timestamptz" => DbType.DateTimeOffset,
            "date" => DbType.Date,
            "time" or "time without time zone" or "time with time zone" => DbType.Time,
            "interval" => DbType.DateTime,
            _ => DbType.Object
        };
        if (result == DbType.Double)
        {
            columnInfo.NumericPrecision = null;
            columnInfo.NumericScale = null;
        }
        return result;
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

        string nativeColumnType = columnInfo.NativeColumnType.ToLower();
        DbType result;
        switch (nativeColumnType)
        {
            case "tinyint":
                result = DbType.SByte;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 8;
                columnInfo.NumericScale = 0;
                break;

            case "smallint":
                result = DbType.Int16;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 16;
                columnInfo.NumericScale = 0;
                break;
            
            case "int" or "integer":
                result = DbType.Int32;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 32;
                columnInfo.NumericScale = 0;
                break;
            
            case "bigint":
                result = DbType.Int64;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = 64;
                columnInfo.NumericScale = 0;
                break;
            
            case "nvarchar" or "varchar":
                result = DbType.String;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;
            
            case "decimal" or "numeric":
                result = DbType.Decimal;
                columnInfo.CharMaxLength = null;
                break;

            case "bit":
                result = DbType.Boolean;
                columnInfo.CharMaxLength = null;
                columnInfo.NumericPrecision = null;
                columnInfo.NumericScale = null;
                break;
            
            default:
                result = DbType.Object;
                break;
        }
        return result;
    }
}
