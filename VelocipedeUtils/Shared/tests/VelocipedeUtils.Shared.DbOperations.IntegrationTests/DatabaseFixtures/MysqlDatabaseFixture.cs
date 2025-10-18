// This class is corrently excluded due to .NET MySQL errors "The given key was not present in the dictionary".

#define EXCLUDE_MYSQL_PROVIDER

#if !EXCLUDE_MYSQL_PROVIDER

using System.Data.Common;
using MySql.Data.MySqlClient;
using VelocipedeUtils.Shared.DbOperations.Enums;
using Testcontainers.MySql;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures
{
    public sealed class MysqlDatabaseFixture : IDatabaseFixture
    {
        private readonly MySqlContainer container =
            new MySqlBuilder()
                .Build();

        public string DatabaseName => string.Empty;
        public string ConnectionString => container.GetConnectionString();
        public string ContainerId => $"{container.Id}";
        public DatabaseType DatabaseType => DatabaseType.MySQL;

        public DbConnection GetDbConnection()
            => new MySqlConnection(ConnectionString);

        public TestDbContext GetTestDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

            return new TestDbContext(optionsBuilder.Options);
        }

        public Task InitializeAsync()
            => container.StartAsync();

        public Task DisposeAsync()
            => container.DisposeAsync().AsTask();
    }
}

#endif
