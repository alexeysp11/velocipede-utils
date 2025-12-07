namespace VelocipedeUtils.Shared.DbOperations.Models.Loops;

/// <summary>
/// The result of executing the <c>foreach</c> operation for the specified database entities.
/// </summary>
public sealed class VelocipedeForeachResult
{
    /// <summary>
    /// A dictionary representing the result of the <c>foreach</c> operation.
    /// </summary>
    /// <remarks>
    /// By default, the property equals to <c>null</c> unless it is initialized explicitly, or by calling
    /// <see cref="Add(string, VelocipedeForeachTableInfo)"/> and <see cref="Remove(string)"/> methods.
    /// </remarks>
    public Dictionary<string, VelocipedeForeachTableInfo>? Result { get; set; }

    /// <summary>
    /// Add result into the collection.
    /// </summary>
    /// <param name="tableName">Table name.</param>
    /// <param name="info">Table info</param>
    public void Add(string tableName, VelocipedeForeachTableInfo info)
    {
        Result ??= [];
        if (!Result.TryAdd(tableName, info))
        {
            Result[tableName] = info;
        }
    }

    /// <summary>
    /// Remove info about the table from the collection.
    /// </summary>
    /// <param name="tableName">Table name.</param>
    /// <returns>
    /// <c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>.
    /// This method returns <c>false</c> if key is not found in the <see cref="Result"/> property.
    /// </returns>
    public bool Remove(string tableName)
    {
        Result ??= [];
        return Result.Remove(tableName);
    }
}
