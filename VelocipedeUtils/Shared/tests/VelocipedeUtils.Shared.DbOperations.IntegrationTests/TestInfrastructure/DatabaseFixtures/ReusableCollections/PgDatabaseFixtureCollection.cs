namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

/// <summary>
/// Used to share a single instance of <see cref="PgDatabaseFixture"/> between tests in multiple test classes.
/// </summary>
[CollectionDefinition(nameof(PgDatabaseFixtureCollection))]
public class PgDatabaseFixtureCollection : ICollectionFixture<PgDatabaseFixture>
{
}
