using FluentAssertions;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.Tests.Core.Compare;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

/// <summary>
/// Base class for testing the following methods:
/// <list type="bullet">
///     <item>
///         <description><see cref="IVelocipedeDbConnectionExtensions.GetAllData(IVelocipedeDbConnection, string, out DataTable)"/>.</description>
///     </item>
///     <item>
///         <description><see cref="IVelocipedeDbConnectionExtensions.GetAllData{T}(IVelocipedeDbConnection, string, out List{T})"/>.</description>
///     </item>
///     <item>
///         <description><see cref="IVelocipedeDbConnectionExtensions.GetAllDataAsync(IVelocipedeDbConnection, string)"/>.</description>
///     </item>
///     <item>
///         <description><see cref="IVelocipedeDbConnectionExtensions.GetAllDataAsync{T}(IVelocipedeDbConnection, string)"/>.</description>
///     </item>
/// </list>
/// </summary>
public abstract class BaseGetAllDataTests : BaseDbConnectionTests
{
    /// <summary>
    /// Default constructor for creating <see cref="BaseGetAllDataTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    /// <param name="createDatabaseSql">SQL query to create database.</param>
    protected BaseGetAllDataTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void GetAllData_FixtureGetAllTestModelsAsDataTable()
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
    public void GetAllData_FixtureGetAllTestModelsAsList()
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

    [Fact]
    public void GetAllData_FixtureGetAllCaseInsesitiveModelsAsDataTable()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        
        // 2. Create table.
        string tableName = nameof(GetAllData_FixtureGetAllCaseInsesitiveModelsAsDataTable);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Expected result.
        DataTable expected = new List<CaseInsensitiveModel>
        {
            new() { Id = 1, Value = "value 1" },
            new() { Id = 2, Value = "value 2" },
            new() { Id = 3, Value = "value 3" },
            new() { Id = 4, Value = "value 4" },
        }.ToDataTable();

        // Act.
        dbConnection
            .GetAllData(tableName, out DataTable result)
            .CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected, caseSensitiveColumnNames: false)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void GetAllData_FixtureGetAllCaseInsesitiveModelsAsList()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = nameof(GetAllData_FixtureGetAllCaseInsesitiveModelsAsList);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Expected result.
        List<CaseInsensitiveModel> expected =
        [
            new() { Id = 1, Value = "value 1" },
            new() { Id = 2, Value = "value 2" },
            new() { Id = 3, Value = "value 3" },
            new() { Id = 4, Value = "value 4" },
        ];

        // Act.
        dbConnection
            .GetAllData(tableName, out List<CaseInsensitiveModel> result)
            .CloseDb();

        // Assert.
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
    public async Task GetAllDataAsync_FixtureGetAllTestModelsAsDataTable()
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
    public async Task GetAllDataAsync_FixtureGetAllTestModelsAsList()
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

    [Fact]
    public async Task GetAllDataAsync_FixtureGetAllCaseInsesitiveModelsAsDataTable()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = nameof(GetAllDataAsync_FixtureGetAllCaseInsesitiveModelsAsDataTable);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Expected result.
        DataTable expected = new List<CaseInsensitiveModel>
        {
            new() { Id = 1, Value = "value 1" },
            new() { Id = 2, Value = "value 2" },
            new() { Id = 3, Value = "value 3" },
            new() { Id = 4, Value = "value 4" },
        }.ToDataTable();

        // Act.
        DataTable result = await dbConnection.GetAllDataAsync(tableName);
        dbConnection.CloseDb();

        // Assert.
        DataTableCompareHelper.AreDataTablesEquivalent(result, expected, caseSensitiveColumnNames: false)
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task GetAllDataAsync_FixtureGetAllCaseInsesitiveModelsAsList()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = nameof(GetAllDataAsync_FixtureGetAllCaseInsesitiveModelsAsList);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Expected result.
        List<CaseInsensitiveModel> expected =
        [
            new() { Id = 1, Value = "value 1" },
            new() { Id = 2, Value = "value 2" },
            new() { Id = 3, Value = "value 3" },
            new() { Id = 4, Value = "value 4" },
        ];

        // Act.
        List<CaseInsensitiveModel> result = await dbConnection.GetAllDataAsync<CaseInsensitiveModel>(tableName);
        dbConnection.CloseDb();

        // Assert.
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
}
