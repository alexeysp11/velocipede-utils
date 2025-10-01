using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Oracle
{
    public sealed class OracleDbConnectionInitializeTests : BaseDbConnectionInitializeTests
    {
        public OracleDbConnectionInitializeTests() : base(DatabaseType.Oracle)
        {
            _connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MyService)));User ID=myusername;Password=mypassword;";
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
            string resultPath = OracleDbConnection.GetDatabaseName(connectionString);

            // Assert.
            resultPath.Should().Be(expectedPath);
        }
    }
}
