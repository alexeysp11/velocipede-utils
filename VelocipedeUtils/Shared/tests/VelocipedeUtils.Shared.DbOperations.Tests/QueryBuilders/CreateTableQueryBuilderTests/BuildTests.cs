using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.Tests.QueryBuilders.CreateTableQueryBuilderTests;

/// <summary>
/// Unit tests for <see cref="CreateTableQueryBuilder.Build"/>.
/// </summary>
public sealed class BuildTests
{
    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void BuildOnce_WithColumnByParameters_QueryBuilderIsBuilt(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Initialize query builder.
        string tableName = "TableName";
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // 2. Add column.
        createQueryBuilder.WithColumn(columnName: "ColumnName", columnType: DbType.String);

        // Act.
        createQueryBuilder.Build();

        // Assert.
        createQueryBuilder.IsBuilt
            .Should()
            .BeTrue();
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void BuildOnce_WithColumnByObject_QueryBuilderIsBuilt(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Initialize query builder.
        string tableName = "TableName";
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // 2. Add column.
        createQueryBuilder.WithColumn(new()
        {
            DatabaseType = databaseType,
            ColumnName = "ColumnName",
            ColumnType = DbType.String
        });

        // Act.
        createQueryBuilder.Build();

        // Assert.
        createQueryBuilder.IsBuilt
            .Should()
            .BeTrue();
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void BuildOnce_NoClumns(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        string tableName = "TableName";
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        Func<ICreateTableQueryBuilder> act = createQueryBuilder.Build;

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.QueryBuilderRequiresColumn);
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void BuildTwice_ThrowsVelocipedeQueryBuilderException(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Initialize query builder.
        string tableName = "TableName";
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // 2. Add column.
        createQueryBuilder.WithColumn(new()
        {
            DatabaseType = databaseType,
            ColumnName = "ColumnName",
            ColumnType = DbType.String
        });
        
        // 3. Build twice.
        createQueryBuilder.Build();
        Func<ICreateTableQueryBuilder> act = createQueryBuilder.Build;

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.QueryBuilderIsBuilt);
    }
}
