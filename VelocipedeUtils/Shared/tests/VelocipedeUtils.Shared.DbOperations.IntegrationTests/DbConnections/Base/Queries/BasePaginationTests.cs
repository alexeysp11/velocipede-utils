using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.Tests.Core.Compare;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

/// <summary>
/// Base class for testing pagination with queries.
/// </summary>
public abstract class BasePaginationTests : BaseDbConnectionTests
{
    protected BasePaginationTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
            SelectFromTestModels,
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
            .QueryFirstOrDefaultAsync<TestModel>(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryFirstOrDefaultAsync<TestModel>(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryFirstOrDefaultAsync<TestModel>(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryFirstOrDefaultAsync<TestModel>(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            SelectFromTestModels,
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
            SelectFromTestModels,
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
            .QueryAsync<TestModel>(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryAsync<TestModel>(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryAsync<TestModel>(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryAsync<TestModel>(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            SelectFromTestModels,
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
                SelectFromTestModels,
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
            SelectFromTestModels,
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
            .QueryDataTableAsync(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryDataTableAsync(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryDataTableAsync(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
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
            .QueryDataTableAsync(SelectFromTestModels, parameters: null, predicate: null, paginationInfo: paginationInfo);
        dbConnection
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
            SelectFromTestModels,
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
}
