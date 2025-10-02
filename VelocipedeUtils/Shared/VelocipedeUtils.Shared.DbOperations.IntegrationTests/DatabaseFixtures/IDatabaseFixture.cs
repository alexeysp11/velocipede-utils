using System.Data.Common;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures
{
    public interface IDatabaseFixture : IAsyncLifetime
    {
        string ConnectionString { get; }
        string ContainerId { get; }
        DatabaseType DatabaseType { get; }
        DbConnection GetDbConnection();
    }
}
