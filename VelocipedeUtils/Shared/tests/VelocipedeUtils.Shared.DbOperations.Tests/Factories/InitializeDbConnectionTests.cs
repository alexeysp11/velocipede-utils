using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Factories;

/// <summary>
/// Class for unit testing <see cref="VelocipedeDbConnectionFactory"/>: initialize database connection.
/// </summary>
public class InitializeDbConnectionTests
{
    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.SQLite, "")]
    [InlineData(VelocipedeDatabaseType.SQLite, "Data Source=mydatabase.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;")]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL, "")]
    [InlineData(VelocipedeDatabaseType.PostgreSQL, "Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=mydatabase;")]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL, "")]
    [InlineData(VelocipedeDatabaseType.MSSQL, "Data Source=YourServerName;Initial Catalog=YourDatabaseName;User ID=YourUsername;Password=YourPassword;")]
    [InlineData(VelocipedeDatabaseType.Oracle, Skip = "The Oracle provider is not implemented yet")]
    [InlineData(VelocipedeDatabaseType.Oracle, "", Skip = "The Oracle provider is not implemented yet")]
    [InlineData(VelocipedeDatabaseType.Oracle, "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MyService)));User ID=myusername;Password=mypassword;", Skip = "The Oracle provider is not implemented yet")]
    [InlineData(VelocipedeDatabaseType.MySQL, Skip = "This test is corrently excluded due to .NET MySQL errors \"The given key was not present in the dictionary\"")]
    [InlineData(VelocipedeDatabaseType.MySQL, "", Skip = "This test is corrently excluded due to .NET MySQL errors \"The given key was not present in the dictionary\"")]
    [InlineData(VelocipedeDatabaseType.MySQL, "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;", Skip = "This test is corrently excluded due to .NET MySQL errors \"The given key was not present in the dictionary\"")]
    public void InitializeDbConnection_CreatedWithSpecifiedParams(VelocipedeDatabaseType databaseType, string? connectionString = null)
    {
        // Arrange & Act.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);

        // Assert.
        dbConnection.Should().NotBeNull();
        dbConnection.DatabaseType.Should().Be(databaseType);
        dbConnection.ConnectionString.Should().Be(connectionString);
    }
}
