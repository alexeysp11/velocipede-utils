using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Models.QueryParameters;

/// <summary>
/// Contains the information needed for pagination.
/// </summary>
public readonly struct VelocipedePaginationInfo
{
    /// <summary>
    /// Pagination type.
    /// </summary>
    public readonly VelocipedePaginationType PaginationType { get; }

    /// <summary>
    /// Limit.
    /// </summary>
    public readonly int Limit { get; }

    /// <summary>
    /// Index of page (starts from 0).
    /// </summary>
    public readonly int Index { get; }

    /// <summary>
    /// Offset.
    /// </summary>
    public readonly int Offset { get; }

    /// <summary>
    /// Field name which is used for ordering query results.
    /// </summary>
    /// <remarks>By default, equals to <c>null</c>.</remarks>
    public readonly string? OrderingFieldName { get; }

    /// <summary>
    /// Default constructor for creating <see cref="VelocipedePaginationInfo"/>.
    /// </summary>
    /// <remarks>By default, it has the following values:
    /// <list type="bullet">
    ///     <item>
    ///         <description><see cref="PaginationType"/> = <see cref="VelocipedePaginationType.LimitOffset"/>,</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="Limit"/> = <see cref="int.MaxValue"/>,</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="Index"/> = 0,</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="Offset"/> = 0.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public VelocipedePaginationInfo()
    {
        PaginationType = VelocipedePaginationType.LimitOffset;
        Limit = int.MaxValue;
        Index = 0;
        Offset = 0;
    }

    /// <summary>
    /// Parameterized constructor for creating <see cref="VelocipedePaginationInfo"/>.
    /// </summary>
    /// <param name="limit">Limit.</param>
    /// <param name="index">Index of page (starts from 0).</param>
    /// <param name="offset">Offset.</param>
    /// <param name="paginationType">Pagination type.</param>
    /// <param name="orderingFieldName">Field name which is used for ordering query results.</param>
    public VelocipedePaginationInfo(
        int limit,
        int index,
        int offset,
        VelocipedePaginationType paginationType = VelocipedePaginationType.LimitOffset,
        string? orderingFieldName = null)
    {
        PaginationType = paginationType;
        Limit = limit;
        Index = index;
        Offset = offset;
        OrderingFieldName = orderingFieldName;
    }

    /// <summary>
    /// Create <see cref="VelocipedePaginationInfo"/> instance by index.
    /// </summary>
    /// <param name="limit">Limit.</param>
    /// <param name="index">Index of page (starts from 0).</param>
    /// <param name="paginationType">Pagination type.</param>
    /// <param name="orderingFieldName">Field name which is used for ordering query results.</param>
    public static VelocipedePaginationInfo CreateByIndex(
        int limit,
        int index,
        VelocipedePaginationType paginationType = VelocipedePaginationType.LimitOffset,
        string? orderingFieldName = null)
    {
        int offset = index * limit;
        return new VelocipedePaginationInfo(limit, index, offset, paginationType, orderingFieldName);
    }

    /// <summary>
    /// Create <see cref="VelocipedePaginationInfo"/> instance by offset.
    /// </summary>
    /// <param name="limit">Limit.</param>
    /// <param name="offset">Offset.</param>
    /// <param name="paginationType">Pagination type.</param>
    /// <param name="orderingFieldName">Field name which is used for ordering query results.</param>
    public static VelocipedePaginationInfo CreateByOffset(
        int limit,
        int offset,
        VelocipedePaginationType paginationType = VelocipedePaginationType.LimitOffset,
        string? orderingFieldName = null)
    {
        int index = offset / limit;
        return new VelocipedePaginationInfo(limit, index, offset, paginationType, orderingFieldName);
    }
}
