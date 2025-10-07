using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures
{
    public class PgDatabaseFixture : IDatabaseFixture
    {
        private readonly PostgreSqlContainer container =
            new PostgreSqlBuilder()
                .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "password")
                .Build();

        public string DatabaseName => PgDbConnection.GetDatabaseName(ConnectionString);
        public string ConnectionString => container.GetConnectionString();
        public string ContainerId => $"{container.Id}";
        public DatabaseType DatabaseType => DatabaseType.PostgreSQL;

        public DbConnection GetDbConnection()
            => new NpgsqlConnection(ConnectionString);

        public TestDbContext GetTestDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseNpgsql(ConnectionString);

            return new TestDbContext(optionsBuilder.Options);
        }

        public Task InitializeAsync()
            => container.StartAsync();

        public Task DisposeAsync()
            => container.DisposeAsync().AsTask();
    }
}
