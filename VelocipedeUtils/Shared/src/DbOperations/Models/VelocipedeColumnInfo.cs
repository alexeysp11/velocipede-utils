namespace VelocipedeUtils.Shared.DbOperations.Models;

/// <summary>
/// Metadata about table columns.
/// </summary>
public sealed class VelocipedeColumnInfo
{
    /// <summary>
    /// Column identifier.
    /// </summary>
    public int ColumnId { get; set; }

    /// <summary>
    /// Column name.
    /// </summary>
    public string? ColumnName { get; set; }

    /// <summary>
    /// Column type represented as a string value.
    /// </summary>
    public string? ColumnType { get; set; }

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

    /// <summary>
    /// Whether the column is self-referencing.
    /// </summary>
    public bool? IsSelfReferencing { get; set; }

    /// <summary>
    /// Whether the column is generated.
    /// </summary>
    public bool? IsGenerated { get; set; }

    /// <summary>
    /// Whether the column is updatable.
    /// </summary>
    public bool? IsUpdatable { get; set; }
}
