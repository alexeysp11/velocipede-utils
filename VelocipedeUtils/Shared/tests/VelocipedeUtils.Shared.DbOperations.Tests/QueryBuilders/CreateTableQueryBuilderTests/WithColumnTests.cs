using FluentAssertions;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.Tests.QueryBuilders.CreateTableQueryBuilderTests;

/// <summary>
/// Unit tests for the following methods:
/// <list type="bullet">
/// <item><description>By parameters: <see cref="CreateTableQueryBuilder.WithColumn(string, DbType, int?, int?, int?, object?, bool, bool)"/></description></item>
/// <item><description>By object: <see cref="CreateTableQueryBuilder.WithColumn(VelocipedeColumnInfo)"/></description></item>
/// <item><description>By list: <see cref="CreateTableQueryBuilder.WithColumns"/></description></item>
/// </list>
/// </summary>
public sealed class WithColumnTests
{
    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithColumnByObject_NullColumn(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithColumnByObject_NotNullColumn(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = databaseType,
            ColumnName = "ColumnName",
            ColumnType = DbType.String,
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithColumnByObject_TwoNotNullColumns(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeColumnInfo columnInfo1 = new()
        {
            DatabaseType = databaseType,
            ColumnName = "ColumnName1",
            ColumnType = DbType.Int32,
        };
        VelocipedeColumnInfo columnInfo2 = new()
        {
            DatabaseType = databaseType,
            ColumnName = "ColumnName2",
            ColumnType = DbType.String,
        };

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // Act.
        createQueryBuilder
            .WithColumn(columnInfo1)
            .WithColumn(columnInfo2);

        // Assert.
        createQueryBuilder.ColumnInfos
            .Should()
            .HaveCount(2)
            .And
            .Contain(columnInfo1)
            .And
            .Contain(columnInfo2);
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithColumnByObject_BuildFirstAndNullColumn(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithColumnByObject_BuildFirstAndNotNullColumn(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = databaseType,
            ColumnName = "ColumnName",
            ColumnType = DbType.String,
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

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithColumns_NullList(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeColumnInfo>? columnInfos = null;

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithColumns(columnInfos);
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
    public void WithColumns_EmptyList(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeColumnInfo> columnInfos = [];

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithColumns(columnInfos);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.EmptyColumnInfoList);
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithColumns_ValidList(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeColumnInfo> columnInfos =
        [
            new() { DatabaseType = databaseType, ColumnName = "ColumnName1", ColumnType = DbType.Int32, },
            new() { DatabaseType = databaseType, ColumnName = "ColumnName2", ColumnType = DbType.String, },
        ];

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

        // Act.
        createQueryBuilder.WithColumns(columnInfos);

        // Assert.
        createQueryBuilder.ColumnInfos
            .Should()
            .BeEquivalentTo(columnInfos);
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void WithColumns_BuildFirstAndNullList(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeColumnInfo>? columnInfos = null;

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithColumns(columnInfos);
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
    public void WithColumns_BuildFirstAndEmptyList(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeColumnInfo> columnInfos = [];

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithColumns(columnInfos);

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
    public void WithColumns_BuildFirstAndValidList(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        // 1. Database object info.
        string tableName = "TableName";
        List<VelocipedeColumnInfo> columnInfos =
        [
            new() { DatabaseType = databaseType, ColumnName = "ColumnName1", ColumnType = DbType.Int32, },
            new() { DatabaseType = databaseType, ColumnName = "ColumnName2", ColumnType = DbType.String, },
        ];

        // 2. Query builder.
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
        Func<ICreateTableQueryBuilder> act = () => createQueryBuilder.WithColumns(columnInfos);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.QueryBuilderIsBuilt);
    }
}
