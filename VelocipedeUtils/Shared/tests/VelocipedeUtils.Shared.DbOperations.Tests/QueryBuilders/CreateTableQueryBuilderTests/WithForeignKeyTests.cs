using VelocipedeUtils.Shared.DbOperations.Models;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeyByObject_NullForeignKey(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeyByObject_NotNullForeignKey(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeyByObject_TwoNotNullForeignKeys(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeyByObject_BuildFirstAndNullForeignKey(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeyByObject_BuildFirstAndNotNullForeignKey(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeys_NullList(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeys_EmptyList(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeys_ValidList(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeys_BuildFirstAndNullList(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeys_BuildFirstAndEmptyList(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithForeignKeys_BuildFirstAndValidList(VelocipedeDatabaseType databaseType)
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
