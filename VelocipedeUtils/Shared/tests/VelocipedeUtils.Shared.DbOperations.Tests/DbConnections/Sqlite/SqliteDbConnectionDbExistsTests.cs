using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite;

public sealed class SqliteDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
{
    private readonly string _folderName;

    public SqliteDbConnectionDbExistsTests() : base(Enums.DatabaseType.SQLite)
    {
        _folderName = nameof(SqliteDbConnectionDbExistsTests);

        // Create directory for testing creating operations.
        if (!DatabaseFolderExists(_folderName))
            CreateDatabaseFolder(_folderName);
    }

    [Fact]
    public void DbExists_FileDoesNotExist_ReturnsFalse()
    {
        // Arrange.
        string connectionString = $"Data Source={_folderName}/{nameof(DbExists_FileDoesNotExist_ReturnsFalse)}.db;";
        string path = SqliteDbConnection.GetDatabaseName(connectionString);
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(Enums.DatabaseType.SQLite, connectionString);
        if (DatabaseExists(path))
            DeleteDatabase(path);

        // Act.
        bool result = dbConnection.DbExists();

        // Assert.
        result.Should().BeFalse();
    }

    [Fact]
    public void DbExists_FileExists_ReturnsTrue()
    {
        // Arrange.
        string connectionString = $"Data Source={_folderName}/{nameof(DbExists_FileExists_ReturnsTrue)}.db;";
        string path = SqliteDbConnection.GetDatabaseName(connectionString);
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(Enums.DatabaseType.SQLite, connectionString);
        if (DatabaseExists(path))
            DeleteDatabase(path);
        CreateDatabase(path);

        // Act.
        bool result = dbConnection.DbExists();

        // Assert.
        result.Should().BeTrue();
    }

    private static void CreateDatabaseFolder(string path) => Directory.CreateDirectory(path);
    private static bool DatabaseFolderExists(string path) => Directory.Exists(path);

    private static void CreateDatabase(string path) => File.Create(path).Close();
    private static bool DatabaseExists(string path) => File.Exists(path);
    private static void DeleteDatabase(string path) => File.Delete(path);
}
