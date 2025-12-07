using System.Data;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Models.Loops;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.Iterators;

/// <summary>
/// Iterator for the <c>foreach</c> operation.
/// </summary>
public sealed class VelocipedeForeachTableIterator : IVelocipedeForeachTableIterator
{
    /// <summary>
    /// Type of the <c>foreach</c> operation for the table.
    /// </summary>
    private enum ForeachTableOperationType
    {
        /// <summary>
        /// Get all data from table.
        /// </summary>
        GetAllData,

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

    private readonly IVelocipedeDbConnection _connection;
    private readonly List<string> _tableNames;
    private readonly Dictionary<ForeachTableOperationType, bool> _operationTypes;
    private bool _allowAddOperationTypes;

    /// <summary>
    /// Initializes a new instance of the <see cref="VelocipedeForeachTableIterator"/> class with specified parameters.
    /// </summary>
    /// <param name="connection">Instance of <see cref="IVelocipedeDbConnection"/> that is connected to database.</param>
    /// <param name="tableNames"><see cref="List{T}"/> of <see cref="string"/> that contains valid table names.</param>
    /// <exception cref="ArgumentNullException">When <c>connection</c>, or <c>tableNames</c> is null.</exception>
    /// <exception cref="ArgumentException">When <c>connection.IsConnected</c> equals to <c>false</c>, or <c>tableNames.Count == 0</c>.</exception>
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
        _allowAddOperationTypes = false;
        _operationTypes = [];
    }

    /// <inheritdoc/>
    public IVelocipedeForeachTableIterator BeginForeach()
    {
        _operationTypes.Clear();
        _allowAddOperationTypes = true;
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeForeachTableIterator EndForeach()
    {
        _allowAddOperationTypes = false;
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeDbConnection GetForeachResult(out VelocipedeForeachResult? foreachResult)
    {
        if (_allowAddOperationTypes)
            throw new InvalidOperationException(ErrorMessageConstants.UnableToGetResultForOpenForeachOperation);

        // Get active operations.
        IEnumerable<ForeachTableOperationType> operations = _operationTypes
            .Where(x => x.Value == true)
            .Select(x => x.Key);

        // If there is no active operations, then return null.
        if (!operations.Any())
        {
            foreachResult = null;
            return _connection;
        }

        // Get foreach result in the loop.
        foreachResult = new VelocipedeForeachResult();
        foreach (string tableName in _tableNames)
        {
            VelocipedeForeachTableInfo tableInfo = new() { TableName = tableName };
            foreach (ForeachTableOperationType operation in operations)
            {
                switch (operation)
                {
                    case ForeachTableOperationType.GetAllData:
                        _connection.GetAllData(tableName, out DataTable? dataTable);
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
            foreachResult.Add(tableName, tableInfo);
        }

        return _connection;
    }

    /// <inheritdoc/>
    public IVelocipedeForeachTableIterator GetAllData()
    {
        TryAddOperationType(ForeachTableOperationType.GetAllData);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeForeachTableIterator GetColumns()
    {
        TryAddOperationType(ForeachTableOperationType.GetColumns);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeForeachTableIterator GetForeignKeys()
    {
        TryAddOperationType(ForeachTableOperationType.GetForeignKeys);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeForeachTableIterator GetSqlDefinition()
    {
        TryAddOperationType(ForeachTableOperationType.GetSqlDefinition);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeForeachTableIterator GetTriggers()
    {
        TryAddOperationType(ForeachTableOperationType.GetTriggers);
        return this;
    }

    /// <summary>
    /// Try to add <see cref="ForeachTableOperationType"/>.
    /// </summary>
    /// <param name="operationType">Operation type</param>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="EndForeach"/> was already called</exception>
    private void TryAddOperationType(ForeachTableOperationType operationType)
    {
        if (!_allowAddOperationTypes)
            throw new InvalidOperationException(ErrorMessageConstants.UnableToAddActionForClosedForeachOperation);
        
        if (!_operationTypes.TryAdd(operationType, true))
        {
            _operationTypes[operationType] = true;
        }
    }
}
