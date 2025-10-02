using Testcontainers.PostgreSql;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DatabaseFixtures
{
    public class PgDatabaseFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer container =
            new PostgreSqlBuilder()
                .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "password")
                .Build();

        public string ConnectionString => container.GetConnectionString();
        public string ContainerId => $"{container.Id}";

        public Task InitializeAsync()
            => container.StartAsync();

        public Task DisposeAsync()
            => container.DisposeAsync().AsTask();
    }
}
