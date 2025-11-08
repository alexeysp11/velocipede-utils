using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.Tests.Core.Compare;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Iterators;

/// <summary>
/// Class for unit testing <see cref="VelocipedeForeachTableIterator"/>.
/// </summary>
public sealed class VelocipedeForeachTableIteratorTests : BaseVelocipedeIteratorTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public VelocipedeForeachTableIteratorTests() : base()
    {
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
                .GetAllData()
                .GetColumns()
                .GetForeignKeys()
                .GetTriggers()
                .GetSqlDefinition()
            .EndForeach()
            .GetForeachResult(out VelocipedeForeachResult? foreachResult);

        // Assert.
        foreachResult.Should().NotBeNull();
        foreachResult.Result.Should().NotBeNull();
        DataTableCompareHelper.AreDataTablesEquivalent(foreachResult.Result[TABLE_NAME_1].Data, _tableList1.ToDataTable());
        DataTableCompareHelper.AreDataTablesEquivalent(foreachResult.Result[TABLE_NAME_2].Data, _tableList2.ToDataTable());
        DataTableCompareHelper.AreDataTablesEquivalent(foreachResult.Result[TABLE_NAME_3].Data, _tableList3.ToDataTable());
        foreachResult.Result[TABLE_NAME_1].ColumnInfo.Should().BeEquivalentTo(_tableColumns1);
        foreachResult.Result[TABLE_NAME_2].ColumnInfo.Should().BeEquivalentTo(_tableColumns2);
        foreachResult.Result[TABLE_NAME_3].ColumnInfo.Should().BeEquivalentTo(_tableColumns3);
        foreachResult.Result[TABLE_NAME_1].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ForeignKeyInfo.Should().BeEquivalentTo(_tableForeignKeys3);
        foreachResult.Result[TABLE_NAME_1].TriggerInfo.Should().BeEquivalentTo(_tableTriggers1);
        foreachResult.Result[TABLE_NAME_2].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_1);
        foreachResult.Result[TABLE_NAME_2].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_2);
        foreachResult.Result[TABLE_NAME_3].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_3);
    }

    [Fact]
    public void GetForeachResult_ReopenForeachLoop()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = GetTableNames();

        // Act.
        connection
            .WithForeachTableIterator(tableNames)
            .BeginForeach()
                .GetAllData()
                .GetColumns()
                .GetForeignKeys()
                .GetTriggers()
                .GetSqlDefinition()
            .EndForeach()
            .BeginForeach()
                .GetColumns()
                .GetTriggers()
                .GetSqlDefinition()
            .EndForeach()
            .GetForeachResult(out VelocipedeForeachResult? foreachResult);

        // Assert.
        foreachResult.Should().NotBeNull();
        foreachResult.Result.Should().NotBeNull();
        foreachResult.Result[TABLE_NAME_1].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ColumnInfo.Should().BeEquivalentTo(_tableColumns1);
        foreachResult.Result[TABLE_NAME_2].ColumnInfo.Should().BeEquivalentTo(_tableColumns2);
        foreachResult.Result[TABLE_NAME_3].ColumnInfo.Should().BeEquivalentTo(_tableColumns3);
        foreachResult.Result[TABLE_NAME_1].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].TriggerInfo.Should().BeEquivalentTo(_tableTriggers1);
        foreachResult.Result[TABLE_NAME_2].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_1);
        foreachResult.Result[TABLE_NAME_2].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_2);
        foreachResult.Result[TABLE_NAME_3].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_3);
    }

    [Fact]
    public void GetForeachResult_GetAllDataFromTable()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = GetTableNames();

        // Act.
        connection
            .WithForeachTableIterator(tableNames)
            .BeginForeach()
                .GetAllData()
            .EndForeach()
            .GetForeachResult(out VelocipedeForeachResult? foreachResult);

        // Assert.
        foreachResult.Should().NotBeNull();
        foreachResult.Result.Should().NotBeNull();
        DataTableCompareHelper.AreDataTablesEquivalent(foreachResult.Result[TABLE_NAME_1].Data, _tableList1.ToDataTable());
        DataTableCompareHelper.AreDataTablesEquivalent(foreachResult.Result[TABLE_NAME_2].Data, _tableList2.ToDataTable());
        DataTableCompareHelper.AreDataTablesEquivalent(foreachResult.Result[TABLE_NAME_3].Data, _tableList3.ToDataTable());
        foreachResult.Result[TABLE_NAME_1].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].SqlDefinition.Should().BeNullOrEmpty();
        foreachResult.Result[TABLE_NAME_2].SqlDefinition.Should().BeNullOrEmpty();
        foreachResult.Result[TABLE_NAME_3].SqlDefinition.Should().BeNullOrEmpty();
    }

    [Fact]
    public void GetForeachResult_GetColumns()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = GetTableNames();

        // Act.
        connection
            .WithForeachTableIterator(tableNames)
            .BeginForeach()
                .GetColumns()
            .EndForeach()
            .GetForeachResult(out VelocipedeForeachResult? foreachResult);

        // Assert.
        foreachResult.Should().NotBeNull();
        foreachResult.Result.Should().NotBeNull();
        foreachResult.Result[TABLE_NAME_1].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ColumnInfo.Should().BeEquivalentTo(_tableColumns1);
        foreachResult.Result[TABLE_NAME_2].ColumnInfo.Should().BeEquivalentTo(_tableColumns2);
        foreachResult.Result[TABLE_NAME_3].ColumnInfo.Should().BeEquivalentTo(_tableColumns3);
        foreachResult.Result[TABLE_NAME_1].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].SqlDefinition.Should().BeNullOrEmpty();
        foreachResult.Result[TABLE_NAME_2].SqlDefinition.Should().BeNullOrEmpty();
        foreachResult.Result[TABLE_NAME_3].SqlDefinition.Should().BeNullOrEmpty();
    }

    [Fact]
    public void GetForeachResult_GetForeignKeys()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = GetTableNames();

        // Act.
        connection
            .WithForeachTableIterator(tableNames)
            .BeginForeach()
                .GetForeignKeys()
            .EndForeach()
            .GetForeachResult(out VelocipedeForeachResult? foreachResult);

        // Assert.
        foreachResult.Should().NotBeNull();
        foreachResult.Result.Should().NotBeNull();
        foreachResult.Result[TABLE_NAME_1].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ForeignKeyInfo.Should().BeEquivalentTo(_tableForeignKeys3);
        foreachResult.Result[TABLE_NAME_1].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].SqlDefinition.Should().BeNullOrEmpty();
        foreachResult.Result[TABLE_NAME_2].SqlDefinition.Should().BeNullOrEmpty();
        foreachResult.Result[TABLE_NAME_3].SqlDefinition.Should().BeNullOrEmpty();
    }

    [Fact]
    public void GetForeachResult_GetTriggers()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = GetTableNames();

        // Act.
        connection
            .WithForeachTableIterator(tableNames)
            .BeginForeach()
                .GetTriggers()
            .EndForeach()
            .GetForeachResult(out VelocipedeForeachResult? foreachResult);

        // Assert.
        foreachResult.Should().NotBeNull();
        foreachResult.Result.Should().NotBeNull();
        foreachResult.Result[TABLE_NAME_1].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].TriggerInfo.Should().BeEquivalentTo(_tableTriggers1);
        foreachResult.Result[TABLE_NAME_2].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].SqlDefinition.Should().BeNullOrEmpty();
        foreachResult.Result[TABLE_NAME_2].SqlDefinition.Should().BeNullOrEmpty();
        foreachResult.Result[TABLE_NAME_3].SqlDefinition.Should().BeNullOrEmpty();
    }

    [Fact]
    public void GetForeachResult_GetSqlDefinition()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = GetTableNames();

        // Act.
        connection
            .WithForeachTableIterator(tableNames)
            .BeginForeach()
                .GetSqlDefinition()
            .EndForeach()
            .GetForeachResult(out VelocipedeForeachResult? foreachResult);

        // Assert.
        foreachResult.Should().NotBeNull();
        foreachResult.Result.Should().NotBeNull();
        foreachResult.Result[TABLE_NAME_1].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].Data.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ColumnInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].ForeignKeyInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_2].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_3].TriggerInfo.Should().BeNull();
        foreachResult.Result[TABLE_NAME_1].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_1);
        foreachResult.Result[TABLE_NAME_2].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_2);
        foreachResult.Result[TABLE_NAME_3].SqlDefinition.Should().Be(TABLE_SQL_DEFINITION_3);
    }
}
