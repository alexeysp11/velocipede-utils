using Dapper;
using FluentAssertions;
using System.Data.Common;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

/// <summary>
/// Base class for testing transactions.
/// </summary>
public abstract class BaseTransactionsTests : BaseDbConnectionTests
{
    protected BaseTransactionsTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
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
        if (dbConnection.DatabaseType == VelocipedeDatabaseType.PostgreSQL)
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
        if (dbConnection.DatabaseType == VelocipedeDatabaseType.PostgreSQL)
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
        if (dbConnection.DatabaseType == VelocipedeDatabaseType.PostgreSQL)
        {
            tableName = $"public.{tableName}";
        }

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        tables.Should().NotContain(tableName);
    }
}
