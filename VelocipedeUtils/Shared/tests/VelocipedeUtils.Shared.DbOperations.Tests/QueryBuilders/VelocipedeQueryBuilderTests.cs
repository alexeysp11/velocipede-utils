using FluentAssertions;
using Moq;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders.CreateTableQueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.Tests.QueryBuilders;

/// <summary>
/// Unit tests for <see cref="VelocipedeQueryBuilder"/>.
/// </summary>
public sealed class VelocipedeQueryBuilderTests
{
    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    [InlineData(DatabaseType.MySQL)]
    [InlineData(DatabaseType.MariaDB)]
    public void CreateByDefaultConstructor(DatabaseType databaseType)
    {
        VelocipedeQueryBuilder queryBuilder = new(databaseType);
        queryBuilder
            .Should()
            .NotBeNull();
        queryBuilder.DatabaseType
            .Should()
            .Be(databaseType);
        queryBuilder.DbConnection
            .Should()
            .BeNull();
    }

    [Theory]
    [InlineData(DatabaseType.None)]
    [InlineData(DatabaseType.InMemory)]
    public void CreateByDefaultConstructor_IncorrectDatabaseType(DatabaseType databaseType)
    {
        Func<VelocipedeQueryBuilder> act = () => new VelocipedeQueryBuilder(databaseType);
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.UnableToCreateQueryBuilderForDbType);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite, typeof(SqliteDbConnection))]
    [InlineData(DatabaseType.PostgreSQL, typeof(PgDbConnection))]
    [InlineData(DatabaseType.MSSQL, typeof(MssqlDbConnection))]
    public void CreateByConstructor_SpecifyDbConnection(DatabaseType databaseType, Type expectedDbConnectionType)
    {
        // Arrange & Act.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType);
        VelocipedeQueryBuilder queryBuilder = new(databaseType, dbConnection);

        // Act.
        queryBuilder
            .Should()
            .NotBeNull();
        queryBuilder.DatabaseType
            .Should()
            .Be(databaseType);
        queryBuilder.DbConnection
            .Should()
            .NotBeNull()
            .And
            .BeOfType(expectedDbConnectionType);
    }

    [Theory]
    [InlineData(DatabaseType.None)]
    [InlineData(DatabaseType.InMemory)]
    public void CreateByConstructor_SpecifyDbConnectionAndIncorrectDatabaseType(DatabaseType databaseType)
    {
        using IVelocipedeDbConnection dbConnection = Mock.Of<IVelocipedeDbConnection>();
        Func<VelocipedeQueryBuilder> act = () => new VelocipedeQueryBuilder(databaseType, dbConnection);
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.UnableToCreateQueryBuilderForDbType);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite, typeof(SqliteDbConnection))]
    [InlineData(DatabaseType.PostgreSQL, typeof(PgDbConnection))]
    [InlineData(DatabaseType.MSSQL, typeof(MssqlDbConnection))]
    public void CreateByDbConnection_UseDbConnectionMethod(DatabaseType databaseType, Type expectedDbConnectionType)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType);
        IVelocipedeQueryBuilder queryBuilder = dbConnection.GetQueryBuilder();

        // Act.
        queryBuilder
            .Should()
            .NotBeNull();
        queryBuilder.DatabaseType
            .Should()
            .Be(databaseType);
        queryBuilder.DbConnection
            .Should()
            .NotBeNull()
            .And
            .BeOfType(expectedDbConnectionType);
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    public void CreateTable_NullTableName_ThrowsArgumentNullException(DatabaseType databaseType)
    {
        // Arrange.
        string? tableName = null;
        VelocipedeQueryBuilder queryBuilder = new(databaseType);
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => queryBuilder.CreateTable(tableName);
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
    public void CreateTable_EmptyTableName_ThrowsArgumentException(DatabaseType databaseType)
    {
        // Arrange.
        string? tableName = null;
        VelocipedeQueryBuilder queryBuilder = new(databaseType);
#nullable disable
        Func<ICreateTableQueryBuilder> act = () => queryBuilder.CreateTable(tableName);
#nullable restore

        // Act & Assert.
        act
            .Should()
            .Throw<ArgumentException>();
    }
}
