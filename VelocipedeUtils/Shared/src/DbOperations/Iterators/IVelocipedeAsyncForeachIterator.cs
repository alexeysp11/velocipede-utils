using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Iterators
{
    /// <summary>
    /// Iterator for the asynchronous <c>foreach</c> operation.
    /// </summary>
    public interface IVelocipedeAsyncForeachIterator
    {
        /// <summary>
        /// Declare the beginning of the <c>foreach</c> operation.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeAsyncForeachIterator"/> instance, allowing for further configuration.</returns>
        IVelocipedeAsyncForeachIterator BeginAsyncForeach();

        /// <summary>
        /// Declare the end of the <c>foreach</c> operation.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeAsyncForeachIterator"/> instance, allowing for further configuration.</returns>
        IVelocipedeAsyncForeachIterator EndAsyncForeach();

        /// <summary>
        /// Get the result of the <c>foreach</c> operation.
        /// </summary>
        /// <returns>
        /// <para>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation that contains a result of the <c>foreach</c> operation.
        /// </para>
        /// <para>
        /// If there is no active operations, then <c>null</c> should be returned as an <c>out</c> parameter;
        /// otherwise, the valid <see cref="VelocipedeForeachResult"/> is expected.
        /// </para>
        /// </returns>
        Task<VelocipedeForeachResult?> GetResultAsync();

        /// <summary>
        /// Decare operation for getting all data from table.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeAsyncForeachIterator"/> instance, allowing for further configuration.</returns>
        IVelocipedeAsyncForeachIterator GetAllDataAsync();

        /// <summary>
        /// Decare operation for getting columns from table.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeAsyncForeachIterator"/> instance, allowing for further configuration.</returns>
        IVelocipedeAsyncForeachIterator GetColumnsAsync();

        /// <summary>
        /// Decare operation for getting all foreign keys from table.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeAsyncForeachIterator"/> instance, allowing for further configuration.</returns>
        IVelocipedeAsyncForeachIterator GetForeignKeysAsync();

        /// <summary>
        /// Decare operation for getting triggers from table.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeAsyncForeachIterator"/> instance, allowing for further configuration.</returns>
        IVelocipedeAsyncForeachIterator GetTriggersAsync();

        /// <summary>
        /// Decare operation for getting SQL definition from table.
        /// </summary>
        /// <returns>The current <see cref="IVelocipedeAsyncForeachIterator"/> instance, allowing for further configuration.</returns>
        IVelocipedeAsyncForeachIterator GetSqlDefinitionAsync();
    }
}
