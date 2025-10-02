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
        public async Task InitializeDatabase_CanRunQuery()
        {
            // Arrange.
            await using DbConnection connection = _fixture.GetDbConnection();
            connection.Open();

            // Act.
            const int expected = 1;
            var actual = await connection.QueryFirstAsync<int>("SELECT 1");

            // Assert.
            actual.Should().Be(expected);
        }

        [Fact]
        public void QueryFirstOrDefault_GetOneRecord()
        {
            // Arrange.
            IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
        }
    }
}
