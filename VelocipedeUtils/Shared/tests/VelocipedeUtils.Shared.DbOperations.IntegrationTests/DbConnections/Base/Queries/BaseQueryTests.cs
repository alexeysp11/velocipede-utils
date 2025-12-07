using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Models;
using VelocipedeUtils.Shared.DbOperations.Models.QueryParameters;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.Query{T}(string, out List{T})"/>.
/// </summary>
public abstract class BaseQueryTests : BaseDbConnectionTests
{
    protected BaseQueryTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
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
            .Query(SelectFromTestModels, out List<TestModel> result)
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
            .Query(SelectFromTestModelsWhereIdBigger, parameters, out List<TestModel> result)
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
            .Query(SelectFromTestModelsWhereIdBigger, parameters, predicate, out List<TestModel> result)
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
                SelectFromTestModels,
                parameters: null,
                predicate: predicate,
                result: out List<TestModel> result)
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
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
            .QueryAsync<TestModel>(SelectFromTestModels);
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
            .QueryAsync<TestModel>(SelectFromTestModelsWhereIdBigger, parameters);
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
            .QueryAsync(SelectFromTestModelsWhereIdBigger, parameters, predicate);
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
            .QueryAsync(SelectFromTestModels, parameters: null, predicate: predicate);
        dbConnection
            .CloseDb();

        // Assert.
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }
}
