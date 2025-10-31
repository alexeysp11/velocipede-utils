using System.Data;
using System.Linq;
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
        private List<string> _tableNames;
        private VelocipedeForeachResult? _foreachResult;
        private bool _allowAddOperationTypes;
        private Dictionary<ForeachTableOperationType, bool> _operationTypes;

        public VelocipedeForeachTableIterator(IVelocipedeDbConnection connection, List<string> tableNames)
        {
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));
            if (!connection.IsConnected)
                throw new ArgumentException(ErrorMessageConstants.DatabaseShouldBeConnected, nameof(connection));

            ArgumentNullException.ThrowIfNull(tableNames, nameof(tableNames));
            if (tableNames.Count == 0)
                throw new ArgumentException(ErrorMessageConstants.ForeachRequiresNotEmptyTableNames, nameof(tableNames));

            _connection = connection;
            _tableNames = tableNames;
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

            _foreachResult = new VelocipedeForeachResult();
            IEnumerable<ForeachTableOperationType> operations = _operationTypes
                .Where(x => x.Value == true)
                .Select(x => x.Key);
            foreach (string tableName in _tableNames)
            {
                VelocipedeForeachTableInfo tableInfo = new() { TableName = tableName };
                foreach (ForeachTableOperationType operation in operations)
                {
                    switch (operation)
                    {
                        case ForeachTableOperationType.GetAllDataFromTable:
                            _connection.GetAllDataFromTable(tableName, out DataTable? dataTable);
                            tableInfo.Data = dataTable;
                            break;

                        case ForeachTableOperationType.GetColumns:
                            _connection.GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo);
                            tableInfo.ColumnInfo = columnInfo;
                            break;

                        case ForeachTableOperationType.GetForeignKeys:
                            _connection.GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo>? foreignKeyInfo);
                            tableInfo.ForeignKeyInfo = foreignKeyInfo;
                            break;

                        case ForeachTableOperationType.GetTriggers:
                            _connection.GetTriggers(tableName, out List<VelocipedeTriggerInfo>? triggerInfo);
                            tableInfo.TriggerInfo = triggerInfo;
                            break;

                        case ForeachTableOperationType.GetSqlDefinition:
                            _connection.GetSqlDefinition(tableName, out string? sqlDefinition);
                            tableInfo.SqlDefinition = sqlDefinition;
                            break;

                        default:
                            break;
                    }
                }
                _foreachResult.Add(tableName, tableInfo);
            }

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
                throw new InvalidOperationException(ErrorMessageConstants.UnableToAddActionForClosedForeachOperation);
            
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
