using Dapper;
using FluentAssertions;
using Npgsql;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;
using VelocipedeUtils.Shared.DbOperations.Tests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionInitializeTests : BaseDbConnectionInitializeTests, IClassFixture<PgDatabaseFixture>
    {
        private PgDatabaseFixture _fixture;

        public PostgresDbConnectionInitializeTests(PgDatabaseFixture fixture) : base(DatabaseType.PostgreSQL)
        {
            _fixture = fixture;
            _connectionString = $"Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=mydatabase;";
        }

        [Fact]
        public async Task Database_CanRunQuery()
        {
            // Arrange.
            await using NpgsqlConnection connection = new(_fixture.ConnectionString);
            connection.Open();

            // Act.
            const int expected = 1;
            var actual = await connection.QueryFirstAsync<int>("SELECT 1");

            // Assert.
            actual.Should().Be(expected);
        }
    }
}
