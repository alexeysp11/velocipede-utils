using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base
{
    public abstract class BaseDbConnectionIsConnectedTests
    {
        private readonly DatabaseType _databaseType;
        protected string _connectionString;

        protected BaseDbConnectionIsConnectedTests(DatabaseType databaseType)
        {
            _databaseType = databaseType;
            _connectionString = string.Empty;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsConnected_InstanceCreatedAndNoConnectionString_NotConnected(string connectionString)
        {
            // Arrange.
            ICommonDbConnection connection = DbConnectionsCreator.InitializeDbConnection(_databaseType, connectionString);

            // Act & Assert.
            connection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void IsConnected_InstanceCreatedAndConnectionStringSet_NotConnected()
        {
            // Arrange.
            ICommonDbConnection connection = DbConnectionsCreator.InitializeDbConnection(_databaseType, _connectionString);

            // Act & Assert.
            connection.IsConnected.Should().BeFalse();
        }
    }
}
