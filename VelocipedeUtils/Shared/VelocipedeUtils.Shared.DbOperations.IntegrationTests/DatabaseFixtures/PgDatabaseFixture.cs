using System.Data.Common;
using Npgsql;
using Testcontainers.PostgreSql;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DatabaseFixtures
{
    public class PgDatabaseFixture : IDatabaseFixture
    {
        private readonly PostgreSqlContainer container =
            new PostgreSqlBuilder()
                .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "password")
                .Build();

        public string ConnectionString => container.GetConnectionString();
        public string ContainerId => $"{container.Id}";
        public DatabaseType DatabaseType => DatabaseType.PostgreSQL;

        public DbConnection GetDbConnection()
            => new NpgsqlConnection(ConnectionString);

        public Task InitializeAsync()
            => container.StartAsync();

        public Task DisposeAsync()
            => container.DisposeAsync().AsTask();
    }
}
