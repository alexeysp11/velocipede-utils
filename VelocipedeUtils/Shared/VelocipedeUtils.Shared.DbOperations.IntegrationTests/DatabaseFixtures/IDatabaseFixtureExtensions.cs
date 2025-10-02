using VelocipedeUtils.Shared.DbOperations.DbConnections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures
{
    public static class IDatabaseFixtureExtensions
    {
        public static IVelocipedeDbConnection GetVelocipedeDbConnection(this IDatabaseFixture fixture)
        {
            return VelocipedeDbConnectionFactory.InitializeDbConnection(fixture.DatabaseType, fixture.ConnectionString);
        }
    }
}
