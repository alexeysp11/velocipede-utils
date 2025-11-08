using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Iterators;

/// <summary>
/// Iterator for the <c>foreach</c> operation.
/// </summary>
public interface IVelocipedeForeachTableIterator
{
    /// <summary>
    /// Declare the beginning of the <c>foreach</c> operation.
    /// </summary>
    /// <returns>The current <see cref="IVelocipedeForeachTableIterator"/> instance, allowing for further configuration.</returns>
    IVelocipedeForeachTableIterator BeginForeach();

    /// <summary>
    /// Declare the end of the <c>foreach</c> operation.
    /// </summary>
    /// <returns>The current <see cref="IVelocipedeForeachTableIterator"/> instance, allowing for further configuration.</returns>
    IVelocipedeForeachTableIterator EndForeach();

    /// <summary>
    /// Get the result of the <c>foreach</c> operation.
    /// </summary>
    /// <param name="foreachResult">
    /// <para>
    /// Result of the <c>foreach</c> operation.
    /// </para>
    /// <para>
    /// If there is no active operations, then <c>null</c> should be returned as an <c>out</c> parameter;
    /// otherwise, the valid <see cref="VelocipedeForeachResult"/> is expected.
    /// </para>
    /// </param>
    /// <returns>The current <see cref="IVelocipedeForeachTableIterator"/> instance, allowing for further configuration.</returns>
    IVelocipedeDbConnection GetForeachResult(out VelocipedeForeachResult? foreachResult);

    /// <summary>
    /// Decare operation for getting all data from table.
    /// </summary>
    /// <returns>The current <see cref="IVelocipedeForeachTableIterator"/> instance, allowing for further configuration.</returns>
    IVelocipedeForeachTableIterator GetAllData();

    /// <summary>
    /// Decare operation for getting columns from table.
    /// </summary>
    /// <returns>The current <see cref="IVelocipedeForeachTableIterator"/> instance, allowing for further configuration.</returns>
    IVelocipedeForeachTableIterator GetColumns();

    /// <summary>
    /// Decare operation for getting all foreign keys from table.
    /// </summary>
    /// <returns>The current <see cref="IVelocipedeForeachTableIterator"/> instance, allowing for further configuration.</returns>
    IVelocipedeForeachTableIterator GetForeignKeys();

    /// <summary>
    /// Decare operation for getting triggers from table.
    /// </summary>
    /// <returns>The current <see cref="IVelocipedeForeachTableIterator"/> instance, allowing for further configuration.</returns>
    IVelocipedeForeachTableIterator GetTriggers();

    /// <summary>
    /// Decare operation for getting SQL definition from table.
    /// </summary>
    /// <returns>The current <see cref="IVelocipedeForeachTableIterator"/> instance, allowing for further configuration.</returns>
    IVelocipedeForeachTableIterator GetSqlDefinition();
}
