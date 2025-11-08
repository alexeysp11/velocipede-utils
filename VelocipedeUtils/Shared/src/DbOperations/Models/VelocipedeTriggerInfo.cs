namespace VelocipedeUtils.Shared.DbOperations.Models;

/// <summary>
/// Info about trigger.
/// </summary>
public sealed class VelocipedeTriggerInfo
{
    /// <summary>
    /// Name of the database that contains the trigger (always the current database).
    /// </summary>
    public string? TriggerCatalog { get; set; }

    /// <summary>
    /// Name of the schema that contains the trigger.
    /// </summary>
    public string? TriggerSchema { get; set; }

    /// <summary>
    /// Name of the trigger.
    /// </summary>
    public string? TriggerName { get; set; }

    /// <summary>
    /// Event that fires the trigger (INSERT, UPDATE, or DELETE).
    /// </summary>
    public string? EventManipulation { get; set; }

    /// <summary>
    /// Name of the database that contains the table that the trigger is defined on (always the current database).
    /// </summary>
    public string? EventObjectCatalog { get; set; }

    /// <summary>
    /// Name of the schema that contains the table that the trigger is defined on.
    /// </summary>
    public string? EventObjectSchema { get; set; }

    /// <summary>
    /// Name of the table that the trigger is defined on.
    /// </summary>
    public string? EventObjectTable { get; set; }

    /// <summary>
    /// Action order for the trigger.
    /// </summary>
    /// <remarks>
    /// Firing order among triggers on the same table having the same event_manipulation, action_timing, and action_orientation.
    /// In PostgreSQL, triggers are fired in name order, so this column reflects that.
    /// </remarks>
    public int? ActionOrder { get; set; }

    /// <summary>
    /// The page number of the root b-tree page for tables and indexes.
    /// </summary>
    /// <remarks>For rows that define views, triggers, and virtual tables, the rootpage column is 0 or NULL.</remarks>
    public int? RootPage { get; set; }

    /// <summary>
    /// WHEN condition of the trigger, null if none (also null if the table is not owned by a currently enabled role).
    /// </summary>
    public string? ActionCondition { get; set; }

    /// <summary>
    /// Statement that is executed by the trigger.
    /// </summary>
    /// <remarks>In PostgreSQL currently always EXECUTE FUNCTION function(...).</remarks>
    public string? ActionStatement { get; set; }

    /// <summary>
    /// Identifies whether the trigger fires once for each processed row or once for each statement (ROW or STATEMENT).
    /// </summary>
    public string? ActionOrientation { get; set; }

    /// <summary>
    /// Time at which the trigger fires (BEFORE, AFTER, or INSTEAD OF).
    /// </summary>
    public string? ActionTiming { get; set; }

    /// <summary>
    /// Name of the “old” transition table, or null if none.
    /// </summary>
    public string? ActionReferenceOldTable { get; set; }

    /// <summary>
    /// Name of the “new” transition table, or null if none.
    /// </summary>
    public string? ActionReferenceNewTable { get; set; }

    /// <summary>
    /// Applies to a feature not available in PostgreSQL.
    /// </summary>
    public string? ActionReferenceOldRow { get; set; }

    /// <summary>
    /// Applies to a feature not available in PostgreSQL.
    /// </summary>
    public string? ActionReferenceNewRow { get; set; }

    /// <summary>
    /// Applies to a feature not available in PostgreSQL.
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// SQL text that describes the object.
    /// </summary>
    public string? SqlDefinition { get; set; }

    /// <summary>
    /// The trigger is active.
    /// </summary>
    public bool? IsActive { get; set; }
}
