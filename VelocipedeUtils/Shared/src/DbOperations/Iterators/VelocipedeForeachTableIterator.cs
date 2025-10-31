using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Iterators
{
    /// <summary>
    /// Iterator for the foreach operation.
    /// </summary>
    public sealed class VelocipedeForeachTableIterator : IVelocipedeForeachTableIterator
    {
        /// <summary>
        /// Type of the foreach operation for the table.
        /// </summary>
        private enum ForeachTableOperationType
        {
            /// <summary>
            /// Get all data from table.
            /// </summary>
            GetAllDataFromTable,

            /// <summary>
            /// Get columns from table.
            /// </summary>
            GetColumns,

            /// <summary>
            /// Get all foreign keys from table.
            /// </summary>
            GetForeignKeys,

            /// <summary>
            /// Get triggers from table.
            /// </summary>
            GetTriggers,

            /// <summary>
            /// Get SQL definition from table.
            /// </summary>
            GetSqlDefinition,
        }

        private IVelocipedeDbConnection _connection;
        private List<string> _tables;
        private VelocipedeForeachResult? _foreachResult;
        private bool _allowAddOperationTypes;
        private Dictionary<ForeachTableOperationType, bool> _operationTypes;

        public VelocipedeForeachTableIterator(IVelocipedeDbConnection connection, List<string> tables)
        {
            _connection = connection;
            _tables = tables;
            _allowAddOperationTypes = true;
            _operationTypes = new Dictionary<ForeachTableOperationType, bool>();
        }

        public IVelocipedeIterator EndForeach()
        {
            _allowAddOperationTypes = false;
            return this;
        }

        public IVelocipedeDbConnection GetForeachResult(out VelocipedeForeachResult? foreachResult)
        {
            if (_allowAddOperationTypes)
                throw new InvalidOperationException(ErrorMessageConstants.UnableToGetResultForOpenForeachOperation);

            foreachResult = _foreachResult;
            return _connection;
        }

        public IVelocipedeForeachTableIterator GetAllDataFromTable()
        {
            TryAddOperationType(ForeachTableOperationType.GetAllDataFromTable);
            return this;
        }

        public IVelocipedeForeachTableIterator GetColumns()
        {
            TryAddOperationType(ForeachTableOperationType.GetColumns);
            return this;
        }

        public IVelocipedeForeachTableIterator GetForeignKeys()
        {
            TryAddOperationType(ForeachTableOperationType.GetForeignKeys);
            return this;
        }

        public IVelocipedeForeachTableIterator GetSqlDefinition()
        {
            TryAddOperationType(ForeachTableOperationType.GetSqlDefinition);
            return this;
        }

        public IVelocipedeForeachTableIterator GetTriggers()
        {
            TryAddOperationType(ForeachTableOperationType.GetTriggers);
            return this;
        }

        private void TryAddOperationType(ForeachTableOperationType operationType)
        {
            if (!_allowAddOperationTypes)
                throw new InvalidOperationException(ErrorMessageConstants.UnableToAddActionForOpenForeachOperation);
            
            if (_operationTypes.ContainsKey(operationType))
            {
                _operationTypes[ForeachTableOperationType.GetAllDataFromTable] = true;
            }
            else
            {
                _operationTypes.Add(ForeachTableOperationType.GetAllDataFromTable, true);
            }
        }
    }
}
