using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite
{
    public sealed class SqliteDbConnectionInitializeTests : BaseDbConnectionInitializeTests
    {
        public SqliteDbConnectionInitializeTests() : base(DatabaseType.SQLite)
        {
            _connectionString = "Data Source=mydatabase.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;";
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
        public void GetDatabaseFilePath_StaticGetter_PathEqualsToExpected(string connectionString, string expectedPath)
        {
            // Arrange & Act.
            string resultPath = SqliteDbConnection.GetDatabaseFilePath(connectionString);

            // Assert.
            resultPath.Should().Be(expectedPath);
        }

        [Theory]
        [InlineData("Data Source=mydatabase.db;", "mydatabase.db")]
        [InlineData("Data Source=mydatabase.db;Mode=ReadWriteCreate;Foreign Keys=True;", "mydatabase.db")]
        [InlineData("Data Source=mydatabase.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;", "mydatabase.db")]
        [InlineData("Data Source='mydatabase.db';", "mydatabase.db")]
        [InlineData("Data Source='mydatabase.db';Mode=ReadWriteCreate;Foreign Keys=True;", "mydatabase.db")]
        [InlineData("Data Source='mydatabase.db';Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;", "mydatabase.db")]
        [InlineData("Data Source=relative-path/mydatabase.db;", "relative-path/mydatabase.db")]
        [InlineData("Data Source='relative-path/mydatabase.db';", "relative-path/mydatabase.db")]
        [InlineData("Data Source='relative-path/mydatabase.db';Mode=ReadWriteCreate;Foreign Keys=True;", "relative-path/mydatabase.db")]
        [InlineData("Data Source='relative-path/mydatabase.db';Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;", "relative-path/mydatabase.db")]
        public void GetDatabaseFilePath_InstanceAndDatabaseFilePathField_PathEqualsToExpected(string connectionString, string expectedPath)
        {
            // Arrange.
            ICommonDbConnection dbConnection = DbConnectionsCreator.InitializeDbConnection(DatabaseType.SQLite, connectionString);

            // Act.
            string instancePath = dbConnection.DatabaseName;
            string staticPath = SqliteDbConnection.GetDatabaseFilePath(connectionString);

            // Assert.
            staticPath.Should().Be(expectedPath);
            staticPath.Should().Be(instancePath);
        }


        [Theory]
        [InlineData("mydatabase.db", "Data Source=mydatabase.db")]
        [InlineData("relative-path/mydatabase.db", "Data Source=relative-path/mydatabase.db")]
        public void GetConnectionString_StaticGetter_ConnectionStringEqualsToExpected(string path, string expectedConnectionString)
        {
            // Arrange & Act.
            string resultConnectionString = SqliteDbConnection.GetConnectionString(path);

            // Assert.
            resultConnectionString.Should().Be(expectedConnectionString);
        }

        [Theory]
        [InlineData("mydatabase.db")]
        [InlineData("relative-path/mydatabase.db")]
        public void IsDatabaseFilePathValid_ResultEqualsToFileExists(string path)
        {
            // Arrange & Act.
            bool result = SqliteDbConnection.IsDatabaseFilePathValid(path);
            bool expected = File.Exists(path);

            // Assert.
            result.Should().Be(expected);
        }
    }
}
