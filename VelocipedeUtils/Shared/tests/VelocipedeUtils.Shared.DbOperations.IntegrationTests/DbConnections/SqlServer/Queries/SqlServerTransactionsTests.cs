using Dapper;
using FluentAssertions;
using System.Data.Common;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Queries;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerTransactionsTests : BaseTransactionsTests
{
    public SqlServerTransactionsTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }

    [Fact]
    public override async Task QueryFirstOrDefaultAsync_SameConnection_ConnectionAndTransaction()
    {
        // Arrange.
        using IVelocipedeDbConnection velocipedeConnection = _fixture.GetVelocipedeDbConnection().OpenDb().BeginTransaction();
        DbConnection connection = velocipedeConnection.Connection!;

        // Act.
        velocipedeConnection.IsConnected.Should().BeTrue();
        _ = await velocipedeConnection.QueryFirstOrDefaultAsync<int>("SELECT 1");
        Func<Task<int>> act = async () => await connection.QueryFirstOrDefaultAsync<int>("SELECT 2");

        // Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>();
    }
}
