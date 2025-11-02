using System.Data;
using FluentAssertions;
using Moq;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Iterators
{
    /// <summary>
    /// Class for unit testing <see cref="VelocipedeForeachTableIterator"/>.
    /// </summary>
    public sealed class VelocipedeForeachTableIteratorTests
    {
        private const string TABLE_NAME_1 = "TableName1";
        private const string TABLE_NAME_2 = "TableName2";
        private const string TABLE_NAME_3 = "TableName3";

        private const string TABLE_SQL_DEFINITION_1 = "CREATE TABLE TableName1 ...";
        private const string TABLE_SQL_DEFINITION_2 = "CREATE TABLE TableName2 ...";
        private const string TABLE_SQL_DEFINITION_3 = "CREATE TABLE TableName3 ...";

        /// <summary>
        /// List that contains records for table 1.
        /// </summary>
        private readonly List<Table1> _tableList1;

        /// <summary>
        /// List that contains records for table 2.
        /// </summary>
        private readonly List<Table2> _tableList2;

        /// <summary>
        /// List that contains records for table 3.
        /// </summary>
        private readonly List<Table3> _tableList3;

        /// <summary>
        /// Columns that the table 1 contains.
        /// </summary>
        private readonly List<VelocipedeColumnInfo> _tableColumns1;

        /// <summary>
        /// Columns that the table 2 contains.
        /// </summary>
        private readonly List<VelocipedeColumnInfo> _tableColumns2;

        /// <summary>
        /// Columns that the table 3 contains.
        /// </summary>
        private readonly List<VelocipedeColumnInfo> _tableColumns3;

        /// <summary>
        /// Foreign keys that the table 3 contains.
        /// </summary>
        private readonly List<VelocipedeForeignKeyInfo> _tableForeignKeys3;

        /// <summary>
        /// Triggers that the table 1 contains.
        /// </summary>
        private readonly List<VelocipedeTriggerInfo> _tableTriggers1;

        /// <summary>
        /// Definition of table 1.
        /// </summary>
        private sealed class Table1
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
        }

        /// <summary>
        /// Definition of table 2.
        /// </summary>
        private sealed class Table2
        {
            public required string Name { get; set; }
            public string? Value { get; set; }
        }

        /// <summary>
        /// Definition of table 3.
        /// </summary>
        private sealed class Table3
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
            public string? Value { get; set; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VelocipedeForeachTableIteratorTests()
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
                new() { ColumnName = "Id", ColumnType = "int" },
                new() { ColumnName = "Name", ColumnType = "varchar(50)" },
            ];
            _tableColumns2 =
            [
                new() { ColumnName = "Name", ColumnType = "varchar(50)" },
                new() { ColumnName = "Value", ColumnType = "varchar(50)" },
            ];
            _tableColumns3 =
            [
                new() { ColumnName = "Id", ColumnType = "int" },
                new() { ColumnName = "Name", ColumnType = "varchar(50)" },
                new() { ColumnName = "Value", ColumnType = "varchar(50)" },
            ];

            // Foreign keys.
            _tableForeignKeys3 = [new() { ForeignKeyId = 1, ConstraintName = "FakeForeignKey", FromColumn = "FromColumn", ToColumn = "ToColumn" }];

            // Triggers.
            _tableTriggers1 = [new() { TriggerName = "Fake trigger", TriggerSchema = "FakeTriggerSchema", TriggerCatalog = "FakeTriggerCatalog", SqlDefinition = "FAKE TRIGGER SQL", DateCreated = DateTime.UtcNow }];
        }

        [Fact]
        public void Constructor_NullVelocipedeConnection_ThrowsArgumentNullException()
        {
            // Arrange.
            IVelocipedeDbConnection? connection = GetNullVelocipedeConnection();
            List<string> tableNames = GetTableNames();
#nullable disable
            Func<VelocipedeForeachTableIterator> act = () => new VelocipedeForeachTableIterator(connection, tableNames);
#nullable restore

            // Act & Assert.
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_NotConnectedVelocipedeConnection_ThrowsArgumentException()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetNotConnectedVelocipedeConnection();
            List<string> tableNames = GetTableNames();
#nullable disable
            Func<VelocipedeForeachTableIterator> act = () => new VelocipedeForeachTableIterator(connection, tableNames);
#nullable restore

            // Act & Assert.
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_NullTableNames_ThrowsArgumentException()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string>? tableNames = GetNullTableNames();
#nullable disable
            Func<VelocipedeForeachTableIterator> act = () => new VelocipedeForeachTableIterator(connection, tableNames);
#nullable restore

            // Act & Assert.
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_EmptyTableNames_ThrowsArgumentException()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string> tableNames = GetEmptyTableNames();
            Func<VelocipedeForeachTableIterator> act = () => new VelocipedeForeachTableIterator(connection, tableNames);

            // Act & Assert.
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddOperationBeforeBeginForeach_ThrowsInvalidOperationException()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string> tableNames = GetTableNames();

            // Act & Assert.
            IVelocipedeForeachTableIterator iterator = connection.WithForeachTableIterator(tableNames);
            var act = () => iterator.GetColumns();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddOperationAfterEndForeach_NoOperationsAddedBefore_ThrowsInvalidOperationException()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string> tableNames = GetTableNames();

            // Act & Assert.
            IVelocipedeForeachTableIterator iterator = connection.WithForeachTableIterator(tableNames);
            iterator.BeginForeach();
            iterator.EndForeach();
            var act = () => iterator.GetColumns();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddOperationAfterEndForeach_OperationAddedBefore_ThrowsInvalidOperationException()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string> tableNames = GetTableNames();

            // Act & Assert.
            IVelocipedeForeachTableIterator iterator = connection.WithForeachTableIterator(tableNames);
            iterator.BeginForeach();
            iterator.GetTriggers();
            iterator.EndForeach();
            var act = () => iterator.GetColumns();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetForeachResult_EndForeachWasNotCalledBefore_ThrowsInvalidOperationException()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string> tableNames = GetTableNames();

            // Act & Assert.
            IVelocipedeForeachTableIterator iterator = connection.WithForeachTableIterator(tableNames);
            iterator.BeginForeach();
            iterator.GetTriggers();
            var act = () => iterator.GetForeachResult(out _);
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetForeachResult_NoOperationsSpecified()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string> tableNames = GetTableNames();

            // Act.
            connection
                .WithForeachTableIterator(tableNames)
                .BeginForeach()
                .EndForeach()
                .GetForeachResult(out VelocipedeForeachResult? foreachResult);

            // Assert.
            foreachResult.Should().BeNull();
        }

        [Fact]
        public void GetForeachResult_AllOperationsSpecified()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string> tableNames = GetTableNames();

            // Act.
            connection
                .WithForeachTableIterator(tableNames)
                .BeginForeach()
                    .GetAllDataFromTable()
                    .GetColumns()
                    .GetForeignKeys()
                    .GetTriggers()
                    .GetSqlDefinition()
                .EndForeach()
                .GetForeachResult(out VelocipedeForeachResult? foreachResult);

            // Assert.
            foreachResult.Should().NotBeNull();
        }

        /// <summary>
        /// Get null object instead of <see cref="IVelocipedeDbConnection"/>.
        /// </summary>
        /// <returns><c>null</c></returns>
        private static IVelocipedeDbConnection? GetNullVelocipedeConnection()
            => null;

        /// <summary>
        /// Get mock of <see cref="IVelocipedeDbConnection"/> that is not connected to database.
        /// </summary>
        /// <returns>Mock of <see cref="IVelocipedeDbConnection"/></returns>
        private IVelocipedeDbConnection GetNotConnectedVelocipedeConnection()
            => GetVelocipedeConnection(isConnected: false);

        /// <summary>
        /// Get mock of <see cref="IVelocipedeDbConnection"/> that is connected to database.
        /// </summary>
        /// <returns>Mock of <see cref="IVelocipedeDbConnection"/></returns>
        private IVelocipedeDbConnection GetConnectedVelocipedeConnection()
            => GetVelocipedeConnection(isConnected: true);

        /// <summary>
        /// Get mock of <see cref="IVelocipedeDbConnection"/> with the specified parameters.
        /// </summary>
        /// <param name="isConnected">Whether <see cref="IVelocipedeDbConnection"/> is connected to database</param>
        /// <returns>Mock object of <see cref="IVelocipedeDbConnection"/></returns>
        private IVelocipedeDbConnection GetVelocipedeConnection(bool isConnected = false)
        {
            // Mock.
            var mockConnection = new Mock<IVelocipedeDbConnection>();

            // Properties.
            mockConnection
                .Setup(x => x.IsConnected)
                .Returns(isConnected);

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

            // Columns.
            List<VelocipedeColumnInfo> columns1 = _tableColumns1;
            List<VelocipedeColumnInfo> columns2 = _tableColumns2;
            List<VelocipedeColumnInfo> columns3 = _tableColumns3;
            mockConnection
                .Setup(x => x.GetColumns(TABLE_NAME_1, out columns1))
                .Returns(mockConnection.Object);
            mockConnection
                .Setup(x => x.GetColumns(TABLE_NAME_2, out columns2))
                .Returns(mockConnection.Object);
            mockConnection
                .Setup(x => x.GetColumns(TABLE_NAME_3, out columns3))
                .Returns(mockConnection.Object);

            // Foreign keys.
            List<VelocipedeForeignKeyInfo> foreignKeys3 = _tableForeignKeys3;
            mockConnection
                .Setup(x => x.GetForeignKeys(TABLE_NAME_3, out foreignKeys3))
                .Returns(mockConnection.Object);

            // Triggers.
            List<VelocipedeTriggerInfo> triggers1 = _tableTriggers1;
            mockConnection
                .Setup(x => x.GetTriggers(TABLE_NAME_1, out triggers1))
                .Returns(mockConnection.Object);

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

            // Fluent interfaces for connected object.
            if (isConnected)
            {
                // WithForeachTableIterator.
                List<string> tableNames = GetTableNames();
                mockConnection
                    .Setup(x => x.WithForeachTableIterator(tableNames))
                    .Returns(new VelocipedeForeachTableIterator(mockConnection.Object, tableNames));
            }

            return mockConnection.Object;
        }

        /// <summary>
        /// Get <c>null</c> instead of list of table names.
        /// </summary>
        /// <returns><c>null</c></returns>
        private static List<string>? GetNullTableNames() => null;

        /// <summary>
        /// Get empty list of table names.
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="string"/> that contains no elements</returns>
        private static List<string> GetEmptyTableNames() => [];

        /// <summary>
        /// Get list of table names.
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="string"/> that contains the specified table names</returns>
        private static List<string> GetTableNames() => [TABLE_NAME_1, TABLE_NAME_2, TABLE_NAME_3];
    }
}
