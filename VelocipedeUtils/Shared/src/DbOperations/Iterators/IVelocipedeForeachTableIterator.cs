using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Iterators
{
    /// <summary>
    /// Iterator for the foreach operation.
    /// </summary>
    public interface IVelocipedeForeachTableIterator
    {
        /// <summary>
        /// Declare the beginning of the <c>foreach</c> operation.
        /// </summary>
        IVelocipedeForeachTableIterator BeginForeach();

        /// <summary>
        /// Declare the end of the <c>foreach</c> operation.
        /// </summary>
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
        /// <returns>Instance of <see cref="IVelocipedeDbConnection"/> within which the <c>foreach</c> operation was executed.</returns>
        IVelocipedeDbConnection GetForeachResult(out VelocipedeForeachResult? foreachResult);

        /// <summary>
        /// Decare operation for getting all data from table.
        /// </summary>
        IVelocipedeForeachTableIterator GetAllDataFromTable();

        /// <summary>
        /// Decare operation for getting columns from table.
        /// </summary>
        IVelocipedeForeachTableIterator GetColumns();

        /// <summary>
        /// Decare operation for getting all foreign keys from table.
        /// </summary>
        IVelocipedeForeachTableIterator GetForeignKeys();

        /// <summary>
        /// Decare operation for getting triggers from table.
        /// </summary>
        IVelocipedeForeachTableIterator GetTriggers();

        /// <summary>
        /// Decare operation for getting SQL definition from table.
        /// </summary>
        IVelocipedeForeachTableIterator GetSqlDefinition();
    }
}
