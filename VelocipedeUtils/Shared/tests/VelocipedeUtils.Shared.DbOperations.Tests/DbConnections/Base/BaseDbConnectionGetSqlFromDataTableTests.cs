using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base
{
    public abstract class BaseDbConnectionGetSqlFromDataTableTests
    {
        private readonly DatabaseType _databaseType;

        protected BaseDbConnectionGetSqlFromDataTableTests(DatabaseType databaseType)
        {
            _databaseType = databaseType;
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData(null, "TestName")]
        public void GetSqlFromDataTable_OneOfTheParametersIsNull_ReturnsException(DataTable dt, string tableName)
        {
            // Arrange.
            ICommonDbConnection dbConnection = DbConnectionsCreator.InitializeDbConnection(_databaseType);
            Action act = () => dbConnection.GetSqlFromDataTable(dt, tableName);

            // Act & Assert.
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
