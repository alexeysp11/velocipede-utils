using System.Data;
using System.Data.Common;
using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.Tests.Core.Compare;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections;

/// <summary>
/// Base class for unit testing <see cref="IVelocipedeDbConnection"/>.
/// </summary>
public abstract class BaseDbConnectionTests
{
    protected IDatabaseFixture _fixture;

    protected readonly string _connectionString;

    private readonly string _createTestModelsSql;
    private readonly string _createTestUsersSql;

    private const string SELECT_FROM_TESTMODELS = @"SELECT ""Id"", ""Name"" FROM ""TestModels""";
    private const string SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER = @"SELECT ""Id"", ""Name"" FROM ""TestModels"" WHERE ""Id"" >= @TestModelsId";

    protected string _createTableSqlForExecuteQuery;
    protected string _createTableSqlForExecuteAsyncQuery;
    protected string _createTableSqlForExecuteWithParamsQuery;
    protected string _createTableSqlForExecuteAsyncWithParamsQuery;

    protected BaseDbConnectionTests(
        IDatabaseFixture fixture,
        string createDatabaseSql,
        string createTestModelsSql,
        string createTestUsersSql)
    {
        _fixture = fixture;
        _connectionString = _fixture.ConnectionString;

        _createTestModelsSql = createTestModelsSql;
        _createTestUsersSql = createTestUsersSql;
        _createTableSqlForExecuteQuery = string.Empty;
        _createTableSqlForExecuteWithParamsQuery = string.Empty;
        _createTableSqlForExecuteAsyncQuery = string.Empty;
        _createTableSqlForExecuteAsyncWithParamsQuery = string.Empty;

        CreateTestDatabase(createDatabaseSql);
        InitializeTestDatabase();
    }

    [Fact]
    public void Execute_CreateTestTableForExecute_TableExists()
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
    public void Execute_CreateTestTableForExecuteWithParams_TableExists()
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
    public async Task ExecuteAsync_CreateTestTableForExecuteAsync_TableExists()
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
    public async Task ExecuteAsync_CreateTestTableForExecuteWithParams_TableExists()
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

    [Fact]
    public void QueryFirstOrDefault_OpenDbAndGetOneRecord_ResultEqualsToExpected()
    {
        // Arrange.
        const int expected = 1;
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault("SELECT 1", out int result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Fact]
    public void QueryFirstOrDefault_FixtureWithoutRestrictions_GetTestModelWithIdEquals1()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        TestModel expected = new() { Id = 1, Name = "Test_1" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault(SELECT_FROM_TESTMODELS, out TestModel? result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void QueryFirstOrDefault_FixtureWithParams_GetTestModelWithIdEquals5()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        TestModel expected = new() { Id = 5, Name = "Test_5" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, out TestModel? result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void QueryFirstOrDefault_FixtureWithParamsAndDelegate_GetTestModelWithIdEquals7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        Func<TestModel, bool> predicate = x => x.Id >= 7;
        TestModel expected = new() { Id = 7, Name = "Test_7" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, predicate, out TestModel? result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void QueryFirstOrDefault_FixtureWithDelegate_GetTestModelWithIdEquals7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<TestModel, bool> predicate = x => x.Id >= 7;
        TestModel expected = new() { Id = 7, Name = "Test_7" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: predicate,
                result: out TestModel? result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void QueryFirstOrDefault_FixtureWithPaginationByZeroOffset_GetTestModelWithIdEquals1(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 7,
            offset: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        TestModel expected = new() { Id = 1, Name = "Test_1" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                result: out TestModel? result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void QueryFirstOrDefault_FixtureWithPaginationByZeroIndex_GetTestModelWithIdEquals1(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 7,
            index: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        TestModel expected = new() { Id = 1, Name = "Test_1" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                result: out TestModel? result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void QueryFirstOrDefault_FixtureWithPaginationByIndex_GetTestModelWithIdEquals5(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 4,
            index: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        TestModel expected = new() { Id = 5, Name = "Test_5" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                result: out TestModel? result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void QueryFirstOrDefault_FixtureWithPaginationByOffset_GetTestModelWithIdEquals2(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 6,
            offset: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        TestModel expected = new() { Id = 2, Name = "Test_2" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryFirstOrDefault(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                result: out TestModel? result);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(7, 0, VelocipedePaginationType.None, "")]
    [InlineData(7, 0, VelocipedePaginationType.None, null)]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, "")]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, null)]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, "")]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, null)]
    public void QueryFirstOrDefault_NullOrEmptyOrderingFieldName_ThrowsInvalidOperationException(
        int limit,
        int offset,
        VelocipedePaginationType paginationType,
        string? orderingFieldName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit,
            offset,
            paginationType,
            orderingFieldName);

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection.OpenDb();
        Func<IVelocipedeDbConnection> act = () => dbConnection.QueryFirstOrDefault<TestModel>(
            SELECT_FROM_TESTMODELS,
            parameters: null,
            predicate: null,
            paginationInfo: paginationInfo,
            result: out _);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();
        dbConnection.IsConnected.Should().BeFalse();

        // Assert.
        dbConnection.Should().NotBeNull();
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.OrderingFieldNameCouldNotBeNullOrEmpty);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_TwoActiveConnections()
    {
        // Arrange.
        const int expected1 = 1;
        const int expected2 = 2;
        using IVelocipedeDbConnection connection1 = _fixture.GetVelocipedeDbConnection().OpenDb();
        using IVelocipedeDbConnection connection2 = _fixture.GetVelocipedeDbConnection().OpenDb();

        // Act.
        connection1.IsConnected.Should().BeTrue();
        connection2.IsConnected.Should().BeTrue();
        int result1 = await connection1.QueryFirstOrDefaultAsync<int>("SELECT 1");
        int result2 = await connection2.QueryFirstOrDefaultAsync<int>("SELECT 2");
        connection1.IsConnected.Should().BeTrue();
        connection2.IsConnected.Should().BeTrue();
        connection1.CloseDb();
        connection2.CloseDb();

        // Assert.
        (connection1 == connection2).Should().BeFalse();
        connection1.ConnectionString.Should().Be(connection2.ConnectionString);
        connection1.DatabaseName.Should().Be(connection2.DatabaseName);
        result1.Should().Be(expected1);
        result2.Should().Be(expected2);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_ConnectionAndDapper()
    {
        // Arrange.
        const int expected1 = 1;
        const int expected2 = 2;
        using IVelocipedeDbConnection velocipedeConnection = _fixture.GetVelocipedeDbConnection().OpenDb();
        using DbConnection connection = _fixture.GetDbConnection();

        // Act.
        velocipedeConnection.IsConnected.Should().BeTrue();
        int result1 = await velocipedeConnection.QueryFirstOrDefaultAsync<int>("SELECT 1");
        int result2 = await connection.QueryFirstOrDefaultAsync<int>("SELECT 2");
        velocipedeConnection.IsConnected.Should().BeTrue();
        velocipedeConnection.CloseDb();

        // Assert.
        result1.Should().Be(expected1);
        result2.Should().Be(expected2);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_SameConnection_ConnectionAndDapper()
    {
        // Arrange.
        const int expected1 = 1;
        const int expected2 = 2;
        using IVelocipedeDbConnection velocipedeConnection = _fixture.GetVelocipedeDbConnection().OpenDb();
        DbConnection connection = velocipedeConnection.Connection!;

        // Act.
        velocipedeConnection.IsConnected.Should().BeTrue();
        int result1 = await velocipedeConnection.QueryFirstOrDefaultAsync<int>("SELECT 1");
        int result2 = await connection.QueryFirstOrDefaultAsync<int>("SELECT 2");
        velocipedeConnection.IsConnected.Should().BeTrue();
        velocipedeConnection.CloseDb();

        // Assert.
        result1.Should().Be(expected1);
        result2.Should().Be(expected2);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_ConnectionAndTransaction()
    {
        // Arrange.
        const int expected1 = 1;
        const int expected2 = 2;
        using IVelocipedeDbConnection connection1 = _fixture.GetVelocipedeDbConnection().OpenDb();
        using IVelocipedeDbConnection connection2 = _fixture.GetVelocipedeDbConnection().OpenDb().BeginTransaction();

        // Act.
        connection1.IsConnected.Should().BeTrue();
        connection2.IsConnected.Should().BeTrue();
        int result1 = await connection1.QueryFirstOrDefaultAsync<int>("SELECT 1");
        int result2 = await connection2.QueryFirstOrDefaultAsync<int>("SELECT 2");
        connection1.IsConnected.Should().BeTrue();
        connection2.IsConnected.Should().BeTrue();
        connection1.CloseDb();
        connection2.CloseDb();

        // Assert.
        (connection1 == connection2).Should().BeFalse();
        connection1.ConnectionString.Should().Be(connection2.ConnectionString);
        connection1.DatabaseName.Should().Be(connection2.DatabaseName);
        result1.Should().Be(expected1);
        result2.Should().Be(expected2);
    }

    [Fact]
    public virtual async Task QueryFirstOrDefaultAsync_SameConnection_ConnectionAndTransaction()
    {
        // Arrange.
        const int expected1 = 1;
        const int expected2 = 2;
        using IVelocipedeDbConnection velocipedeConnection = _fixture.GetVelocipedeDbConnection().OpenDb().BeginTransaction();
        DbConnection connection = velocipedeConnection.Connection!;

        // Act.
        velocipedeConnection.IsConnected.Should().BeTrue();
        int result1 = await velocipedeConnection.QueryFirstOrDefaultAsync<int>("SELECT 1");
        int result2 = await connection.QueryFirstOrDefaultAsync<int>("SELECT 2");
        velocipedeConnection.IsConnected.Should().BeTrue();
        velocipedeConnection.CloseDb();

        // Assert.
        result1.Should().Be(expected1);
        result2.Should().Be(expected2);
    }

    [Fact]
    public virtual async Task QueryFirstOrDefaultAsync_TwoTransactions()
    {
        // Arrange.
        const int expected1 = 1;
        const int expected2 = 2;
        using IVelocipedeDbConnection connection1 = _fixture.GetVelocipedeDbConnection().OpenDb().BeginTransaction();
        using IVelocipedeDbConnection connection2 = _fixture.GetVelocipedeDbConnection().OpenDb().BeginTransaction();

        // Act.
        connection1.IsConnected.Should().BeTrue();
        connection2.IsConnected.Should().BeTrue();
        int result1 = await connection1.QueryFirstOrDefaultAsync<int>("SELECT 1");
        int result2 = await connection2.QueryFirstOrDefaultAsync<int>("SELECT 2");
        connection1.IsConnected.Should().BeTrue();
        connection2.IsConnected.Should().BeTrue();
        connection1.CloseDb();
        connection2.CloseDb();

        // Assert.
        (connection1 == connection2).Should().BeFalse();
        connection1.ConnectionString.Should().Be(connection2.ConnectionString);
        connection1.DatabaseName.Should().Be(connection2.DatabaseName);
        result1.Should().Be(expected1);
        result2.Should().Be(expected2);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_OpenDbAndGetOneRecord_ResultEqualsToExpected()
    {
        // Arrange.
        const int expected = 1;
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        int result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync<int>("SELECT 1");
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_FixtureWithoutRestrictions_GetTestModelWithIdEquals1()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        TestModel expected = new() { Id = 1, Name = "Test_1" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        TestModel? result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync<TestModel>(SELECT_FROM_TESTMODELS);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_FixtureWithParams_GetTestModelWithIdEquals5()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        TestModel expected = new() { Id = 5, Name = "Test_5" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        TestModel? result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync<TestModel>(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_FixtureWithParamsAndDelegate_GetTestModelWithIdEquals7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        Func<TestModel, bool> predicate = x => x.Id >= 7;
        TestModel expected = new() { Id = 7, Name = "Test_7" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        TestModel? result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, predicate);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_FixtureWithDelegate_GetTestModelWithIdEquals7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<TestModel, bool> predicate = x => x.Id >= 7;
        TestModel expected = new() { Id = 7, Name = "Test_7" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        TestModel? result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync(SELECT_FROM_TESTMODELS, parameters: null, predicate: predicate);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryFirstOrDefaultAsync_FixtureWithPaginationByZeroOffset_GetTestModelWithIdEquals1(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 7,
            offset: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        TestModel expected = new() { Id = 1, Name = "Test_1" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        TestModel? result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync<TestModel>(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryFirstOrDefaultAsync_FixtureWithPaginationByZeroIndex_GetTestModelWithIdEquals1(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 7,
            index: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        TestModel expected = new() { Id = 1, Name = "Test_1" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        TestModel? result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync<TestModel>(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryFirstOrDefaultAsync_FixtureWithPaginationByIndex_GetTestModelWithIdEquals1(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 4,
            index: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        TestModel expected = new() { Id = 5, Name = "Test_5" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        TestModel? result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync<TestModel>(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryFirstOrDefaultAsync_FixtureWithPaginationByOffset_GetTestModelWithIdEquals2(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 6,
            offset: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        TestModel expected = new() { Id = 2, Name = "Test_2" };

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        TestModel? result = await dbConnection
            .OpenDb()
            .QueryFirstOrDefaultAsync<TestModel>(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(7, 0, VelocipedePaginationType.None, "")]
    [InlineData(7, 0, VelocipedePaginationType.None, null)]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, "")]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, null)]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, "")]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, null)]
    public async Task QueryFirstOrDefaultAsync_NullOrEmptyOrderingFieldName_ThrowsInvalidOperationException(
        int limit,
        int offset,
        VelocipedePaginationType paginationType,
        string? orderingFieldName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit,
            offset,
            paginationType,
            orderingFieldName);

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection.OpenDb();
        Func<Task<TestModel?>> act = async () => await dbConnection.QueryFirstOrDefaultAsync<TestModel>(
            SELECT_FROM_TESTMODELS,
            parameters: null,
            predicate: null,
            paginationInfo: paginationInfo);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();
        dbConnection.IsConnected.Should().BeFalse();

        // Assert.
        dbConnection.Should().NotBeNull();
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.OrderingFieldNameCouldNotBeNullOrEmpty);
    }

    [Fact]
    public void Query_FixtureWithoutRestrictions_GetAllTestModels()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Query(SELECT_FROM_TESTMODELS, out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().HaveCount(8);
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public void Query_FixtureWithParams_GetAllTestModelsWithIdBiggerThan5()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        List<TestModel> expected =
        [
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Query(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public void Query_FixtureWithParamsAndDelegate_GetAllTestModelsWithIdBiggerThan5AndLessThan7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        Func<TestModel, bool> predicate = x => x.Id <= 7;
        List<TestModel> expected =
        [
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Query(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, predicate, out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public void Query_FixtureWithDelegate_GetAllTestModelsWithIdLessThan7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<TestModel, bool> predicate = x => x.Id <= 7;
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Query(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: predicate,
                result: out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void Query_FixtureWithPaginationByZeroOffset_GetAllTestModelsWithIdLessThan7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 7,
            offset: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Query(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                result: out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void Query_FixtureWithPaginationByZeroIndex_GetAllTestModelsWithIdLessThan7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 7,
            index: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Query(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                result: out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void Query_FixtureWithPaginationByIndex_GetAllTestModelsWithIdFrom5To8(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 4,
            index: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        List<TestModel> expected =
        [
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Query(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                result: out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void Query_FixtureWithPaginationByOffset_GetAllTestModelsWithIdFrom2To7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 6,
            offset: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        List<TestModel> expected =
        [
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .Query(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                result: out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(7, 0, VelocipedePaginationType.None, "")]
    [InlineData(7, 0, VelocipedePaginationType.None, null)]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, "")]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, null)]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, "")]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, null)]
    public void Query_NullOrEmptyOrderingFieldName_ThrowsInvalidOperationException(
        int limit,
        int offset,
        VelocipedePaginationType paginationType,
        string? orderingFieldName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit,
            offset,
            paginationType,
            orderingFieldName);

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection.OpenDb();
        Func<IVelocipedeDbConnection> act = () => dbConnection.Query<TestModel>(
            SELECT_FROM_TESTMODELS,
            parameters: null,
            predicate: null,
            paginationInfo: paginationInfo,
            result: out _);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();
        dbConnection.IsConnected.Should().BeFalse();

        // Assert.
        dbConnection.Should().NotBeNull();
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.OrderingFieldNameCouldNotBeNullOrEmpty);
    }

    [Fact]
    public async Task QueryAsync_FixtureWithoutRestrictions_GetAllTestModels()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .QueryAsync<TestModel>(SELECT_FROM_TESTMODELS);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().HaveCount(8);
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task QueryAsync_FixtureWithParams_GetAllTestModelsWithIdBiggerThan5()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        List<TestModel> expected =
        [
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .QueryAsync<TestModel>(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task QueryAsync_FixtureWithParamsAndDelegate_GetAllTestModelsWithIdBiggerThan5AndLessThan7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        Func<TestModel, bool> predicate = x => x.Id <= 7;
        List<TestModel> expected =
        [
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .QueryAsync(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, predicate);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task QueryAsync_FixtureWithDelegate_GetAllTestModelsWithIdLessThan7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<TestModel, bool> predicate = x => x.Id <= 7;
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .QueryAsync(SELECT_FROM_TESTMODELS, parameters: null, predicate: predicate);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryAsync_FixtureWithPaginationByZeroOffset_GetAllTestModelsWithIdLessThan7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 7,
            offset: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .QueryAsync<TestModel>(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryAsync_FixtureWithPaginationByZeroIndex_GetAllTestModelsWithIdLessThan7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 7,
            index: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .QueryAsync<TestModel>(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryAsync_FixtureWithPaginationByIndex_GetAllTestModelsWithIdFrom5To8(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 4,
            index: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        List<TestModel> expected =
        [
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .QueryAsync<TestModel>(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryAsync_FixtureWithPaginationByOffset_GetAllTestModelsWithIdFrom2To7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 6,
            offset: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        List<TestModel> expected =
        [
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .QueryAsync<TestModel>(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(7, 0, VelocipedePaginationType.None, "")]
    [InlineData(7, 0, VelocipedePaginationType.None, null)]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, "")]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, null)]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, "")]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, null)]
    public async Task QueryAsync_NullOrEmptyOrderingFieldName_ThrowsInvalidOperationException(
        int limit,
        int offset,
        VelocipedePaginationType paginationType,
        string? orderingFieldName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit,
            offset,
            paginationType,
            orderingFieldName);

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection.OpenDb();
        Func<Task<List<TestModel>>> act = async () => await dbConnection.QueryAsync<TestModel>(
            SELECT_FROM_TESTMODELS,
            parameters: null,
            predicate: null,
            paginationInfo: paginationInfo);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();
        dbConnection.IsConnected.Should().BeFalse();

        // Assert.
        dbConnection.Should().NotBeNull();
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.OrderingFieldNameCouldNotBeNullOrEmpty);
    }

    [Fact]
    public void QueryDataTable_FixtureWithoutRestrictions_GetAllTestModels()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryDataTable(SELECT_FROM_TESTMODELS, out DataTable result)
            .CloseDb();

        // Assert.
        result.Rows.Count.Should().Be(8);
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Fact]
    public void QueryDataTable_FixtureWithParams_GetTestModelsWithIdBiggerThan5()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        DataTable expected = new List<TestModel>
        {
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryDataTable(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Fact]
    public void QueryDataTable_FixtureWithParamsAndDelegate_GetTestModelsWithIdBiggerThan5AndLessThan7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        Func<dynamic, bool> predicate = x => x.Id <= 7;
        DataTable expected = new List<TestModel>
        {
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryDataTable(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, predicate, out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Fact]
    public void QueryDataTable_FixtureWithDelegate_GetTestModelsWithIdLessThan7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<dynamic, bool> predicate = x => x.Id <= 7;
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryDataTable(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: predicate,
                dtResult: out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void QueryDataTable_FixtureWithPaginationByZeroOffset_GetTestModelsWithIdLessThan7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 7,
            offset: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryDataTable(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                dtResult: out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void QueryDataTable_FixtureWithPaginationByZeroIndex_GetTestModelsWithIdLessThan7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 7,
            index: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryDataTable(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                dtResult: out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void QueryDataTable_FixtureWithPaginationByIndex_GetTestModelsWithIdFrom5To8(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 4,
            index: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        DataTable expected = new List<TestModel>
        {
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryDataTable(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                dtResult: out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public void QueryDataTable_FixtureWithPaginationByOffset_GetTestModelsWithIdFrom2To7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 6,
            offset: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        DataTable expected = new List<TestModel>
        {
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .QueryDataTable(
                SELECT_FROM_TESTMODELS,
                parameters: null,
                predicate: null,
                paginationInfo: paginationInfo,
                dtResult: out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(7, 0, VelocipedePaginationType.None, "")]
    [InlineData(7, 0, VelocipedePaginationType.None, null)]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, "")]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, null)]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, "")]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, null)]
    public void QueryDataTable_NullOrEmptyOrderingFieldName_ThrowsInvalidOperationException(
        int limit,
        int offset,
        VelocipedePaginationType paginationType,
        string? orderingFieldName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit,
            offset,
            paginationType,
            orderingFieldName);

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection.OpenDb();
        Func<IVelocipedeDbConnection> act = () => dbConnection.QueryDataTable(
            SELECT_FROM_TESTMODELS,
            parameters: null,
            predicate: null,
            paginationInfo: paginationInfo,
            dtResult: out _);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();
        dbConnection.IsConnected.Should().BeFalse();

        // Assert.
        dbConnection.Should().NotBeNull();
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.OrderingFieldNameCouldNotBeNullOrEmpty);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void QueryDataTable_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.QueryDataTable(SELECT_FROM_TESTMODELS, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Fact]
    public void QueryDataTable_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.QueryDataTable(SELECT_FROM_TESTMODELS, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void QueryDataTable_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.QueryDataTable(SELECT_FROM_TESTMODELS, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Fact]
    public async Task QueryDataTableAsync_FixtureWithoutRestrictions_GetAllTestModels()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .QueryDataTableAsync(SELECT_FROM_TESTMODELS);
        dbConnection
            .CloseDb();

        // Assert.
        result.Rows.Count.Should().Be(8);
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Fact]
    public async Task QueryDataTableAsync_FixtureWithParams_GetTestModelsWithIdBiggerThan5()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        DataTable expected = new List<TestModel>
        {
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .QueryDataTableAsync(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters);
        dbConnection
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Fact]
    public async Task QueryDataTableAsync_FixtureWithParamsAndDelegate_GetTestModelsWithIdBiggerThan5AndLessThan7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
        Func<dynamic, bool> predicate = x => x.Id <= 7;
        DataTable expected = new List<TestModel>
        {
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .QueryDataTableAsync(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, predicate);
        dbConnection
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Fact]
    public async Task QueryDataTableAsync_FixtureWithDelegate_GetTestModelsWithIdLessThan7()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<dynamic, bool> predicate = x => x.Id <= 7;
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .QueryDataTableAsync(SELECT_FROM_TESTMODELS, parameters: null, predicate: predicate);
        dbConnection
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryDataTableAsync_FixtureWithPaginationByZeroOffset_GetTestModelsWithIdLessThan7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 7,
            offset: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .QueryDataTableAsync(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryDataTableAsync_FixtureWithPaginationByZeroIndex_GetTestModelsWithIdLessThan7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 7,
            index: 0,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .QueryDataTableAsync(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryDataTableAsync_FixtureWithPaginationByIndex_GetTestModelsWithIdFrom5To8(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(
            limit: 4,
            index: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        DataTable expected = new List<TestModel>
        {
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .QueryDataTableAsync(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(VelocipedePaginationType.LimitOffset)]
    [InlineData(VelocipedePaginationType.KeysetById)]
    public async Task QueryDataTableAsync_FixtureWithPaginationByOffset_GetTestModelsWithIdFrom2To7(
        VelocipedePaginationType paginationType)
    {
        // Arrange.
        string orderingFieldName = "Id";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit: 6,
            offset: 1,
            paginationType: paginationType,
            orderingFieldName: orderingFieldName);
        DataTable expected = new List<TestModel>
        {
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
        }.Select(x => new { x.Id, x.Name }).ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .QueryDataTableAsync(SELECT_FROM_TESTMODELS, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task QueryDataTableAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task> act = async () => await dbConnection.QueryDataTableAsync(SELECT_FROM_TESTMODELS);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Fact]
    public async Task QueryDataTableAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task> act = async () => await dbConnection.QueryDataTableAsync(SELECT_FROM_TESTMODELS);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public async Task QueryDataTableAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task> act = async () => await dbConnection.QueryDataTableAsync(SELECT_FROM_TESTMODELS);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Theory]
    [InlineData(7, 0, VelocipedePaginationType.None, "")]
    [InlineData(7, 0, VelocipedePaginationType.None, null)]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, "")]
    [InlineData(7, 0, VelocipedePaginationType.LimitOffset, null)]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, "")]
    [InlineData(7, 0, VelocipedePaginationType.KeysetById, null)]
    public async Task QueryDataTableAsync_NullOrEmptyOrderingFieldName_ThrowsInvalidOperationException(
        int limit,
        int offset,
        VelocipedePaginationType paginationType,
        string? orderingFieldName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(
            limit,
            offset,
            paginationType,
            orderingFieldName);

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection.OpenDb();
        Func<Task<DataTable>> act = async () => await dbConnection.QueryDataTableAsync(
            SELECT_FROM_TESTMODELS,
            parameters: null,
            predicate: null,
            paginationInfo: paginationInfo);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection.CloseDb();
        dbConnection.IsConnected.Should().BeFalse();

        // Assert.
        dbConnection.Should().NotBeNull();
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.OrderingFieldNameCouldNotBeNullOrEmpty);
    }

    [Fact]
    public void GetAllData_FixtureGetAllTestModelsAsDataTable_QuantityEqualsToSpecified()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        }.ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .GetAllData("\"TestModels\"", out DataTable result)
            .CloseDb();

        // Assert.
        result.Rows.Count.Should().Be(8);
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Fact]
    public void GetAllData_FixtureGetAllTestModelsAsList_QuantityEqualsToSpecified()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        dbConnection
            .OpenDb()
            .GetAllData("\"TestModels\"", out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Count.Should().Be(8);
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetAllData_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetAllData("\"TestModels\"", out _);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Fact]
    public void GetAllData_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetAllData("\"TestModels\"", out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void GetAllData_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetAllData("\"TestModels\"", out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }
    
    [Fact]
    public async Task GetAllDataAsync_FixtureGetAllTestModelsAsDataTable_QuantityEqualsToSpecified()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        DataTable expected = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        }.ToDataTable();

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        DataTable result = await dbConnection
            .OpenDb()
            .GetAllDataAsync("\"TestModels\"");
        dbConnection
            .CloseDb();

        // Assert.
        result.Rows.Count.Should().Be(8);
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Fact]
    public async Task GetAllDataAsync_FixtureGetAllTestModelsAsList_QuantityEqualsToSpecified()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<TestModel> expected =
        [
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        ];

        // Act.
        dbConnection.IsConnected.Should().BeFalse();
        List<TestModel> result = await dbConnection
            .OpenDb()
            .GetAllDataAsync<TestModel>("\"TestModels\"");
        dbConnection
            .CloseDb();

        // Assert.
        result.Count.Should().Be(8);
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetAllDataAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<DataTable>> act = async () => await dbConnection.GetAllDataAsync("\"TestModels\"");

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Fact]
    public async Task GetAllDataAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<TestModel>>> act = async () => await dbConnection.GetAllDataAsync<TestModel>("\"TestModels\"");

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public async Task GetAllDataAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<DataTable>> act = async () => await dbConnection.GetAllDataAsync("\"TestModels\"");

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Fact]
    public void DbExists_ConnectionStringFromFixture_ReturnsTrue()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        bool result = dbConnection.DbExists();

        // Assert.
        result.Should().BeTrue();
    }

    [Fact]
    public void DbExists_ConnectAndSetNotExistingDb_ReturnsFalse()
    {
        // Arrange.
        string dbName = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection
            .GetConnectionString(dbName, out string newConnectionString)
            .SetConnectionString(newConnectionString);

        // Act.
        bool result = dbConnection.DbExists();

        // Assert.
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void DbExists_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.DbExists();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Fact]
    public void DbExists_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.DbExists();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void DbExists_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.DbExists();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Fact]
    public void CreateDb_ConnectionStringFromFixture_ThrowsInvalidOperationException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.CreateDb();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.DatabaseAlreadyExists);
    }

    [Fact]
    public virtual void CreateDb_ConnectAndSetNotExistingDbUsingSetters_DbExists()
    {
        // Arrange.
        string dbName = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        if (_fixture.DatabaseType == DatabaseType.SQLite)
            dbName = $"{dbName}.db";

        // Act.
        dbConnection
            .OpenDb()
            .GetConnectionString(dbName, out string newConnectionString)
            .SetConnectionString(newConnectionString)
            .CreateDb()
            .SwitchDb(dbName)
            .CloseDb();
        bool dbExists = dbConnection.DbExists();

        // Assert.
        dbExists.Should().BeTrue();
    }

    [Fact]
    public virtual void CreateDb_CreateNotExistingDbUsingExtensionMethod_DbExists()
    {
        // Arrange.
        string dbName = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .CreateDb(dbName)
            .SwitchDb(dbName)
            .CloseDb();
        bool dbExists = dbConnection.DbExists();

        // Assert.
        dbExists.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateDb_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDb();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void CreateDb_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDb();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Fact]
    public void CreateDb_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDb();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Fact]
    public void CreateDbIfNotExists_ConnectionStringFromFixture_NotThrowAnyException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.CreateDbIfNotExists();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateDbIfNotExists_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDbIfNotExists();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void CreateDbIfNotExists_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDbIfNotExists();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Fact]
    public void OpenDb_ConnectionStringFromFixture_NotThrowAnyException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.OpenDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeTrue();
    }

    [Fact]
    public void OpenDb_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.OpenDb();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void OpenDb_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.OpenDb();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void OpenDb_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.OpenDb();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void CloseDb_ConnectionStringFromFixture_NotThrowAnyException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void CloseDb_FixtureConnected_NotThrowAnyException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.OpenDb();
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void CloseDb_GuidInsteadOfConnectionString_NotThrowAnyException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CloseDb_NullOrEmptyConnectionString_NotThrowAnyException(string? connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void CloseDb_IncorrectConnectionString_NotThrowAnyException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void GetTablesInDb_FixtureNotConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        var expected = new List<string>
        {
            "TestModels",
            "TestUsers",
            "TestCities",
        };

        // Act.
        dbConnection.GetTablesInDb(out List<string> result);
        result = result
            .Select(x => x.Replace("public.", ""))
            .ToList();

        // Assert.
        result.Should().HaveCountGreaterThanOrEqualTo(3);
        foreach (var expectedString in expected)
        {
            result.Should().Contain(expectedString);
        }
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void GetTablesInDb_FixtureConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        var expected = new List<string>
        {
            "TestModels",
            "TestUsers",
            "TestCities",
        };

        // Act.
        dbConnection
            .OpenDb()
            .GetTablesInDb(out List<string> result)
            .CloseDb();
        result = result
            .Select(x => x.Replace("public.", ""))
            .ToList();

        // Assert.
        result.Should().HaveCountGreaterThanOrEqualTo(3);
        foreach (var expectedString in expected)
        {
            result.Should().Contain(expectedString);
        }
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void GetTablesInDb_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTablesInDb(out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetTablesInDb_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTablesInDb(out _);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void GetTablesInDb_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTablesInDb(out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public async Task GetTablesInDbAsync_FixtureNotConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        var expected = new List<string>
        {
            "TestModels",
            "TestUsers",
            "TestCities",
        };

        // Act.
        List<string> result = await dbConnection.GetTablesInDbAsync();
        result = result
            .Select(x => x.Replace("public.", ""))
            .ToList();

        // Assert.
        result.Should().HaveCountGreaterThanOrEqualTo(3);
        foreach (var expectedString in expected)
        {
            result.Should().Contain(expectedString);
        }
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public async Task GetTablesInDbAsync_FixtureConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        var expected = new List<string>
        {
            "TestModels",
            "TestUsers",
            "TestCities",
        };

        // Act.
        List<string> result = await dbConnection
            .OpenDb()
            .GetTablesInDbAsync();
        dbConnection
            .CloseDb();
        result = result
            .Select(x => x.Replace("public.", ""))
            .ToList();

        // Assert.
        result.Should().HaveCountGreaterThanOrEqualTo(3);
        foreach (var expectedString in expected)
        {
            result.Should().Contain(expectedString);
        }
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public async Task GetTablesInDbAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<string>>> act = async () => await dbConnection.GetTablesInDbAsync();

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetTablesInDbAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<string>>> act = async () => await dbConnection.GetTablesInDbAsync();

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public async Task GetTablesInDbAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<string>>> act = async () => await dbConnection.GetTablesInDbAsync();

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void GetColumns_FixtureNotConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string tableName = "\"TestModels\"";

        // Act.
        dbConnection.GetColumns(tableName, out List<VelocipedeColumnInfo>? result);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Fact]
    public void GetColumns_FixtureConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string tableName = "\"TestModels\"";

        // Act.
        dbConnection
            .OpenDb()
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? result)
            .CloseDb();
        
        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Fact]
    public void GetColumns_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetColumns(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetColumns_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetColumns(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void GetColumns_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetColumns(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public async Task GetColumnsAsync_FixtureNotConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string tableName = "\"TestModels\"";

        // Act.
        List<VelocipedeColumnInfo>? result = await dbConnection.GetColumnsAsync(tableName);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetColumnsAsync_FixtureConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string tableName = "\"TestModels\"";

        // Act.
        List<VelocipedeColumnInfo>? result = await dbConnection
            .OpenDb()
            .GetColumnsAsync(tableName);
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetColumnsAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeColumnInfo>>> act = async () => await dbConnection.GetColumnsAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetColumnsAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeColumnInfo>>> act = async () => await dbConnection.GetColumnsAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public async Task GetColumnsAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<object>> act = async () => await dbConnection.GetColumnsAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 1)]
    public void GetForeignKeys_FixtureNotConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection.GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo>? result);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 1)]
    public void GetForeignKeys_FixtureConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo>? result)
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Fact]
    public void GetForeignKeys_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetForeignKeys(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetForeignKeys_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetForeignKeys(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void GetForeignKeys_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetForeignKeys(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 1)]
    public async Task GetForeignKeysAsync_FixtureNotConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeForeignKeyInfo>? result = await dbConnection.GetForeignKeysAsync(tableName);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 1)]
    public async Task GetForeignKeysAsync_FixtureConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeForeignKeyInfo>? result = await dbConnection
            .OpenDb()
            .GetForeignKeysAsync(tableName);
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Fact]
    public async Task GetForeignKeysAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeForeignKeyInfo>>> act = async () => await dbConnection.GetForeignKeysAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetForeignKeysAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<object>> act = async () => await dbConnection.GetForeignKeysAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public async Task GetForeignKeysAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<object>> act = async () => await dbConnection.GetForeignKeysAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 2)]
    public void GetTriggers_FixtureNotConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection.GetTriggers(tableName, out List<VelocipedeTriggerInfo>? result);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 2)]
    public void GetTriggers_FixtureConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .GetTriggers(tableName, out List<VelocipedeTriggerInfo>? result)
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Fact]
    public void GetTriggers_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTriggers(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetTriggers_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTriggers(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void GetTriggers_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTriggers(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 2)]
    public async Task GetTriggersAsync_FixtureNotConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeTriggerInfo>? result = await dbConnection.GetTriggersAsync(tableName);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 2)]
    public async Task GetTriggersAsync_FixtureConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeTriggerInfo>? result = await dbConnection
            .OpenDb()
            .GetTriggersAsync(tableName);
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Fact]
    public async Task GetTriggersAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeTriggerInfo>>> act = async () => await dbConnection.GetTriggersAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetTriggersAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeTriggerInfo>>> act = async () => await dbConnection.GetTriggersAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public async Task GetTriggersAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeTriggerInfo>>> act = async () => await dbConnection.GetTriggersAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
    public virtual void GetSqlDefinition_FixtureNotConnected_ResultEqualsToExpected(string tableName)
    {
        // Arrange.
        string expected = GetExpectedSqlDefinition(tableName);
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection.GetSqlDefinition(tableName, out string? result);
        result = result?.Replace("\r\n", "\n");
        expected = expected.Replace("\r\n", "\n");

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
    public virtual void GetSqlDefinition_FixtureConnected_ResultEqualsToExpected(string tableName)
    {
        // Arrange.
        string expected = GetExpectedSqlDefinition(tableName);
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .GetSqlDefinition(tableName, out string? result)
            .CloseDb();
        result = result?.Replace("\r\n", "\n");
        expected = expected.Replace("\r\n", "\n");

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Fact]
    public void GetSqlDefinition_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetSqlDefinition_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void GetSqlDefinition_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
    public virtual async Task GetSqlDefinitionAsync_FixtureNotConnected_ResultEqualsToExpected(string tableName)
    {
        // Arrange.
        string expected = GetExpectedSqlDefinition(tableName);
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        string? result = await dbConnection.GetSqlDefinitionAsync(tableName);
        result = result?.Replace("\r\n", "\n");
        expected = expected.Replace("\r\n", "\n");

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
    public virtual async Task GetSqlDefinitionAsync_FixtureConnected_ResultEqualsToExpected(string tableName)
    {
        // Arrange.
        string expected = GetExpectedSqlDefinition(tableName);
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        string? result = await dbConnection
            .OpenDb()
            .GetSqlDefinitionAsync(tableName);
        dbConnection
            .CloseDb();
        result = result?.Replace("\r\n", "\n");
        expected = expected.Replace("\r\n", "\n");

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Fact]
    public async Task GetSqlDefinitionAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<string?>> act = async () => await dbConnection.GetSqlDefinitionAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetSqlDefinitionAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<string?>> act = async () => await dbConnection.GetSqlDefinitionAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public async Task GetSqlDefinitionAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<string?>> act = async () => await dbConnection.GetSqlDefinitionAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void SwitchDb_DbNameFromFixture_ReconnectedWithSameConnectionString()
    {
        // Arrange.
        string connectionString = string.Empty;
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string? dbName = dbConnection.GetActiveDatabaseName();
        Action act = () => dbConnection.SwitchDb(dbName, out connectionString);

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.ConnectionString.Should().Be(connectionString);
        dbConnection.ConnectionString.Should().Be(_fixture.ConnectionString);
        dbConnection.IsConnected.Should().BeTrue();
    }

    [Fact]
    public void SwitchDb_GuidInsteadOfDbName_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string dbName = Guid.NewGuid().ToString();
        Action act = () => dbConnection.SwitchDb(dbName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void SwitchDb_NullOrEmptyDbName_ThrowsInvalidOperationException(string? dbName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.SwitchDb(dbName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbNameException>()
            .WithMessage(ErrorMessageConstants.IncorrectDatabaseName);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT DATABASE NAME")]
    [InlineData("-;")]
    [InlineData("`<3;")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-database-name")]
    [InlineData("super-not-existing-database-name")]
    public void SwitchDb_IncorrectDbName_ThrowsVelocipedeDbConnectParamsException(string dbName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.SwitchDb(dbName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void BeginTransaction_ActiveConnection_Ok()
    {
        // Arrange.
        var sql = $@"create table ""{nameof(BeginTransaction_ActiveConnection_Ok)}"" (""Name"" varchar(50) NOT NULL)";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection().OpenDb();

        // Act.
        Action act = () => dbConnection.BeginTransaction().RollbackTransaction().CloseDb();

        // Assert.
        act
            .Should()
            .NotThrow();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void BeginTransaction_InactiveConnection_Ok()
    {
        // Arrange.
        var sql = $@"create table ""{nameof(BeginTransaction_InactiveConnection_Ok)}"" (""Name"" varchar(50) NOT NULL)";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        Action act = () => dbConnection.BeginTransaction().RollbackTransaction().CloseDb();

        // Assert.
        act
            .Should()
            .NotThrow();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void CommitTransaction_CreateTableUsingActiveConnection_TableExists()
    {
        // Arrange.
        string tableName = nameof(CommitTransaction_CreateTableUsingActiveConnection_TableExists);
        var sql = $@"create table ""{tableName}"" (""Name"" varchar(50) NOT NULL)";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction()
            .GetTablesInDb(out List<string>? tables)
            .CloseDb();
        if (dbConnection.DatabaseType == DatabaseType.PostgreSQL)
        {
            tableName = $"public.{tableName}";
        }

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        tables.Should().Contain(tableName);
    }

    [Fact]
    public void CommitTransaction_InactiveConnection_ThrowsInvalidOperationException()
    {
        // Arrange.
        var sql = $@"create table ""{nameof(CommitTransaction_InactiveConnection_ThrowsInvalidOperationException)}"" (""Name"" varchar(50) NOT NULL)";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        Action act = () => dbConnection.CommitTransaction();

        // Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.UnableToCommitNotOpenTransaction);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void RollbackTransaction_InactiveConnection_ThrowsInvalidOperationException()
    {
        // Arrange.
        var sql = $@"create table ""{nameof(RollbackTransaction_InactiveConnection_ThrowsInvalidOperationException)}"" (""Name"" varchar(50) NOT NULL)";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        Action act = () => dbConnection.RollbackTransaction();

        // Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.UnableToRollbackNotOpenTransaction);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void RollbackTransaction_CreateTableUsingActiveConnection_TableNotExist()
    {
        // Arrange.
        string tableName = nameof(RollbackTransaction_CreateTableUsingActiveConnection_TableNotExist);
        var sql = $@"create table ""{tableName}"" (""Name"" varchar(50) NOT NULL)";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .RollbackTransaction()
            .GetTablesInDb(out List<string>? tables)
            .CloseDb();
        if (dbConnection.DatabaseType == DatabaseType.PostgreSQL)
        {
            tableName = $"public.{tableName}";
        }

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        tables.Should().NotContain(tableName);
    }

    [Fact]
    public void CloseConnectionInTransaction_CreateTableUsingActiveConnection_TableNotExist()
    {
        // Arrange.
        string tableName = nameof(CloseConnectionInTransaction_CreateTableUsingActiveConnection_TableNotExist);
        var sql = $@"create table ""{tableName}"" (""Name"" varchar(50) NOT NULL)";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CloseDb();
        dbConnection
            .OpenDb()
            .GetTablesInDb(out List<string>? tables)
            .CloseDb();
        if (dbConnection.DatabaseType == DatabaseType.PostgreSQL)
        {
            tableName = $"public.{tableName}";
        }

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        tables.Should().NotContain(tableName);
    }

    private void CreateTestDatabase(string sql)
    {
        using DbConnection connection = _fixture.GetDbConnection();
        connection.Open();
        connection.Execute(sql);
    }

    private void InitializeTestDatabase()
    {
        using TestDbContext dbContext = _fixture.GetTestDbContext();

        // Test models.
        var testModels = new List<TestModel>
        {
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
        };
        foreach (TestModel testModel in testModels)
        {
            int? existingId = dbContext.TestModels.FirstOrDefault(x => x.Id == testModel.Id)?.Id;
            if (!existingId.HasValue)
            {
                dbContext.TestModels.Add(testModel);
            }
        }

        // Cities.
        var cities = new List<TestCity>
        {
            new() { Id = 1, Name = "City_1" },
            new() { Id = 2, Name = "City_2" },
            new() { Id = 3, Name = "City_3" },
            new() { Id = 4, Name = "City_4" },
        };
        foreach (TestCity city in cities)
        {
            int? existingId = dbContext.TestCities.FirstOrDefault(x => x.Id == city.Id)?.Id;
            if (!existingId.HasValue)
            {
                dbContext.TestCities.Add(city);
            }
        }
        TestCity? firstCity = cities.FirstOrDefault();
        TestCity? lastCity = cities.LastOrDefault();

        // Users.
        var users = new List<TestUser>
        {
            new() { Id = 1, Name = "User_1", Email = "User_1@example.com", CityId = lastCity?.Id, City = lastCity },
            new() { Id = 2, Name = "User_2", Email = "User_2@example.com" },
            new() { Id = 3, Name = "User_3", Email = "User_3@example.com" },
            new() { Id = 4, Name = "User_4", Email = "User_4@example.com", CityId = firstCity?.Id, City = firstCity },
            new() { Id = 5, Name = "User_5", Email = "User_5@example.com", CityId = firstCity?.Id, City = firstCity },
            new() { Id = 6, Name = "User_6", Email = "User_6@example.com" },
            new() { Id = 7, Name = "User_7", Email = "User_7@example.com", CityId = firstCity?.Id, City = firstCity },
            new() { Id = 8, Name = "User_8", Email = "User_8@example.com" },
            new() { Id = 9, Name = "User_9", Email = "User_9@example.com", CityId = lastCity?.Id, City = lastCity },
            new() { Id = 10, Name = "User_10", Email = "User_10@example.com", CityId = lastCity?.Id, City = lastCity },
            new() { Id = 11, Name = "User_11", Email = "User_11@example.com" },
        };
        foreach (TestUser user in users)
        {
            int? existingId = dbContext.TestUsers.FirstOrDefault(x => x.Id == user.Id)?.Id;
            if (!existingId.HasValue)
            {
                dbContext.TestUsers.Add(user);
            }
        }

        dbContext.SaveChanges();
    }

    private string GetExpectedSqlDefinition(string tableName)
    {
        tableName = tableName.Trim('"');
        return tableName switch
        {
            "TestModels" => _createTestModelsSql,
            "TestUsers" => _createTestUsersSql,
            _ => "",
        };
    }
}
