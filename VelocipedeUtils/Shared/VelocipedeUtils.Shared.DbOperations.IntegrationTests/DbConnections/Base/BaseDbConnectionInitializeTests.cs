using System.Data.Common;
using Dapper;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base
{
    public abstract class BaseDbConnectionInitializeTests
    {
        protected IDatabaseFixture _fixture;
        protected readonly string _connectionString;

        protected BaseDbConnectionInitializeTests(IDatabaseFixture fixture)
        {
            _fixture = fixture;
            _connectionString = _fixture.ConnectionString;
        }

        [Fact]
        public async Task InitializeDatabaseInContainers_CanRunQuery()
        {
            // Arrange.
            const int expected = 1;
            await using DbConnection connection = _fixture.GetDbConnection();
            connection.Open();

            // Act.
            int result = await connection.QueryFirstAsync<int>("SELECT 1");

            // Assert.
            result.Should().Be(expected);
        }

        [Fact]
        public void QueryFirstOrDefault_OpenDbAndGetOneRecord_ResultEqualsToExpected()
        {
            // Arrange.
            const int expected = 1;
            IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .QueryFirstOrDefault<int>("SELECT 1", out int result);
            dbConnection.IsConnected.Should().BeTrue();
            dbConnection
                .CloseDb();

            // Assert.
            dbConnection.Should().NotBeNull();
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().Be(expected);
        }
    }
}
