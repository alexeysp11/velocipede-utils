using VelocipedeUtils.Shared.DbOperations.QueryBuilders.CreateTableQueryBuilders;
using VelocipedeUtils.Shared.DbOperations.Models;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.QueryBuilders.CreateTableQueryBuilderTests;

/// <summary>
/// Unit tests for the following methods:
/// <list type="bullet">
/// <item><description>By object: <see cref="CreateTableQueryBuilder.WithForeignKey"/></description></item>
/// <item><description>By list: <see cref="CreateTableQueryBuilder.WithForeignKeys"/></description></item>
/// </list>
/// </summary>
public sealed class WithForeignKeyTests
{
    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void WithForeignKeyByObject_NullForeignKey(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeForeignKeyInfo? foreignKeyInfo = null;

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithForeignKey(foreignKeyInfo);
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
    public void WithForeignKeyByObject_NotNullForeignKey(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeForeignKeyInfo foreignKeyInfo = new()
        {
            DatabaseType = databaseType,
            FromColumn = "FromColumnName",
            ToColumn = "ToColumnName",
        };

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // Act.
        createQueryBuilder.WithForeignKey(foreignKeyInfo);

        // Assert.
        createQueryBuilder.ForeignKeyInfos
            .Should()
            .HaveCount(1)
            .And
            .Contain(foreignKeyInfo);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void WithForeignKeyByObject_TwoNotNullForeignKeys(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeForeignKeyInfo foreignKeyInfo1 = new()
        {
            DatabaseType = databaseType,
            FromColumn = "FromColumnName1",
            ToColumn = "ToColumnName1",
        };
        VelocipedeForeignKeyInfo foreignKeyInfo2 = new()
        {
            DatabaseType = databaseType,
            FromColumn = "FromColumnName2",
            ToColumn = "ToColumnName2",
        };

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // Act.
        createQueryBuilder
            .WithForeignKey(foreignKeyInfo1)
            .WithForeignKey(foreignKeyInfo2);

        // Assert.
        createQueryBuilder.ForeignKeyInfos
            .Should()
            .HaveCount(2)
            .And
            .Contain(foreignKeyInfo1)
            .And
            .Contain(foreignKeyInfo2);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void WithForeignKeyByObject_BuildFirstAndNullForeignKey(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeForeignKeyInfo? foreignKeyInfo = null;

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithForeignKey(foreignKeyInfo);
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
    public void WithForeignKeyByObject_BuildFirstAndNotNullForeignKey(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeForeignKeyInfo foreignKeyInfo = new()
        {
            DatabaseType = databaseType,
            FromColumn = "FromColumnName",
            ToColumn = "ToColumnName",
        };

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithForeignKey(foreignKeyInfo);

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
    public void WithForeignKeys_NullList(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeForeignKeyInfo>? columnInfos = null;

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithForeignKeys(columnInfos);
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
    public void WithForeignKeys_EmptyList(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeForeignKeyInfo> columnInfos = [];

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithForeignKeys(columnInfos);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.EmptyForeignKeyInfoList);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void WithForeignKeys_ValidList(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeForeignKeyInfo> foreignKeyInfo =
        [
            new() { DatabaseType = databaseType, FromColumn = "FromColumnName1", ToColumn = "ToColumnName1", },
            new() { DatabaseType = databaseType, FromColumn = "FromColumnName2", ToColumn = "ToColumnName2", },
        ];

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // Act.
        createQueryBuilder.WithForeignKeys(foreignKeyInfo);

        // Assert.
        createQueryBuilder.ForeignKeyInfos
            .Should()
            .BeEquivalentTo(foreignKeyInfo);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void WithForeignKeys_BuildFirstAndNullList(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeForeignKeyInfo>? columnInfos = null;

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithForeignKeys(columnInfos);
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
    public void WithForeignKeys_BuildFirstAndEmptyList(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeForeignKeyInfo> columnInfos = [];

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithForeignKeys(columnInfos);

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
    public void WithForeignKeys_BuildFirstAndValidList(DatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeForeignKeyInfo> foreignKeyInfo =
        [
            new() { DatabaseType = databaseType, FromColumn = "FromColumnName1", ToColumn = "ToColumnName1", },
            new() { DatabaseType = databaseType, FromColumn = "FromColumnName2", ToColumn = "ToColumnName2", },
        ];

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithForeignKeys(foreignKeyInfo);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.QueryBuilderIsBuilt);
    }
}
