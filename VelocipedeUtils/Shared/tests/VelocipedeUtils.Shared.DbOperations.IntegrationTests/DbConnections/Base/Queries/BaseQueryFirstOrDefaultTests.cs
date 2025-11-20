using System.Data.Common;
using Dapper;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.QueryFirstOrDefault{T}(string, out T?)"/>.
/// </summary>
public abstract class BaseQueryFirstOrDefaultTests : BaseDbConnectionTests
{
    protected BaseQueryFirstOrDefaultTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void QueryFirstOrDefault_OpenDbAndGetOneRecord()
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
            .QueryFirstOrDefault(SelectFromTestModels, out TestModel? result);
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
            .QueryFirstOrDefault(SelectFromTestModelsWhereIdBigger, parameters, out TestModel? result);
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
            .QueryFirstOrDefault(SelectFromTestModelsWhereIdBigger, parameters, predicate, out TestModel? result);
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
                SelectFromTestModels,
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
    public async Task QueryFirstOrDefaultAsync_OpenDbAndGetOneRecord()
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
            .QueryFirstOrDefaultAsync<TestModel>(SelectFromTestModels);
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
            .QueryFirstOrDefaultAsync<TestModel>(SelectFromTestModelsWhereIdBigger, parameters);
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
            .QueryFirstOrDefaultAsync(SelectFromTestModelsWhereIdBigger, parameters, predicate);
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
            .QueryFirstOrDefaultAsync(SelectFromTestModels, parameters: null, predicate: predicate);
        dbConnection.IsConnected.Should().BeTrue();
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }
}
