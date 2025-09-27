using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base
{
    public abstract class BaseDbConnectionInitializeTests
    {
        private readonly DatabaseType _databaseType;
        protected string _connectionString;

        public BaseDbConnectionInitializeTests(DatabaseType databaseType)
        {
            _databaseType = databaseType;
            _connectionString = string.Empty;
        }

        [Fact]
        public void DatabaseType_ShouldBeEqualToSpecified()
        {
            // Arrange.
            ICommonDbConnection dbConnection = InitializeDbConnection(_databaseType);

            // Act & Assert.
            dbConnection.DatabaseType.Should().Be(_databaseType);
        }

        [Fact]
        public void SetConnectionString_SetEmptyWhenCreatingAndChange_ChangedToSpecified()
        {
            // Arrange.
            ICommonDbConnection dbConnection = InitializeDbConnection(_databaseType);

            // Act.
            string initialConnectionString = dbConnection.ConnectionString;
            dbConnection.SetConnectionString(_connectionString);

            // Assert.
            _connectionString.Should().NotBeNullOrEmpty();
            initialConnectionString.Should().BeNullOrEmpty();
            dbConnection.ConnectionString.Should().Be(_connectionString);
        }

        [Fact]
        public void SetConnectionString_SpecifyWhenCreating_EqualsToSpecified()
        {
            // Arrange.
            ICommonDbConnection dbConnection = InitializeDbConnection(_databaseType, _connectionString);

            // Act & Assert.
            _connectionString.Should().NotBeNullOrEmpty();
            dbConnection.ConnectionString.Should().Be(_connectionString);
        }

        private ICommonDbConnection InitializeDbConnection(DatabaseType databaseType, string? connectionString = null)
        {
            return databaseType switch
            {
                DatabaseType.SQLite => new SqliteDbConnection(connectionString),
                DatabaseType.PostgreSQL => new PgDbConnection(connectionString),
                DatabaseType.MSSQL => new MssqlDbConnection(connectionString),
                DatabaseType.MySQL => new MysqlDbConnection(connectionString),
                _ => throw new ArgumentException("Specified database type is not valid", nameof(databaseType))
            };
        }
    }
}
