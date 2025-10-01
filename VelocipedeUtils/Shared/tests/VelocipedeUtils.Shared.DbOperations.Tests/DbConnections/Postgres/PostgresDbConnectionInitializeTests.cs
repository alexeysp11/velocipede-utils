using Dapper;
using FluentAssertions;
using Npgsql;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionInitializeTests : BaseDbConnectionInitializeTests, IClassFixture<PgDatabaseFixture>
    {
        private PgDatabaseFixture _fixture;

        public PostgresDbConnectionInitializeTests(PgDatabaseFixture fixture) : base(DatabaseType.PostgreSQL)
        {
            _fixture = fixture;
            _connectionString = $"Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=mydatabase;";
        }

        [Theory]
        [InlineData("Host=localhost;Database=mydatabase;", "mydatabase")]
        [InlineData("Host='localhost';Database=mydatabase;", "mydatabase")]
        [InlineData("Host=localhost;Database='mydatabase';", "mydatabase")]
        [InlineData("Host='localhost';Database='mydatabase';", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Database=mydatabase;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username='myuser';Database=mydatabase;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=mydatabase;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=my_database;", "my_database")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=my-database;", "my-database")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Password='mypassword';Database=mydatabase;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;SSLMode=Prefer;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;SSLMode=Prefer", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;SSLMode=Prefer;Timeout=15", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;SSLMode=Prefer;Timeout=15;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;SSLMode=Prefer;Pooling=true", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;SSLMode=Prefer;Minimum Pool Size=5;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;SSLMode=Prefer;Maximum Pool Size=25;", "mydatabase")]
        [InlineData("Host=localhost;Port=5432;Username=myuser;Database=mydatabase;SSLMode=Prefer;Minimum Pool Size=5;Maximum Pool Size=25;", "mydatabase")]
        public void GetDatabaseName_StaticGetter_DatabaseNameEqualsToExpected(string connectionString, string expected)
        {
            // Arrange & Act.
            string result = PgDbConnection.GetDatabaseName(connectionString);

            // Assert.
            result.Should().Be(expected);
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
