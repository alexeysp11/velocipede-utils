using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Creator
{
    public class InitializeDbConnectionTests
    {
        [Theory]
        [InlineData(DatabaseType.SQLite)]
        [InlineData(DatabaseType.SQLite, "")]
        [InlineData(DatabaseType.SQLite, "Data Source=mydatabase.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;")]
        [InlineData(DatabaseType.PostgreSQL)]
        [InlineData(DatabaseType.PostgreSQL, "")]
        [InlineData(DatabaseType.PostgreSQL, "Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=mydatabase;")]
        [InlineData(DatabaseType.MSSQL)]
        [InlineData(DatabaseType.MSSQL, "")]
        [InlineData(DatabaseType.MSSQL, "Data Source=YourServerName;Initial Catalog=YourDatabaseName;User ID=YourUsername;Password=YourPassword;")]
        [InlineData(DatabaseType.Oracle)]
        [InlineData(DatabaseType.Oracle, "")]
        [InlineData(DatabaseType.Oracle, "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MyService)));User ID=myusername;Password=mypassword;")]
        [InlineData(DatabaseType.MySQL)]
        [InlineData(DatabaseType.MySQL, "")]
        [InlineData(DatabaseType.MySQL, "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;")]
        public void InitializeDbConnection_CreatedWithSpecifiedParams(DatabaseType databaseType, string? connectionString = null)
        {
            // Arrange & Act.
            ICommonDbConnection dbConnection = DbConnectionsCreator.InitializeDbConnection(databaseType, connectionString);

            // Assert.
            dbConnection.Should().NotBeNull();
            dbConnection.DatabaseType.Should().Be(databaseType);
            dbConnection.ConnectionString.Should().Be(connectionString);
        }
    }
}
