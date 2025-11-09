using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.Tests.Core.Compare;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Iterators;

/// <summary>
/// Class for unit testing <see cref="VelocipedeAsyncForeachIterator"/>.
/// </summary>
public sealed class VelocipedeAsyncForeachIteratorTest : BaseVelocipedeIteratorTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public VelocipedeAsyncForeachIteratorTest() : base()
    {
    }

    [Fact]
    public void Constructor_NullVelocipedeConnection_ThrowsArgumentNullException()
    {
        // Arrange.
        IVelocipedeDbConnection? connection = NullVelocipedeConnection;
        List<string> tableNames = TableNames;
#nullable disable
        Func<VelocipedeAsyncForeachIterator> act = () => new VelocipedeAsyncForeachIterator(connection, tableNames);
#nullable restore

        // Act & Assert.
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_NotConnectedVelocipedeConnection_ThrowsArgumentException()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetNotConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;
#nullable disable
        Func<VelocipedeAsyncForeachIterator> act = () => new VelocipedeAsyncForeachIterator(connection, tableNames);
#nullable restore

        // Act & Assert.
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NullTableNames_ThrowsArgumentException()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string>? tableNames = NullTableNames;
#nullable disable
        Func<VelocipedeAsyncForeachIterator> act = () => new VelocipedeAsyncForeachIterator(connection, tableNames);
#nullable restore

        // Act & Assert.
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_EmptyTableNames_ThrowsArgumentException()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = EmptyTableNames;
        Func<VelocipedeAsyncForeachIterator> act = () => new VelocipedeAsyncForeachIterator(connection, tableNames);

        // Act & Assert.
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddOperationBeforeBeginForeach_ThrowsInvalidOperationException()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act & Assert.
        IVelocipedeAsyncForeachIterator iterator = connection.WithAsyncForeachIterator(tableNames);
        var act = () => iterator.GetColumnsAsync();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddOperationAfterEndForeach_NoOperationsAddedBefore_ThrowsInvalidOperationException()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act & Assert.
        IVelocipedeAsyncForeachIterator iterator = connection.WithAsyncForeachIterator(tableNames);
        iterator.BeginAsyncForeach();
        iterator.EndAsyncForeach();
        var act = () => iterator.GetColumnsAsync();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddOperationAfterEndForeach_OperationAddedBefore_ThrowsInvalidOperationException()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act & Assert.
        IVelocipedeAsyncForeachIterator iterator = connection.WithAsyncForeachIterator(tableNames);
        iterator.BeginAsyncForeach();
        iterator.GetTriggersAsync();
        iterator.EndAsyncForeach();
        var act = () => iterator.GetColumnsAsync();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task GetResultAsync_EndForeachWasNotCalledBefore_ThrowsInvalidOperationException()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act & Assert.
        IVelocipedeAsyncForeachIterator iterator = connection.WithAsyncForeachIterator(tableNames);
        iterator.BeginAsyncForeach();
        iterator.GetTriggersAsync();
        Func<Task<VelocipedeForeachResult?>> act = async () => await iterator.GetResultAsync();
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetResultAsync_NoOperationsSpecified()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act.
        var foreachResult = await connection
            .WithAsyncForeachIterator(tableNames)
            .BeginAsyncForeach()
            .EndAsyncForeach()
            .GetResultAsync();

        // Assert.
        foreachResult.Should().BeNull();
    }

    [Fact]
    public async Task GetResultAsync_AllOperationsSpecified()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act.
        VelocipedeForeachResult? foreachResult = await connection
            .WithAsyncForeachIterator(tableNames)
            .BeginAsyncForeach()
                .GetAllDataAsync()
                .GetColumnsAsync()
                .GetForeignKeysAsync()
                .GetTriggersAsync()
                .GetSqlDefinitionAsync()
            .EndAsyncForeach()
            .GetResultAsync();

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
    public async Task GetResultAsync_ReopenForeachLoop()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act.
        VelocipedeForeachResult? foreachResult = await connection
            .WithAsyncForeachIterator(tableNames)
            .BeginAsyncForeach()
                .GetAllDataAsync()
                .GetColumnsAsync()
                .GetForeignKeysAsync()
                .GetTriggersAsync()
                .GetSqlDefinitionAsync()
            .EndAsyncForeach()
            .BeginAsyncForeach()
                .GetColumnsAsync()
                .GetTriggersAsync()
                .GetSqlDefinitionAsync()
            .EndAsyncForeach()
            .GetResultAsync();

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
    public async Task GetResultAsync_GetAllDataFromTable()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act.
        VelocipedeForeachResult? foreachResult = await connection
            .WithAsyncForeachIterator(tableNames)
            .BeginAsyncForeach()
                .GetAllDataAsync()
            .EndAsyncForeach()
            .GetResultAsync();

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
    public async Task GetResultAsync_GetColumns()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act.
        VelocipedeForeachResult? foreachResult = await connection
            .WithAsyncForeachIterator(tableNames)
            .BeginAsyncForeach()
                .GetColumnsAsync()
            .EndAsyncForeach()
            .GetResultAsync();

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
    public async Task GetResultAsync_GetForeignKeys()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act.
        VelocipedeForeachResult? foreachResult = await connection
            .WithAsyncForeachIterator(tableNames)
            .BeginAsyncForeach()
                .GetForeignKeysAsync()
            .EndAsyncForeach()
            .GetResultAsync();

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
    public async Task GetResultAsync_GetTriggers()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act.
        VelocipedeForeachResult? foreachResult = await connection
            .WithAsyncForeachIterator(tableNames)
            .BeginAsyncForeach()
                .GetTriggersAsync()
            .EndAsyncForeach()
            .GetResultAsync();

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
    public async Task GetResultAsync_GetSqlDefinition()
    {
        // Arrange.
        IVelocipedeDbConnection connection = GetConnectedVelocipedeConnection();
        List<string> tableNames = TableNames;

        // Act.
        VelocipedeForeachResult? foreachResult = await connection
            .WithAsyncForeachIterator(tableNames)
            .BeginAsyncForeach()
                .GetSqlDefinitionAsync()
            .EndAsyncForeach()
            .GetResultAsync();

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
