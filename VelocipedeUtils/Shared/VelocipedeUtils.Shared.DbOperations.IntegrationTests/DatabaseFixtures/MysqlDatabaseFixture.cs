using System.Data.Common;
using MySql.Data.MySqlClient;
using VelocipedeUtils.Shared.DbOperations.Enums;
using Testcontainers.MySql;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures
{
    public sealed class MysqlDatabaseFixture : IDatabaseFixture
    {
        private readonly MySqlContainer container =
            new MySqlBuilder()
                .Build();

        public string ConnectionString => container.GetConnectionString();
        public string ContainerId => $"{container.Id}";
        public DatabaseType DatabaseType => DatabaseType.MySQL;

        public DbConnection GetDbConnection()
            => new MySqlConnection(ConnectionString);

        public Task InitializeAsync()
            => container.StartAsync();

        public Task DisposeAsync()
            => container.DisposeAsync().AsTask();
    }
}
