using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionInitializeTests : BaseDbConnectionInitializeTests
    {
        public PostgresDbConnectionInitializeTests() : base(DatabaseType.PostgreSQL)
        {
            _connectionString = "Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=mydatabase;";
        }

        [Theory]
        [InlineData("Data Source=mydatabase.db;", "mydatabase.db")]
        [InlineData("Data Source=mydatabase.db;Mode=ReadWriteCreate;Foreign Keys=True;", "mydatabase.db")]
        [InlineData("Data Source=mydatabase.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;", "mydatabase.db")]
        [InlineData("Data Source='mydatabase.db';", "mydatabase.db")]
        [InlineData("Data Source='mydatabase.db';Mode=ReadWriteCreate;Foreign Keys=True;", "mydatabase.db")]
        [InlineData("Data Source='mydatabase.db';Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;", "mydatabase.db")]
        [InlineData("Data Source='relative-path/mydatabase.db';", "relative-path/mydatabase.db")]
        [InlineData("Data Source='relative-path/mydatabase.db';Mode=ReadWriteCreate;Foreign Keys=True;", "relative-path/mydatabase.db")]
        [InlineData("Data Source='relative-path/mydatabase.db';Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;", "relative-path/mydatabase.db")]
        public void GetDatabaseName_StaticGetter_DatabaseNameEqualsToExpected(string connectionString, string expectedPath)
        {
            // Arrange & Act.
            string resultPath = PgDbConnection.GetDatabaseName(connectionString);

            // Assert.
            resultPath.Should().Be(expectedPath);
        }
    }
}
