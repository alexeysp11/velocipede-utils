namespace VelocipedeUtils.Shared.DbOperations.Iterators
{
    /// <summary>
    /// Iterator for the foreach operation.
    /// </summary>
    public interface IVelocipedeForeachTableIterator : IVelocipedeIterator
    {
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
