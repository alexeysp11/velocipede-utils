using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
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
        createQueryBuilder.DatabaseType
            .Should()
            .Be(databaseType);
        createQueryBuilder.TableName
            .Should()
            .Be(tableName);
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
        Func<CreateTableQueryBuilder> act = () => new CreateTableQueryBuilder(databaseType, tableName);
#nullable restore

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeTableNameException>()
            .WithMessage(ErrorMessageConstants.IncorrectTableName);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void CreateByConstructor_EmptyTableName_ThrowsArgumentException(DatabaseType databaseType)
    {
        // Arrange.
        string tableName = "";
        Func<CreateTableQueryBuilder> act = () => new CreateTableQueryBuilder(databaseType, tableName);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeTableNameException>()
            .WithMessage(ErrorMessageConstants.IncorrectTableName);
    }
}
