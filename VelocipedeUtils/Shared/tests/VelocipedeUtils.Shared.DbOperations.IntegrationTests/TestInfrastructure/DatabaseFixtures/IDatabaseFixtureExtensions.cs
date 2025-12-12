using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

/// <summary>
/// Extensions for working with <see cref="IDatabaseFixture"/>.
/// </summary>
public static class IDatabaseFixtureExtensions
{
    /// <summary>
    /// Get database connection that implements <see cref="IVelocipedeDbConnection"/> interface.
    /// </summary>
    /// <param name="fixture">Instance of <see cref="IDatabaseFixture"/>.</param>
    /// <returns>A new instance of <see cref="IVelocipedeDbConnection"/>.</returns>
    public static IVelocipedeDbConnection GetVelocipedeDbConnection(this IDatabaseFixture fixture)
    {
        return VelocipedeDbConnectionFactory.InitializeDbConnection(fixture.DatabaseType, fixture.ConnectionString);
    }
}
