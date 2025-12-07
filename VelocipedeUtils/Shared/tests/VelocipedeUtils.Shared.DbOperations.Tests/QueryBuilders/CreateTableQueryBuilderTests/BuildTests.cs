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
    public void BuildOnce_QuryBuilderIsBuilt(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        string tableName = "TableName";
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);

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
    public void BuildTwice_ThrowsVelocipedeQueryBuilderException(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        string tableName = "TableName";
        CreateTableQueryBuilder createQueryBuilder = new(databaseType, tableName);
        createQueryBuilder.Build();
        Func<ICreateTableQueryBuilder> act = createQueryBuilder.Build;

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.QueryBuilderIsBuilt);
    }
}
