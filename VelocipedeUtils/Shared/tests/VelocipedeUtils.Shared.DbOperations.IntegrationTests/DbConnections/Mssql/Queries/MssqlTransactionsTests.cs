using Dapper;
using FluentAssertions;
using System.Data.Common;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Queries;

[Collection(nameof(MssqlDatabaseFixtureCollection))]
public sealed class MssqlTransactionsTests : BaseTransactionsTests
{
    public MssqlTransactionsTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
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
