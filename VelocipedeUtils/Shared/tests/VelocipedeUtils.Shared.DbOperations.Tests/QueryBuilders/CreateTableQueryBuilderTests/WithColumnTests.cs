using FluentAssertions;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders.CreateTableQueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.Tests.QueryBuilders.CreateTableQueryBuilderTests;

/// <summary>
/// Unit tests for the following methods:
/// <list type="bullet">
/// <item><description>By parameters: <see cref="CreateTableQueryBuilder.WithColumn(string, DbType, int?, int?, int?, object?, bool, bool)"/></description></item>
/// <item><description>By object: <see cref="CreateTableQueryBuilder.WithColumn(VelocipedeColumnInfo)"/></description></item>
/// </list>
/// </summary>
public sealed class WithColumnTests
{
    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void WithColumnByObject_NullColumn(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeColumnInfo? columnInfo = null;

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithColumn(columnInfo);
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
    public void WithColumnByObject_NotNullColumn(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = databaseType,
            ColumnName = "ColumnName",
            DbType = DbType.String,
        };

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // Act.
        createQueryBuilder.WithColumn(columnInfo);

        // Assert.
        createQueryBuilder.ColumnInfos
            .Should()
            .HaveCount(1)
            .And
            .Contain(columnInfo);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void WithColumnByObject_BuildFirstAndNullColumn(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeColumnInfo? columnInfo = null;

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithColumn(columnInfo);
#nullable restore

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.QueryBuilderIsBuilt);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void WithColumnByObject_BuildFirstAndNotNullColumn(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = databaseType,
            ColumnName = "ColumnName",
            DbType = DbType.String,
        };

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithColumn(columnInfo);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.QueryBuilderIsBuilt);
    }
}
