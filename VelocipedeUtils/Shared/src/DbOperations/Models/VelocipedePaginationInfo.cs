namespace VelocipedeUtils.Shared.DbOperations.Models;

/// <summary>
/// Record that contains the information needed for pagination.
/// </summary>
public readonly struct VelocipedePaginationInfo
{
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
    /// Default constructor for creating <see cref="VelocipedePaginationInfo"/>.
    /// </summary>
    /// <param name="limit">Limit.</param>
    /// <param name="index">Index of page (starts from 0).</param>
    /// <param name="offset">Offset.</param>
    public VelocipedePaginationInfo(int limit, int index, int offset)
    {
        Limit = limit;
        Index = index;
        Offset = offset;
    }

    /// <summary>
    /// Create <see cref="VelocipedePaginationInfo"/> instance by index.
    /// </summary>
    /// <param name="limit">Limit.</param>
    /// <param name="index">Index of page (starts from 0).</param>
    public static VelocipedePaginationInfo CreateByIndex(int limit, int index)
    {
        int offset = index * limit;
        return new VelocipedePaginationInfo(limit, index, offset);
    }

    /// <summary>
    /// Create <see cref="VelocipedePaginationInfo"/> instance by offset.
    /// </summary>
    /// <param name="limit">Limit.</param>
    /// <param name="offset">Offset.</param>
    public static VelocipedePaginationInfo CreateByOffset(int limit, int offset)
    {
        int index = offset / limit;
        return new VelocipedePaginationInfo(limit, index, offset);
    }
}
