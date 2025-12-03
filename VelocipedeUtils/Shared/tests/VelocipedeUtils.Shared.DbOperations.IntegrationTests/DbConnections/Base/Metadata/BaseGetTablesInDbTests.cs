using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Helpers;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.GetTablesInDb(out List{string})"/>.
/// </summary>
public abstract class BaseGetTablesInDbTests : BaseDbConnectionTests
{
    /// <summary>
    /// Default constructor for creating <see cref="BaseGetTablesInDbTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    /// <param name="createDatabaseSql">SQL query to create database.</param>
    protected BaseGetTablesInDbTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void GetTablesInDb_FixtureNotConnected()
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
    public void GetTablesInDb_FixtureConnected()
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

    [Theory]
    [InlineData(DelimitIdentifierType.None)]
    [InlineData(DelimitIdentifierType.DoubleQuotes)]
    public void GetTablesInDb_CaseInsensitive(
        DelimitIdentifierType delimitIdentifierType)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        CaseConversionType conversionType = CaseConversionType.None;
        string tableName = TableNameHelper.GetTableNameByTestMethod(
            methodName: nameof(GetTablesInDb_CaseInsensitive),
            conversionType: conversionType,
            delimitIdentifierType: delimitIdentifierType);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Expected result.
        string expectedTableName = tableName.ToLower();

        // Act.
        dbConnection
            .OpenDb()
            .GetTablesInDb(out List<string> result)
            .CloseDb();
        result = result
            .Select(x => x.Replace("public.", "").ToLower())
            .ToList();

        // Assert.
        result.Should().HaveCountGreaterThanOrEqualTo(1);
        result.Should().Contain(expectedTableName);
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
    public async Task GetTablesInDbAsync_FixtureNotConnected()
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
    public async Task GetTablesInDbAsync_FixtureConnected()
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

    [Theory]
    [InlineData(DelimitIdentifierType.None)]
    [InlineData(DelimitIdentifierType.DoubleQuotes)]
    public async Task GetTablesInDbAsync_CaseInsensitive(
        DelimitIdentifierType delimitIdentifierType)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        CaseConversionType conversionType = CaseConversionType.None;
        string tableName = TableNameHelper.GetTableNameByTestMethod(
            methodName: nameof(GetTablesInDbAsync_CaseInsensitive),
            conversionType: conversionType,
            delimitIdentifierType: delimitIdentifierType);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Expected result.
        string expectedTableName = tableName.ToLower();

        // Act.
        dbConnection.OpenDb();
        List<string> result = await dbConnection.GetTablesInDbAsync();
        dbConnection.CloseDb();
        result = result
            .Select(x => x.Replace("public.", "").ToLower())
            .ToList();

        // Assert.
        result.Should().HaveCountGreaterThanOrEqualTo(1);
        result.Should().Contain(expectedTableName);
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
}
