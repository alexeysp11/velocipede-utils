namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

/// <summary>
/// Used to share a single instance of <see cref="DatabaseFixtureResolver"/> between tests in multiple test classes.
/// </summary>
[CollectionDefinition("DatabaseFixtureResolverCollection")]
public class DatabaseFixtureResolverCollection : ICollectionFixture<DatabaseFixtureResolver>
{
}
