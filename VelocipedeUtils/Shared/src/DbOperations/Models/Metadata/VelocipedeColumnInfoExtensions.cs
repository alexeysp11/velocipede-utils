using System.Data;
using System.Globalization;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Models.Metadata;

/// <summary>
/// Extension methods for <see cref="VelocipedeColumnInfo"/>.
/// </summary>
public static class VelocipedeColumnInfoExtensions
{
    /// <summary>
    /// Formats the default value into a string suitable for an SQL query,
    /// taking into account the requirements of different data types
    /// (quoted characters for strings/dates, unquoted characters for numbers).
    /// </summary>
    public static string FormatDefaultValue(this VelocipedeColumnInfo columnInfo)
    {
        object? value = columnInfo.DefaultValue;
        DbType type = columnInfo.ColumnType;

        if (value == null || value == DBNull.Value)
        {
            return "NULL";
        }

        switch (type)
        {
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.String:
            case DbType.StringFixedLength:
            case DbType.Guid:
                return $"'{value.ToString()?.Replace("'", "''")}'";

            case DbType.Xml:
                return columnInfo.DatabaseType switch
                {
                    VelocipedeDatabaseType.PostgreSQL => $"'{value.ToString()?.Replace("'", "''")}'::xml",
                    _ => $"'{value.ToString()?.Replace("'", "''")}'"
                };

            case DbType.Time:
                // Dates are formatted in the universal ISO 8601 format and enclosed in quotation marks.
                if (value is DateTime time)
                {
                    // This works for MSSQL and PostgreSQL.
                    return $"'{time:HH:mm:ss}'";
                }
                return $"'{value}'";

            case DbType.Date:
            case DbType.DateTime:
            case DbType.DateTime2:
            case DbType.DateTimeOffset:
                // Dates are formatted in the universal ISO 8601 format and enclosed in quotation marks.
                if (value is DateTime dateTime)
                {
                    // This works for MSSQL and PostgreSQL.
                    return $"'{dateTime:yyyy-MM-dd HH:mm:ss}'";
                }
                return $"'{value}'";

            case DbType.Boolean:
                // Boolean values ​​have different syntax in DBMS
                // MSSQL and SQLite use 0/1; PostgreSQL - true/false.
                if (columnInfo.DatabaseType == VelocipedeDatabaseType.PostgreSQL)
                {
                    return value.ToString()?.ToLowerInvariant() ?? "NULL";
                }
                return (bool)value ? "1" : "0";

            case DbType.Byte:
            case DbType.SByte:
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            case DbType.UInt16:
            case DbType.UInt32:
            case DbType.UInt64:
            case DbType.Decimal:
            case DbType.Double:
            case DbType.Single:
            case DbType.VarNumeric:
            case DbType.Currency:
                return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "NULL";

            default:
                // Other types (e.g. Binary) may require specific handling.
                return $"'{value}'";
        }
    }

    /// <summary>
    /// Get native type of the column, getting <see cref="VelocipedeColumnInfo.NativeColumnType"/> by <see cref="VelocipedeColumnInfo.ColumnType"/>.
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
        string baseType = columnInfo.ColumnType switch
        {
            DbType.SByte or DbType.Byte => "tinyint",
            DbType.Int16 or DbType.UInt16 => "smallint",
            DbType.Int32 or DbType.UInt32 => "integer",
            DbType.Int64 or DbType.UInt64 => "bigint",
            
            DbType.String or DbType.StringFixedLength or DbType.AnsiString or DbType.AnsiStringFixedLength
                => columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0
                    ? $"varchar({columnInfo.CharMaxLength})"
                    : "text",
            
            DbType.Xml => "text",

            DbType.Binary => "blob",
            
            DbType.Time => "time",
            DbType.Date => "date",
            DbType.DateTime or DbType.DateTimeOffset => "datetime",
            DbType.DateTime2 => "datetime2",
            
            DbType.Boolean => "boolean",

            DbType.Double or DbType.Single => "double precision",

            DbType.VarNumeric or DbType.Currency => "numeric",
            DbType.Decimal => "decimal",

            DbType.Guid => "text",

            _ => throw new NotSupportedException($"Unsupported DbType for SQLite: {columnInfo.ColumnType}")
        };

        // Numeric datatypes.
        if (columnInfo.ColumnType is DbType.VarNumeric or DbType.Decimal or DbType.Currency)
        {
            return GetNumericNativeColumnType(columnInfo, baseType);
        }

        return baseType;
    }

    /// <summary>
    /// Map <see cref="DbType"/> to PostgreSQL native type.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for PostgreSQL.</exception>
    public static string GetPostgresNativeType(this VelocipedeColumnInfo columnInfo)
    {
        string baseType = columnInfo.ColumnType switch
        {
            DbType.SByte or DbType.Byte or DbType.Int16 or DbType.UInt16 => "smallint",
            DbType.Int32 or DbType.UInt32 => "integer",
            DbType.Int64 or DbType.UInt64 => "bigint",
            
            DbType.String or DbType.StringFixedLength or DbType.AnsiString or DbType.AnsiStringFixedLength
                => columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0
                    ? $"varchar({columnInfo.CharMaxLength})"
                    : "text",

            DbType.Xml => "xml",

            DbType.Binary => "bytea",

            DbType.Time => "time",
            DbType.Date => "date",
            DbType.DateTime or DbType.DateTime2 => "timestamp",
            DbType.DateTimeOffset => "timestamp with time zone",
            
            DbType.Boolean => "boolean",

            DbType.Double or DbType.Single => "double precision",

            DbType.VarNumeric or DbType.Currency => "numeric",
            DbType.Decimal => "decimal",

            DbType.Guid => "uuid",

            _ => throw new NotSupportedException($"Unsupported DbType for PostgreSQL: {columnInfo.ColumnType}")
        };

        // Numeric datatypes.
        if (columnInfo.ColumnType is DbType.VarNumeric or DbType.Decimal or DbType.Currency)
        {
            return GetNumericNativeColumnType(columnInfo, baseType);
        }

        return baseType;
    }

    /// <summary>
    /// Map <see cref="DbType"/> to SQL Server native type.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <returns>String representation of the column type.</returns>
    /// <exception cref="NotSupportedException">Unsupported <see cref="DbType"/> for SQL Server.</exception>
    public static string GetMssqlNativeType(this VelocipedeColumnInfo columnInfo)
    {
        string baseType = columnInfo.ColumnType switch
        {
            DbType.SByte or DbType.Byte => "tinyint",
            DbType.Int16 or DbType.UInt16 => "smallint",
            DbType.Int32 or DbType.UInt32 => "int",
            DbType.Int64 or DbType.UInt64 => "bigint",

            DbType.String or DbType.StringFixedLength => columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0
                ? $"nvarchar({columnInfo.CharMaxLength})"
                : "nvarchar(max)",
            DbType.AnsiString or DbType.AnsiStringFixedLength => columnInfo.CharMaxLength.HasValue && columnInfo.CharMaxLength > 0
                ? $"varchar({columnInfo.CharMaxLength})"
                : "varchar(max)",
            
            DbType.Xml => "xml",

            DbType.Binary => "binary",
            
            DbType.Time => "time",
            DbType.Date => "date",
            DbType.DateTime => "datetime",
            DbType.DateTime2 => "datetime2",
            DbType.DateTimeOffset => "datetimeoffset",

            DbType.Boolean => "bit",

            DbType.Double or DbType.Single => "double precision",

            DbType.VarNumeric or DbType.Currency => "numeric",
            DbType.Decimal => "decimal",

            DbType.Guid => "uniqueidentifier",

            _ => throw new NotSupportedException($"Unsupported DbType for SQL Server: {columnInfo.ColumnType}")
        };

        // Numeric datatypes.
        if (columnInfo.ColumnType is DbType.VarNumeric or DbType.Decimal or DbType.Currency)
        {
            return GetNumericNativeColumnType(columnInfo, baseType);
        }

        return baseType;
    }

    /// <summary>
    /// Convert to numeric native column type.
    /// </summary>
    /// <param name="columnInfo">Column metadata.</param>
    /// <param name="baseType">Pre-calculated column base type.</param>
    /// <returns>Numeric native column type.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown in the following cases:
    /// <list type="bullet">
    ///     <item><description><see cref="VelocipedeColumnInfo.ColumnType"/> is not <see cref="DbType.VarNumeric"/>, <see cref="DbType.Decimal"/> or <see cref="DbType.Currency"/>;</description></item>
    ///     <item><description><paramref name="baseType"/> is not <c>"numeric"</c> or <c>"decimal"</c>;</description></item>
    ///     <item><description><see cref="VelocipedeColumnInfo.NumericScale"/> is bigger than <see cref="VelocipedeColumnInfo.NumericPrecision"/>.</description></item>
    /// </list>
    /// </exception>
    public static string GetNumericNativeColumnType(this VelocipedeColumnInfo columnInfo, string baseType)
    {
        if (columnInfo.ColumnType is not (DbType.VarNumeric or DbType.Decimal or DbType.Currency))
        {
            throw new InvalidOperationException(ErrorMessageConstants.NumericNativeColumnTypeConversion);
        }
        if (baseType is not ("numeric" or "decimal"))
        {
            throw new InvalidOperationException(ErrorMessageConstants.IncorrectBaseForNumericNativeColumnType);
        }

        bool hasValidPrecision = columnInfo.NumericPrecision.HasValue && columnInfo.NumericPrecision > 0;
        bool hasValidScale = columnInfo.NumericScale.HasValue && columnInfo.NumericScale > 0;
        if (hasValidPrecision && hasValidScale)
        {
            if (columnInfo.NumericPrecision < columnInfo.NumericScale)
            {
                throw new InvalidOperationException(ErrorMessageConstants.NumericScaleBiggerThanPrecision);
            }

            if (columnInfo.ColumnType is DbType.Currency)
            {
                return columnInfo.NumericScale > NumericConstants.CurrencyDefaultScale
                    ? $"{baseType}({columnInfo.NumericPrecision}, {NumericConstants.CurrencyDefaultScale})"
                    : $"{baseType}({columnInfo.NumericPrecision}, {columnInfo.NumericScale})";
            }
            else
            {
                return $"{baseType}({columnInfo.NumericPrecision}, {columnInfo.NumericScale})";
            }
        }
        else
        {
            if (columnInfo.ColumnType is DbType.Currency)
            {
                return hasValidPrecision && columnInfo.NumericPrecision > NumericConstants.CurrencyDefaultScale
                    ? $"{baseType}({columnInfo.NumericPrecision}, {NumericConstants.CurrencyDefaultScale})"
                    : $"{baseType}({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})";
            }
            else
            {
                return hasValidPrecision ? $"{baseType}({columnInfo.NumericPrecision})" : baseType;
            }
        }
    }
}
