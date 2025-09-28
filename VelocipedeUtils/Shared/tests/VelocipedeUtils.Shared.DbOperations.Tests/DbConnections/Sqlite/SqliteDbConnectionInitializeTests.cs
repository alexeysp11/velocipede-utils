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
        [InlineData("Data Source='relative-path/mydatabase.db';", "relative-path/mydatabase.db")]
        [InlineData("Data Source='relative-path/mydatabase.db';Mode=ReadWriteCreate;Foreign Keys=True;", "relative-path/mydatabase.db")]
        [InlineData("Data Source='relative-path/mydatabase.db';Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;", "relative-path/mydatabase.db")]
        public void GetDatabaseFilePath_InstanceAndDatabaseFilePathField_PathEqualsToExpected(string connectionString, string expectedPath)
        {
            // Arrange.
            ICommonDbConnection dbConnection = DbConnectionsCreator.InitializeDbConnection(DatabaseType.SQLite, connectionString);
            if (dbConnection == null || dbConnection.DatabaseType != DatabaseType.SQLite)
            {
                throw new InvalidOperationException("Incorrect type of the created database connection: SQLite expected");
            }
            SqliteDbConnection sqliteDbConnection = (SqliteDbConnection)dbConnection;

            // Act.
            string instancePath = sqliteDbConnection.DatabaseFilePath;
            string staticPath = SqliteDbConnection.GetDatabaseFilePath(connectionString);

            // Assert.
            staticPath.Should().Be(expectedPath);
            staticPath.Should().Be(instancePath);
        }
    }
}
