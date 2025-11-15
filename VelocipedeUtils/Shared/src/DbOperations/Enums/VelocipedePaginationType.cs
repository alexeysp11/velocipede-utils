namespace VelocipedeUtils.Shared.DbOperations.Enums;

/// <summary>
/// Pagination type.
/// </summary>
public enum VelocipedePaginationType
{
    /// <summary>
    /// Undefined pagination type.
    /// </summary>
    /// <remarks>If this type of pagination selected, no pagination will be applied.</remarks>
    None = 0,

    /// <summary>
    /// Traditional LIMIT and OFFSET pagination.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method calculates an OFFSET value to skip a certain number of rows from the beginning of the result set:
    /// <c>SELECT * FROM your_table ORDER BY id ASC LIMIT 10 OFFSET 20</c>.
    /// </para>
    /// <para>
    /// Can become slow on large datasets because the database still needs to scan and then discard all the rows up to the offset.
    /// </para>
    /// </remarks>
    LimitOffset = 1,

    /// <summary>
    /// Keyset (cursor) pagination using ID.
    /// </summary>
    /// <remarks>
    /// Instead of using a numerical offset, it uses the last value from the previous page to filter the next set of results:
    /// <c>SELECT * FROM your_table WHERE id > [last_id_from_previous_page] ORDER BY id ASC LIMIT 10</c>.
    /// </remarks>
    KeysetById = 2
}
