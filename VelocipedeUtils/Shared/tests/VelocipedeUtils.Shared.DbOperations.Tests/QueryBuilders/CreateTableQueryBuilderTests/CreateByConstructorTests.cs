using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.Tests.QueryBuilders.CreateTableQueryBuilderTests;

/// <summary>
/// Unit tests for creating instance of <see cref="CreateTableQueryBuilder"/>.
/// </summary>
public sealed class CreateByConstructorTests
{
    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void CreateByConstructor_ValidTableName(VelocipedeDatabaseType databaseType)
    {
        // Arrange & Act.
        string tableName = "TableName";
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // Assert.
        createQueryBuilder
            .Should()
            .NotBeNull();
        createQueryBuilder.DatabaseType
            .Should()
            .Be(databaseType);
        createQueryBuilder.TableName
            .Should()
            .Be(tableName);
        createQueryBuilder.ColumnInfos
            .Should()
            .NotBeNull()
            .And
            .BeEmpty();
        createQueryBuilder.ForeignKeyInfos
            .Should()
            .NotBeNull()
            .And
            .BeEmpty();
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void CreateByConstructor_NullTableName_ThrowsArgumentNullException(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void CreateByConstructor_EmptyTableName_ThrowsArgumentException(VelocipedeDatabaseType databaseType)
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
