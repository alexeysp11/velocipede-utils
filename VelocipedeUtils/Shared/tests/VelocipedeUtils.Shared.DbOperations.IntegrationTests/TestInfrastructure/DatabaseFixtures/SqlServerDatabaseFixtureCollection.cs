namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

/// <summary>
/// Used to share a single instance of <see cref="SqlServerDatabaseFixture"/> between tests in multiple test classes.
/// </summary>
[CollectionDefinition(nameof(SqlServerDatabaseFixtureCollection))]
public class SqlServerDatabaseFixtureCollection : ICollectionFixture<SqlServerDatabaseFixture>
{
}
