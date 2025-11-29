namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

/// <summary>
/// Used to share a single instance of <see cref="MssqlDatabaseFixture"/> between tests in multiple test classes.
/// </summary>
[CollectionDefinition(nameof(MssqlDatabaseFixtureCollection))]
public class MssqlDatabaseFixtureCollection : ICollectionFixture<MssqlDatabaseFixture>
{
}
