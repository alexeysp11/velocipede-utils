using System.Data;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Iterators;

/// <summary>
/// Iterator for the asynchronous <c>foreach</c> operation.
/// </summary>
public sealed class VelocipedeAsyncForeachIterator : IVelocipedeAsyncForeachIterator
{
    /// <summary>
    /// Type of the <c>foreach</c> operation for the table.
    /// </summary>
    private enum AsyncForeachOperationType
    {
        /// <summary>
        /// Asynchronously get all data from table.
        /// </summary>
        GetAllDataAsync,

        /// <summary>
        /// Asynchronously get columns from table.
        /// </summary>
        GetColumnsAsync,

        /// <summary>
        /// Asynchronously get all foreign keys from table.
        /// </summary>
        GetForeignKeysAsync,

        /// <summary>
        /// Asynchronously get triggers from table.
        /// </summary>
        GetTriggersAsync,

        /// <summary>
        /// Asynchronously get SQL definition from table.
        /// </summary>
        GetSqlDefinitionAsync,
    }

    private readonly IVelocipedeDbConnection _connection;
    private readonly List<string> _tableNames;
    private readonly Dictionary<AsyncForeachOperationType, bool> _operationTypes;
    private bool _allowAddOperationTypes;

    /// <summary>
    /// Initializes a new instance of the <see cref="VelocipedeAsyncForeachIterator"/> class with specified parameters.
    /// </summary>
    /// <param name="connection">Instance of <see cref="IVelocipedeDbConnection"/> that is connected to database.</param>
    /// <param name="tableNames"><see cref="List{T}"/> of <see cref="string"/> that contains valid table names.</param>
    /// <exception cref="ArgumentNullException">When <c>connection</c>, or <c>tableNames</c> is null.</exception>
    /// <exception cref="ArgumentException">When <c>connection.IsConnected</c> equals to <c>false</c>, or <c>tableNames.Count == 0</c>.</exception>
    public VelocipedeAsyncForeachIterator(IVelocipedeDbConnection connection, List<string> tableNames)
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
    public IVelocipedeAsyncForeachIterator BeginAsyncForeach()
    {
        _operationTypes.Clear();
        _allowAddOperationTypes = true;
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeAsyncForeachIterator EndAsyncForeach()
    {
        _allowAddOperationTypes = false;
        return this;
    }

    /// <inheritdoc/>
    public async Task<VelocipedeForeachResult?> GetResultAsync()
    {
        if (_allowAddOperationTypes)
            throw new InvalidOperationException(ErrorMessageConstants.UnableToGetResultForOpenForeachOperation);

        // Get active operations.
        IEnumerable<AsyncForeachOperationType> operations = _operationTypes
            .Where(x => x.Value == true)
            .Select(x => x.Key);

        // If there is no active operations, then return null.
        if (!operations.Any())
        {
            return null;
        }

        // Get foreach result in the loop.
        object lockObject = new();
        List<Task> tasks = [];
        VelocipedeForeachResult foreachResult = new();
        foreach (string tableName in _tableNames)
        {
            Task task = Task.Run(async () =>
            {
                VelocipedeForeachTableInfo tableInfo = new() { TableName = tableName };
                foreach (AsyncForeachOperationType operation in operations)
                {
                    switch (operation)
                    {
                        case AsyncForeachOperationType.GetAllDataAsync:
                            tableInfo.Data = await _connection.GetAllDataAsync(tableName);
                            break;

                        case AsyncForeachOperationType.GetColumnsAsync:
                            tableInfo.ColumnInfo = await _connection.GetColumnsAsync(tableName);
                            break;

                        case AsyncForeachOperationType.GetForeignKeysAsync:
                            tableInfo.ForeignKeyInfo = await _connection.GetForeignKeysAsync(tableName);
                            break;

                        case AsyncForeachOperationType.GetTriggersAsync:
                            tableInfo.TriggerInfo = await _connection.GetTriggersAsync(tableName);
                            break;

                        case AsyncForeachOperationType.GetSqlDefinitionAsync:
                            tableInfo.SqlDefinition = await _connection.GetSqlDefinitionAsync(tableName);
                            break;

                        default:
                            break;
                    }
                }
                lock (lockObject)
                {
                    foreachResult.Add(tableName, tableInfo);
                }
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks.ToArray());
        return foreachResult;
    }

    /// <inheritdoc/>
    public IVelocipedeAsyncForeachIterator GetAllDataAsync()
    {
        TryAddOperationType(AsyncForeachOperationType.GetAllDataAsync);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeAsyncForeachIterator GetColumnsAsync()
    {
        TryAddOperationType(AsyncForeachOperationType.GetColumnsAsync);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeAsyncForeachIterator GetForeignKeysAsync()
    {
        TryAddOperationType(AsyncForeachOperationType.GetForeignKeysAsync);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeAsyncForeachIterator GetSqlDefinitionAsync()
    {
        TryAddOperationType(AsyncForeachOperationType.GetSqlDefinitionAsync);
        return this;
    }

    /// <inheritdoc/>
    public IVelocipedeAsyncForeachIterator GetTriggersAsync()
    {
        TryAddOperationType(AsyncForeachOperationType.GetTriggersAsync);
        return this;
    }

    /// <summary>
    /// Try to add <see cref="AsyncForeachOperationType"/>.
    /// </summary>
    /// <param name="operationType">Operation type</param>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="EndForeach"/> was already called</exception>
    private void TryAddOperationType(AsyncForeachOperationType operationType)
    {
        if (!_allowAddOperationTypes)
            throw new InvalidOperationException(ErrorMessageConstants.UnableToAddActionForClosedForeachOperation);

        if (!_operationTypes.TryAdd(operationType, true))
        {
            _operationTypes[operationType] = true;
        }
    }
}
