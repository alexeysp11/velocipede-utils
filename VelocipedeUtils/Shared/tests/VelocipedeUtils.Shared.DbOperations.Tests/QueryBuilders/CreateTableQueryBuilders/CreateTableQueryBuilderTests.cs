using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders.CreateTableQueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.Tests.QueryBuilders.CreateTableQueryBuilders;

/// <summary>
/// Unit tests for <see cref="CreateTableQueryBuilder"/>.
/// </summary>
public sealed class CreateTableQueryBuilderTests
{
    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void CreateByConstructor_ValidTableName(DatabaseType databaseType)
    {
        // Arrange.
        string tableName = "TableName";
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // Act & Assert.
        createQueryBuilder
            .Should()
            .NotBeNull();
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void CreateByConstructor_NullTableName_ThrowsArgumentNullException(DatabaseType databaseType)
    {
        // Arrange.
        string? tableName = null;
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => new CreateTableQueryBuilder(databaseType, tableName);
#nullable restore

        // Act & Assert.
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void CreateByConstructor_EmptyTableName_ThrowsArgumentException(DatabaseType databaseType)
    {
        // Arrange.
        string tableName = "";
        Func<ICreateTableQueryBuilder> act = () => new CreateTableQueryBuilder(databaseType, tableName);

        // Act & Assert.
        act
            .Should()
            .Throw<ArgumentException>();
    }
}
