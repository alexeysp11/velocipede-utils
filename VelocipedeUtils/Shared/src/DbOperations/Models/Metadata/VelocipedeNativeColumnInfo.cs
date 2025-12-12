using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Models.Metadata;

/// <summary>
/// Metadata about table columns from database.
/// </summary>
public sealed class VelocipedeNativeColumnInfo
{
    /// <summary>
    /// Database type.
    /// </summary>
    public VelocipedeDatabaseType DatabaseType { get; set; }

    /// <summary>
    /// Column name.
    /// </summary>
    public string? ColumnName { get; set; }

    /// <summary>
    /// Column native type represented as a string.
    /// </summary>
    public string? NativeColumnType { get; set; }

    /// <summary>
    /// The calculated type of the column.
    /// </summary>
    public DbType? CalculatedColumnType => this.GetCalculatedColumnType();

    /// <summary>
    /// If native column type identifies a character or bit string type, the declared maximum length;
    /// <c>null</c> for all other data types or if no maximum length was declared.
    /// </summary>
    public int? CharMaxLength { get; set; }

    /// <summary>
    /// Numeric precision for integer/decimal/numeric.
    /// </summary>
    public int? NumericPrecision { get; set; }

    /// <summary>
    /// Numeric scale for integer/decimal/numeric.
    /// </summary>
    public int? NumericScale { get; set; }

    /// <summary>
    /// Default value of the column.
    /// </summary>
    public object? DefaultValue { get; set; }

    /// <summary>
    /// Whether the column is a primary key.
    /// </summary>
    public bool IsPrimaryKey { get; set; }

    /// <summary>
    /// Whether the column is nullable.
    /// </summary>
    public bool IsNullable { get; set; }
}
