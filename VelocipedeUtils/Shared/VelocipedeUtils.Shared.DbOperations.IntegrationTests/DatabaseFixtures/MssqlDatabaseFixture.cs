using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures
{
    public sealed class MssqlDatabaseFixture : IDatabaseFixture
    {
        private readonly MsSqlContainer container =
            new MsSqlBuilder()
                .Build();

        public string ConnectionString => container.GetConnectionString();
        public string ContainerId => $"{container.Id}";
        public DatabaseType DatabaseType => DatabaseType.MSSQL;

        public DbConnection GetDbConnection()
            => new SqlConnection(ConnectionString);

        public TestDbContext GetTestDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            return new TestDbContext(optionsBuilder.Options);
        }

        public Task InitializeAsync()
            => container.StartAsync();

        public Task DisposeAsync()
            => container.DisposeAsync().AsTask();
    }
}
