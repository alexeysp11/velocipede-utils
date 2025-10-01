using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mysql
{
    public class MysqlDbConnectionInitializeTests : BaseDbConnectionInitializeTests
    {
        public MysqlDbConnectionInitializeTests() : base(DatabaseType.MySQL)
        {
            _connectionString = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
        }

        [Theory]
        [InlineData("Server=localhost;Port=3306;Database=my_database", "my_database")]
        [InlineData("Server=localhost;Port=3306;Database=my_database;", "my_database")]
        [InlineData("Server=localhost;Port=3306;Database=my_database;Uid=root;Pwd=password;", "my_database")]
        [InlineData("Server=localhost;Port=3306;Database='my_database';Uid=root;Pwd=password;", "my_database")]
        [InlineData("Server='localhost';Port=3306;Database=my_database;Uid=root;Pwd=password;", "my_database")]
        [InlineData("Server='localhost';Port=3306;Database=my_database;Uid='root';Pwd=password;", "my_database")]
        [InlineData("Server='localhost';Port=3306;Database=my_database;Uid='root';Pwd='password';", "my_database")]
        [InlineData("Server='localhost';Port=3306;Database='my_database';Uid='root';Pwd='password';", "my_database")]
        [InlineData("Server=localhost;Port=3306;Database=my_database;Uid=root;Pwd=password;SslMode=None", "my_database")]
        [InlineData("Server=localhost;Port=3306;Database=my_database;Uid=root;Pwd=password;SslMode=None;", "my_database")]
        [InlineData("Server=localhost;Port=3306;Database=my_database;Uid=root;Pwd=password;SslMode=None;Connection Timeout=30;", "my_database")]
        [InlineData("Server=localhost;Port=3306;Database=my_database;Uid=root;Pwd=password;SslMode=None;Connection Timeout=30;Pooling=false", "my_database")]
        public void GetDatabaseName_StaticGetter_DatabaseNameEqualsToExpected(string connectionString, string expectedPath)
        {
            // Arrange & Act.
            string resultPath = MysqlDbConnection.GetDatabaseName(connectionString);

            // Assert.
            resultPath.Should().Be(expectedPath);
        }
    }
}
