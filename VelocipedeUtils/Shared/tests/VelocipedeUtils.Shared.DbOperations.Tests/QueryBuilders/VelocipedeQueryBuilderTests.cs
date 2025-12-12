using FluentAssertions;
using Moq;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.Tests.QueryBuilders;

/// <summary>
/// Unit tests for <see cref="VelocipedeQueryBuilder"/>.
/// </summary>
public sealed class VelocipedeQueryBuilderTests
{
    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    [InlineData(VelocipedeDatabaseType.MySQL)]
    [InlineData(VelocipedeDatabaseType.MariaDB)]
    public void CreateByDefaultConstructor(VelocipedeDatabaseType databaseType)
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
    [InlineData(VelocipedeDatabaseType.None)]
    [InlineData(VelocipedeDatabaseType.InMemory)]
    public void CreateByDefaultConstructor_IncorrectDatabaseType(VelocipedeDatabaseType databaseType)
    {
        Func<VelocipedeQueryBuilder> act = () => new VelocipedeQueryBuilder(databaseType);
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.UnableToCreateQueryBuilderForDbType);
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite, typeof(SqliteDbConnection))]
    [InlineData(VelocipedeDatabaseType.PostgreSQL, typeof(PgDbConnection))]
    [InlineData(VelocipedeDatabaseType.MSSQL, typeof(SqlServerDbConnection))]
    public void CreateByConstructor_SpecifyDbConnection(VelocipedeDatabaseType databaseType, Type expectedDbConnectionType)
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
    [InlineData(VelocipedeDatabaseType.None)]
    [InlineData(VelocipedeDatabaseType.InMemory)]
    public void CreateByConstructor_SpecifyDbConnectionAndIncorrectDatabaseType(VelocipedeDatabaseType databaseType)
    {
        using IVelocipedeDbConnection dbConnection = Mock.Of<IVelocipedeDbConnection>();
        Func<VelocipedeQueryBuilder> act = () => new VelocipedeQueryBuilder(databaseType, dbConnection);
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.UnableToCreateQueryBuilderForDbType);
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite, typeof(SqliteDbConnection))]
    [InlineData(VelocipedeDatabaseType.PostgreSQL, typeof(PgDbConnection))]
    [InlineData(VelocipedeDatabaseType.MSSQL, typeof(SqlServerDbConnection))]
    public void CreateByDbConnection_UseDbConnectionMethod(VelocipedeDatabaseType databaseType, Type expectedDbConnectionType)
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
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void CreateTable_ValidTableName(VelocipedeDatabaseType databaseType)
    {
        // Arrange & Act.
        string tableName = "TableName";
        VelocipedeQueryBuilder queryBuilder = new(databaseType);
        ICreateTableQueryBuilder createQueryBuilder = queryBuilder.CreateTable(tableName);

        // Assert.
        createQueryBuilder
            .Should()
            .NotBeNull()
            .And
            .BeOfType<CreateTableQueryBuilder>();
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
    public void CreateTable_NullTableName_ThrowsArgumentNullException(VelocipedeDatabaseType databaseType)
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
            .Throw<VelocipedeTableNameException>()
            .WithMessage(ErrorMessageConstants.IncorrectTableName);
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void CreateTable_EmptyTableName_ThrowsArgumentException(VelocipedeDatabaseType databaseType)
    {
        // Arrange.
        string tableName = "";
        VelocipedeQueryBuilder queryBuilder = new(databaseType);
        Func<ICreateTableQueryBuilder> act = () => queryBuilder.CreateTable(tableName);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeTableNameException>()
            .WithMessage(ErrorMessageConstants.IncorrectTableName);
    }
}
