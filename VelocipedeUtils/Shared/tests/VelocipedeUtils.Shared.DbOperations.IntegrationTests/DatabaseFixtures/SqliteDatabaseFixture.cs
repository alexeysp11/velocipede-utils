using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures
{
    public class SqliteDatabaseFixture : IDatabaseFixture
    {
        public string DatabaseName => SqliteDbConnection.GetDatabaseName(ConnectionString);

        public string ConnectionString { get; private set; }

        public string ContainerId { get; private set; }

        public DatabaseType DatabaseType => DatabaseType.SQLite;

        public SqliteDatabaseFixture()
        {
            ContainerId = Guid.NewGuid().ToString();
            ConnectionString = SqliteDbConnection.GetConnectionString($"{ContainerId}.db");
        }

        public DbConnection GetDbConnection()
            => new SqliteConnection(ConnectionString);

        public TestDbContext GetTestDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlite(ConnectionString);

            return new TestDbContext(optionsBuilder.Options);
        }

        public Task InitializeAsync()
        {
            if (!File.Exists(DatabaseName))
            {
                File.Create(DatabaseName).Close();
            }
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            if (File.Exists(DatabaseName))
            {
                SqliteConnection.ClearAllPools();
                File.Delete(DatabaseName);
            }
            return Task.CompletedTask;
        }
    }
}
