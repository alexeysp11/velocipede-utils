using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionIsConnectedTests : BaseDbConnectionIsConnectedTests
    {
        public MssqlDbConnectionIsConnectedTests() : base(DatabaseType.MSSQL)
        {
            _connectionString = "Data Source=YourServerName;Initial Catalog=YourDatabaseName;User ID=YourUsername;Password=YourPassword;";
        }

        [Theory]
        [InlineData("Data Source=myServer;Initial Catalog=myDatabase", "myDatabase")]
        [InlineData("Data Source=myServer;Initial Catalog=myDatabase;", "myDatabase")]
        [InlineData("Data Source=myServer;Initial Catalog=myDatabase;Connection Timeout=30;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;Connection Timeout=30;", "myDatabase")]
        [InlineData("Data Source=myServer;Initial Catalog=myDatabase;Integrated Security=True", "myDatabase")]
        [InlineData("Data Source=myServer;Initial Catalog=myDatabase;Integrated Security=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;Integrated Security=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;User Id=myUsername;Integrated Security=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;User Id=myUsername;Password=myPassword;Integrated Security=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;User Id=myUsername;Pwd=myPassword;Integrated Security=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;Uid=myUsername;Integrated Security=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;Uid=myUsername;Password=myPassword;Integrated Security=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;Uid=myUsername;Pwd=myPassword;Integrated Security=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;User Id=myUsername;Pwd=myPassword;Integrated Security=True;Encrypt=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;Uid=myUsername;Pwd=myPassword;Integrated Security=True;Encrypt=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;Uid=myUsername;Pwd=myPassword;Integrated Security=True;Encrypt=True;Trust Server Certificate=True", "myDatabase")]
        [InlineData("Data Source=myServer;Database=myDatabase;User Id=myUsername;Password=myPassword;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;", "myDatabase")]
        [InlineData("Data Source='myServer';Database=myDatabase;User Id=myUsername;Password=myPassword;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;", "myDatabase")]
        [InlineData("Data Source='myServer';Database=myDatabase;User Id=myUsername;Password='myPassword';Integrated Security=True;Encrypt=True;Trust Server Certificate=True;", "myDatabase")]
        [InlineData("Data Source='myServer';Database=myDatabase;User Id='myUsername';Password=myPassword;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;", "myDatabase")]
        [InlineData("Data Source=myServer;Database='myDatabase';User Id=myUsername;Password=myPassword;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;", "myDatabase")]
        [InlineData("Data Source='myServer';Database='myDatabase';User Id=myUsername;Password=myPassword;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;", "myDatabase")]
        [InlineData("Data Source='myServer';Database='myDatabase';User Id=myUsername;Password='myPassword';Integrated Security=True;Encrypt=True;Trust Server Certificate=True;", "myDatabase")]
        [InlineData("Data Source='myServer';Database='myDatabase';User Id='myUsername';Password='myPassword';Integrated Security=True;Encrypt=True;Trust Server Certificate=True;", "myDatabase")]
        public void GetDatabaseName_StaticGetter_DatabaseNameEqualsToExpected(string connectionString, string expectedPath)
        {
            // Arrange & Act.
            string resultPath = MssqlDbConnection.GetDatabaseName(connectionString);

            // Assert.
            resultPath.Should().Be(expectedPath);
        }
    }
}
