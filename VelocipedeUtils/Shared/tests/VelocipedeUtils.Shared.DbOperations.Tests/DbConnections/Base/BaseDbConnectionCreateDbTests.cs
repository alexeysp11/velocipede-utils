using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base
{
    public abstract class BaseDbConnectionCreateDbTests
    {
        private readonly DatabaseType _databaseType;

        protected BaseDbConnectionCreateDbTests(DatabaseType databaseType)
        {
            _databaseType = databaseType;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DbCreate_ConnectionStringIsNullOrEmpty_ThrowsInvalidOperationException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType, connectionString);
            Action act = () => dbConnection.CreateDb();

            // Act & Assert.
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
