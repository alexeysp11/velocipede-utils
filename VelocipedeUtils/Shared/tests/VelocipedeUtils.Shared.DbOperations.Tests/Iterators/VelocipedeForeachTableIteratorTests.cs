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
        public void GetValidResultInfo()
        {
            // Arrange.
            IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
            List<string> tableNames = GetTableNames();

            // Act.
            connection
                .ForeachTable(tableNames)
                    .GetAllDataFromTable()
                .EndForeach()
                .GetForeachResult(out VelocipedeForeachResult? foreachResult);

            // Assert.
            foreachResult.Should().NotBeNull();
        }

        /// <summary>
        /// Get null object instead of <see cref="IVelocipedeDbConnection"/>.
        /// </summary>
        /// <returns><c>null</c></returns>
        private IVelocipedeDbConnection? GetNullVelocipedeConnection()
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
            // Tables.
            List<string> tableNames = GetTableNames();
            DataTable tableList1 = _tableList1.ToDataTable();
            DataTable tableList2 = _tableList2.ToDataTable();
            DataTable tableList3 = _tableList3.ToDataTable();

            // Mock.
            var mockConnection = new Mock<IVelocipedeDbConnection>();

            // Properties.
            mockConnection
                .Setup(x => x.IsConnected)
                .Returns(isConnected);

            // GetAllDataFromTable.
            mockConnection
                .Setup(x => x.QueryDataTable($"SELECT * FROM {TABLE_NAME_1}", out tableList1))
                .Returns(mockConnection.Object);
            mockConnection
                .Setup(x => x.QueryDataTable($"SELECT * FROM {TABLE_NAME_2}", out tableList2))
                .Returns(mockConnection.Object);
            mockConnection
                .Setup(x => x.QueryDataTable($"SELECT * FROM {TABLE_NAME_3}", out tableList2))
                .Returns(mockConnection.Object);

            // ForeachTable.
            mockConnection
                .Setup(x => x.ForeachTable(tableNames))
                .Returns(new VelocipedeForeachTableIterator(mockConnection.Object, tableNames));

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
