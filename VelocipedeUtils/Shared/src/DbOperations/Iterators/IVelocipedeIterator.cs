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
        /// Declare the end of the foreach operation.
        /// </summary>
        IVelocipedeIterator EndForeach();

        /// <summary>
        /// Get the result of the foreach operation.
        /// </summary>
        IVelocipedeDbConnection GetForeachResult(out VelocipedeForeachResult? foreachResult);
    }
}
