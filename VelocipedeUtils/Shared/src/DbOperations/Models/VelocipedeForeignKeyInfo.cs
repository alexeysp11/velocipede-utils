using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Models;

/// <summary>
/// Info about foreign key constraints.
/// </summary>
public sealed class VelocipedeForeignKeyInfo
{
    /// <summary>
    /// Database type.
    /// </summary>
    public DatabaseType DatabaseType { get; set; }

    /// <summary>
    /// A unique identifier for the foreign key within the table.
    /// </summary>
    public long ForeignKeyId { get; set; }

    /// <summary>
    /// The sequence number of the column within a multi-column foreign key.
    /// </summary>
    /// <remarks>For single-column foreign keys, this will typically be 0.</remarks>
    public long SequenceNumber { get; set; }

    /// <summary>
    /// Constraint name.
    /// </summary>
    public string? ConstraintName { get; set; }

    /// <summary>
    /// The name of the current table's schema.
    /// </summary>
    public string? FromTableSchema { get; set; }

    /// <summary>
    /// The name of the current table.
    /// </summary>
    public string? FromTableName { get; set; }

    /// <summary>
    /// The name of the column in the current table.
    /// </summary>
    public string? FromColumn { get; set; }

    /// <summary>
    /// The name of the parent table's schema referenced by the foreign key.
    /// </summary>
    public string? ToTableSchema { get; set; }

    /// <summary>
    /// The name of the parent table referenced by the foreign key.
    /// </summary>
    public string? ToTableName { get; set; }

    /// <summary>
    /// The name of the column in the parent table that the foreign key references.
    /// </summary>
    public string? ToColumn { get; set; }

    /// <summary>
    /// The action taken when a referenced key in the parent table is updated
    /// (e.g., NO ACTION, CASCADE, SET NULL, RESTRICT, SET DEFAULT).
    /// </summary>
    public string? OnUpdate { get; set; }

    /// <summary>
    /// The action taken when a referenced key in the parent table is deleted
    /// (e.g., NO ACTION, CASCADE, SET NULL, RESTRICT, SET DEFAULT).
    /// </summary>
    public string? OnDelete { get; set; }

    /// <summary>
    /// The matching clause used for the foreign key (e.g., NONE, SIMPLE, PARTIAL, FULL).
    /// </summary>
    public string? MatchingClause { get; set; }
}
