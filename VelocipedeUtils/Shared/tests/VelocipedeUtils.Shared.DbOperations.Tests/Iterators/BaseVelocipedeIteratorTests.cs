using System.Data;
using Moq;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Iterators;

/// <summary>
/// Base class for unit testing iterators.
/// </summary>
public abstract class BaseVelocipedeIteratorTests
{
    /// <summary>
    /// Name of the test table 1.
    /// </summary>
    protected const string TABLE_NAME_1 = "TableName1";

    /// <summary>
    /// Name of the test table 2.
    /// </summary>
    protected const string TABLE_NAME_2 = "TableName2";

    /// <summary>
    /// Name of the test table 3.
    /// </summary>
    protected const string TABLE_NAME_3 = "TableName3";

    /// <summary>
    /// SQL definition of the test table 1.
    /// </summary>
    protected const string TABLE_SQL_DEFINITION_1 = "CREATE TABLE TableName1 ...";

    /// <summary>
    /// SQL definition of the test table 2.
    /// </summary>
    protected const string TABLE_SQL_DEFINITION_2 = "CREATE TABLE TableName2 ...";

    /// <summary>
    /// SQL definition of the test table 3.
    /// </summary>
    protected const string TABLE_SQL_DEFINITION_3 = "CREATE TABLE TableName3 ...";

    /// <summary>
    /// List that contains records for table 1.
    /// </summary>
    protected readonly List<Table1> _tableList1;

    /// <summary>
    /// List that contains records for table 2.
    /// </summary>
    protected readonly List<Table2> _tableList2;

    /// <summary>
    /// List that contains records for table 3.
    /// </summary>
    protected readonly List<Table3> _tableList3;

    /// <summary>
    /// Columns that the table 1 contains.
    /// </summary>
    protected readonly List<VelocipedeNativeColumnInfo> _tableColumns1;

    /// <summary>
    /// Columns that the table 2 contains.
    /// </summary>
    protected readonly List<VelocipedeNativeColumnInfo> _tableColumns2;

    /// <summary>
    /// Columns that the table 3 contains.
    /// </summary>
    protected readonly List<VelocipedeNativeColumnInfo> _tableColumns3;

    /// <summary>
    /// Foreign keys that the table 3 contains.
    /// </summary>
    protected readonly List<VelocipedeForeignKeyInfo> _tableForeignKeys3;

    /// <summary>
    /// Triggers that the table 1 contains.
    /// </summary>
    protected readonly List<VelocipedeTriggerInfo> _tableTriggers1;

    /// <summary>
    /// Definition of table 1.
    /// </summary>
    protected sealed class Table1
    {
        /// <summary>
        /// Identifier of the test instance.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// Name of the test instance.
        /// </summary>
        public required string Name { get; set; }
    }

    /// <summary>
    /// Definition of table 2.
    /// </summary>
    protected sealed class Table2
    {
        /// <summary>
        /// Name of the test instance.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Value of the test instance.
        /// </summary>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Definition of table 3.
    /// </summary>
    protected sealed class Table3
    {
        /// <summary>
        /// Identifier of the test instance.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// Name of the test instance.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Value of the test instance.
        /// </summary>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    protected BaseVelocipedeIteratorTests()
    {
        // Table lists.
        _tableList1 =
        [
            new() { Id = 1, Name = "Name1" },
            new() { Id = 2, Name = "Name2" },
            new() { Id = 3, Name = "Name3" },
        ];
        _tableList2 =
        [
            new() { Name = "Name1", Value = "Value1" },
            new() { Name = "Name2", Value = null },
            new() { Name = "Name3", Value = null },
            new() { Name = "Name4", Value = "Value4" },
            new() { Name = "Name5", Value = "Value5" },
        ];
        _tableList3 =
        [
            new() { Id = 1, Name = "Name1", Value = "Value1" },
            new() { Id = 2, Name = "Name2", Value = null },
            new() { Id = 3, Name = "Name3", Value = null },
            new() { Id = 4, Name = "Name4", Value = "Value4" },
            new() { Id = 5, Name = "Name5", Value = "Value5" },
        ];

        // Table columns.
        _tableColumns1 =
        [
            new() { ColumnName = "Id", NativeColumnType = "int" },
            new() { ColumnName = "Name", NativeColumnType = "varchar(50)" },
        ];
        _tableColumns2 =
        [
            new() { ColumnName = "Name", NativeColumnType = "varchar(50)" },
            new() { ColumnName = "Value", NativeColumnType = "varchar(50)" },
        ];
        _tableColumns3 =
        [
            new() { ColumnName = "Id", NativeColumnType = "int" },
            new() { ColumnName = "Name", NativeColumnType = "varchar(50)" },
            new() { ColumnName = "Value", NativeColumnType = "varchar(50)" },
        ];

        // Foreign keys.
        _tableForeignKeys3 = [new() { ForeignKeyId = 1, ConstraintName = "FakeForeignKey.Table3", FromColumn = "FromColumn", ToColumn = "ToColumn" }];

        // Triggers.
        _tableTriggers1 = [new() { TriggerName = "Fake trigger.Table1", TriggerSchema = "FakeTriggerSchema", TriggerCatalog = "FakeTriggerCatalog", SqlDefinition = "FAKE TRIGGER SQL", DateCreated = DateTime.UtcNow }];
    }

    /// <summary>
    /// Gets null object instead of <see cref="IVelocipedeDbConnection"/>.
    /// </summary>
    /// <returns><c>null</c></returns>
    protected static IVelocipedeDbConnection? NullVelocipedeConnection
        => null;

    /// <summary>
    /// Get mock of <see cref="IVelocipedeDbConnection"/> that is not connected to database.
    /// </summary>
    /// <returns>Mock of <see cref="IVelocipedeDbConnection"/></returns>
    protected IVelocipedeDbConnection GetNotConnectedVelocipedeConnection()
        => GetVelocipedeConnection(isConnected: false);

    /// <summary>
    /// Get mock of <see cref="IVelocipedeDbConnection"/> that is connected to database.
    /// </summary>
    /// <returns>Mock of <see cref="IVelocipedeDbConnection"/></returns>
    protected IVelocipedeDbConnection GetConnectedVelocipedeConnection()
        => GetVelocipedeConnection(isConnected: true);

    /// <summary>
    /// Get mock of <see cref="IVelocipedeDbConnection"/> with the specified parameters.
    /// </summary>
    /// <param name="isConnected">Whether <see cref="IVelocipedeDbConnection"/> is connected to database</param>
    /// <returns>Mock object of <see cref="IVelocipedeDbConnection"/></returns>
    protected IVelocipedeDbConnection GetVelocipedeConnection(bool isConnected = false)
    {
        // Mock.
        var mockConnection = new Mock<IVelocipedeDbConnection>();

        // Properties.
        mockConnection.Setup(x => x.IsConnected).Returns(isConnected);
        mockConnection.Setup(x => x.DatabaseType).Returns(Enums.VelocipedeDatabaseType.InMemory);

        // Data.
        DataTable table1 = _tableList1.ToDataTable();
        DataTable table2 = _tableList2.ToDataTable();
        DataTable table3 = _tableList3.ToDataTable();
        mockConnection
            .Setup(x => x.QueryDataTable($"SELECT * FROM {TABLE_NAME_1}", out table1))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.QueryDataTable($"SELECT * FROM {TABLE_NAME_2}", out table2))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.QueryDataTable($"SELECT * FROM {TABLE_NAME_3}", out table3))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.QueryDataTableAsync($"SELECT * FROM {TABLE_NAME_1}"))
            .ReturnsAsync(table1);
        mockConnection
            .Setup(x => x.QueryDataTableAsync($"SELECT * FROM {TABLE_NAME_2}"))
            .ReturnsAsync(table2);
        mockConnection
            .Setup(x => x.QueryDataTableAsync($"SELECT * FROM {TABLE_NAME_3}"))
            .ReturnsAsync(table3);

        // Columns.
        List<VelocipedeNativeColumnInfo> columns1 = _tableColumns1;
        List<VelocipedeNativeColumnInfo> columns2 = _tableColumns2;
        List<VelocipedeNativeColumnInfo> columns3 = _tableColumns3;
        mockConnection
            .Setup(x => x.GetColumns(TABLE_NAME_1, out columns1))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetColumns(TABLE_NAME_2, out columns2))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetColumns(TABLE_NAME_3, out columns3))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetColumnsAsync(TABLE_NAME_1))
            .ReturnsAsync(columns1);
        mockConnection
            .Setup(x => x.GetColumnsAsync(TABLE_NAME_2))
            .ReturnsAsync(columns2);
        mockConnection
            .Setup(x => x.GetColumnsAsync(TABLE_NAME_3))
            .ReturnsAsync(columns3);

        // Foreign keys.
        List<VelocipedeForeignKeyInfo>? nullForeignKeys = null;
        List<VelocipedeForeignKeyInfo> foreignKeys3 = _tableForeignKeys3;
        mockConnection
            .Setup(x => x.GetForeignKeys(TABLE_NAME_1, out nullForeignKeys))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetForeignKeys(TABLE_NAME_2, out nullForeignKeys))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetForeignKeys(TABLE_NAME_3, out foreignKeys3))
            .Returns(mockConnection.Object);
#nullable disable
        mockConnection
            .Setup(x => x.GetForeignKeysAsync(TABLE_NAME_1))
            .ReturnsAsync(nullForeignKeys);
        mockConnection
            .Setup(x => x.GetForeignKeysAsync(TABLE_NAME_2))
            .ReturnsAsync(nullForeignKeys);
#nullable restore
        mockConnection
            .Setup(x => x.GetForeignKeysAsync(TABLE_NAME_3))
            .ReturnsAsync(foreignKeys3);

        // Triggers.
        List<VelocipedeTriggerInfo> triggers1 = _tableTriggers1;
        List<VelocipedeTriggerInfo>? nullTriggers = null;
        mockConnection
            .Setup(x => x.GetTriggers(TABLE_NAME_1, out triggers1))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetTriggers(TABLE_NAME_2, out nullTriggers))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetTriggers(TABLE_NAME_3, out nullTriggers))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetTriggersAsync(TABLE_NAME_1))
            .ReturnsAsync(triggers1);
#nullable disable
        mockConnection
            .Setup(x => x.GetTriggersAsync(TABLE_NAME_2))
            .ReturnsAsync(nullTriggers);
        mockConnection
            .Setup(x => x.GetTriggersAsync(TABLE_NAME_3))
            .ReturnsAsync(nullTriggers);
#nullable restore

        // SQL definition.
        string? sqlDefinition1 = TABLE_SQL_DEFINITION_1;
        string? sqlDefinition2 = TABLE_SQL_DEFINITION_2;
        string? sqlDefinition3 = TABLE_SQL_DEFINITION_3;
        mockConnection
            .Setup(x => x.GetSqlDefinition(TABLE_NAME_1, out sqlDefinition1))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetSqlDefinition(TABLE_NAME_2, out sqlDefinition2))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetSqlDefinition(TABLE_NAME_3, out sqlDefinition3))
            .Returns(mockConnection.Object);
        mockConnection
            .Setup(x => x.GetSqlDefinitionAsync(TABLE_NAME_1))
            .ReturnsAsync(sqlDefinition1);
        mockConnection
            .Setup(x => x.GetSqlDefinitionAsync(TABLE_NAME_2))
            .ReturnsAsync(sqlDefinition2);
        mockConnection
            .Setup(x => x.GetSqlDefinitionAsync(TABLE_NAME_3))
            .ReturnsAsync(sqlDefinition3);

        // Fluent interfaces for connected object.
        if (isConnected)
        {
            // Iterators.
            List<string> tableNames = TableNames;
            mockConnection
                .Setup(x => x.WithForeachTableIterator(tableNames))
                .Returns(new VelocipedeForeachTableIterator(mockConnection.Object, tableNames));
            mockConnection
                .Setup(x => x.WithAsyncForeachIterator(tableNames))
                .Returns(new VelocipedeAsyncForeachIterator(mockConnection.Object, tableNames));
        }

        return mockConnection.Object;
    }

    /// <summary>
    /// Gets <c>null</c> instead of list of table names.
    /// </summary>
    /// <returns><c>null</c></returns>
    protected static List<string>? NullTableNames => null;

    /// <summary>
    /// Gets empty list of table names.
    /// </summary>
    /// <returns><see cref="List{T}"/> of <see cref="string"/> that contains no elements</returns>
    protected static List<string> EmptyTableNames => [];

    /// <summary>
    /// Gets list of table names.
    /// </summary>
    /// <returns><see cref="List{T}"/> of <see cref="string"/> that contains the specified table names</returns>
    protected static List<string> TableNames => [TABLE_NAME_1, TABLE_NAME_2, TABLE_NAME_3];
}
