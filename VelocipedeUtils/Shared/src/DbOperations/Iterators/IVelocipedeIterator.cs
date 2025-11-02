using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Iterators
{
    /// <summary>
    /// Base interface for the iterator.
    /// </summary>
    public interface IVelocipedeIterator
    {
        /// <summary>
        /// Declare the end of the <c>foreach</c> operation.
        /// </summary>
        IVelocipedeIterator EndForeach();

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
    }
}
