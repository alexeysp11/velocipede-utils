using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.Execute(string)"/>.
/// </summary>
public abstract class BaseExecuteTests : BaseDbConnectionTests
{
    protected string _createTableSqlForExecuteQuery;
    protected string _createTableSqlForExecuteAsyncQuery;
    protected string _createTableSqlForExecuteWithParamsQuery;
    protected string _createTableSqlForExecuteAsyncWithParamsQuery;

    protected BaseExecuteTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
        _createTableSqlForExecuteQuery = string.Empty;
        _createTableSqlForExecuteWithParamsQuery = string.Empty;
        _createTableSqlForExecuteAsyncQuery = string.Empty;
        _createTableSqlForExecuteAsyncWithParamsQuery = string.Empty;
    }

    [Fact]
    public void Execute_CreateTestTableForExecute()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string expected = dbConnection.DatabaseType switch
        {
            DatabaseType.PostgreSQL => "public.TestTableForExecute",
            _ => "TestTableForExecute",
        };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Execute(_createTableSqlForExecuteQuery);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.GetTablesInDb(out List<string> tables);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        tables.Should().Contain(expected);
    }

    [Fact]
    public void Execute_CreateTestTableForExecuteWithParams()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string expectedTable = dbConnection.DatabaseType switch
        {
            DatabaseType.PostgreSQL => "public.TestTableForExecuteWithParams",
            _ => "TestTableForExecuteWithParams",
        };
        const string expectedName = "Name_1";
        const string selectQuery = @"SELECT ""Name"" FROM ""TestTableForExecuteWithParams""";
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TestRecordName", Value = expectedName }];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Execute(_createTableSqlForExecuteWithParamsQuery, parameters);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.GetTablesInDb(out List<string> tables);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.Query(selectQuery, out List<string> names);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        tables.Should().Contain(expectedTable);
        names.Should().Contain(expectedName);
    }

    [Fact]
    public async Task ExecuteAsync_CreateTestTableForExecuteAsync()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string expected = dbConnection.DatabaseType switch
        {
            DatabaseType.PostgreSQL => "public.TestTableForExecuteAsync",
            _ => "TestTableForExecuteAsync",
        };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        await dbConnection
            .OpenDb()
            .ExecuteAsync(_createTableSqlForExecuteAsyncQuery);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.GetTablesInDb(out List<string> tables);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        tables.Should().Contain(expected);
    }

    [Fact]
    public async Task ExecuteAsync_CreateTestTableForExecuteWithParams()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string expectedTable = dbConnection.DatabaseType switch
        {
            DatabaseType.PostgreSQL => "public.TestTableForExecuteAsyncWithParams",
            _ => "TestTableForExecuteAsyncWithParams",
        };
        const string expectedName = "Name_1";
        const string selectQuery = @"SELECT ""Name"" FROM ""TestTableForExecuteAsyncWithParams""";
        List<VelocipedeCommandParameter> parameters = [new() { Name = "TestRecordName", Value = expectedName }];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        await dbConnection
            .OpenDb()
            .ExecuteAsync(_createTableSqlForExecuteAsyncWithParamsQuery, parameters);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.GetTablesInDb(out List<string> tables);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.Query(selectQuery, out List<string> names);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        tables.Should().Contain(expectedTable);
        names.Should().Contain(expectedName);
    }
}
