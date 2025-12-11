namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

/// <summary>
/// This class may not be used explicitly anywhere, but it is necessary for the correct initialization
/// of Docker containers for tests.
/// </summary>
[Collection(nameof(DatabaseFixtureResolverCollection))]
public class DatabaseFixtureResolverCollectionAnchor
{
}
