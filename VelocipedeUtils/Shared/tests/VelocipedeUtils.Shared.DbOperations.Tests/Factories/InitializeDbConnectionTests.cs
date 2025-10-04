using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Factories
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
        [InlineData(DatabaseType.Oracle, Skip = "The Oracle provider is not implemented yet")]
        [InlineData(DatabaseType.Oracle, "", Skip = "The Oracle provider is not implemented yet")]
        [InlineData(DatabaseType.Oracle, "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MyService)));User ID=myusername;Password=mypassword;", Skip = "The Oracle provider is not implemented yet")]
        [InlineData(DatabaseType.MySQL, Skip = "This test is corrently excluded due to .NET MySQL errors \"The given key was not present in the dictionary\"")]
        [InlineData(DatabaseType.MySQL, "", Skip = "This test is corrently excluded due to .NET MySQL errors \"The given key was not present in the dictionary\"")]
        [InlineData(DatabaseType.MySQL, "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;", Skip = "This test is corrently excluded due to .NET MySQL errors \"The given key was not present in the dictionary\"")]
        public void InitializeDbConnection_CreatedWithSpecifiedParams(DatabaseType databaseType, string? connectionString = null)
        {
            // Arrange & Act.
            IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);

            // Assert.
            dbConnection.Should().NotBeNull();
            dbConnection.DatabaseType.Should().Be(databaseType);
            dbConnection.ConnectionString.Should().Be(connectionString);
        }
    }
}
