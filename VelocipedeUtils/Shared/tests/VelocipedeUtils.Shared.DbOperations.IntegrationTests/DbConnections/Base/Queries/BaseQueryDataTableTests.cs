using FluentAssertions;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Models;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.DbOperations.Models.QueryParameters;
using VelocipedeUtils.Shared.Tests.Core.Compare;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.QueryDataTable(string, out DataTable)"/>.
/// </summary>
public abstract class BaseQueryDataTableTests : BaseDbConnectionTests
{
    protected BaseQueryDataTableTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
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
            .QueryDataTable(SelectFromTestModels, out DataTable result)
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
            .QueryDataTable(SelectFromTestModelsWhereIdBigger, parameters, out DataTable result)
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
            .QueryDataTable(SelectFromTestModelsWhereIdBigger, parameters, predicate, out DataTable result)
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
                SelectFromTestModels,
                parameters: null,
                predicate: predicate,
                dtResult: out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void QueryDataTable_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.QueryDataTable(SelectFromTestModels, out _);

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
        Action act = () => dbConnection.QueryDataTable(SelectFromTestModels, out _);

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
        Action act = () => dbConnection.QueryDataTable(SelectFromTestModels, out _);

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
            .QueryDataTableAsync(SelectFromTestModels);
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
            .QueryDataTableAsync(SelectFromTestModelsWhereIdBigger, parameters);
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
            .QueryDataTableAsync(SelectFromTestModelsWhereIdBigger, parameters, predicate);
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
            .QueryDataTableAsync(SelectFromTestModels, parameters: null, predicate: predicate);
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
        Func<Task> act = async () => await dbConnection.QueryDataTableAsync(SelectFromTestModels);

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
        Func<Task> act = async () => await dbConnection.QueryDataTableAsync(SelectFromTestModels);

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
        Func<Task> act = async () => await dbConnection.QueryDataTableAsync(SelectFromTestModels);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }
}
