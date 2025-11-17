using FluentAssertions;
using Microsoft.Data.Sqlite;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Queries;

public sealed class SqliteTransactionsTests : BaseTransactionsTests, IClassFixture<SqliteDatabaseFixture>
{
    public SqliteTransactionsTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL)
    {
    }
    [Fact]
    public override Task QueryFirstOrDefaultAsync_TwoTransactions()
    {
        // Arrange & Act.
        using IVelocipedeDbConnection connection1 = _fixture.GetVelocipedeDbConnection().OpenDb().BeginTransaction();
        using IVelocipedeDbConnection connection2 = _fixture.GetVelocipedeDbConnection().OpenDb();
        var act = () => connection2.BeginTransaction();

        // Assert.
        act
            .Should()
            .Throw<SqliteException>("'database is locked' exception expected to be thrown");

        // Rollback active transactions to prevent "Test Class Cleanup Failure" when deleting database files.
        connection1.RollbackTransaction();

        return Task.CompletedTask;
    }
}
