using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Unit tests for <see cref="VelocipedeQueryBuilderException"/>.
/// </summary>
public sealed class VelocipedeQueryBuilderExceptionTests
{
    [Fact]
    public void CreateByConstructor_Parameterless()
    {
        // Arrange & Act.
        VelocipedeQueryBuilderException exception = new();

        // Assert.
        exception
            .Should()
            .NotBeNull();
        exception.Message
            .Should()
            .Be(ErrorMessageConstants.ErrorInQueryBuilder);
        exception.InnerException
            .Should()
            .BeNull();
    }

    [Fact]
    public void CreateByConstructor_InnerException()
    {
        // Arrange.
        IndexOutOfRangeException innerException = new();

        // Act.
        VelocipedeQueryBuilderException exception = new(innerException);

        // Assert.
        exception
            .Should()
            .NotBeNull();
        exception.Message
            .Should()
            .Be(ErrorMessageConstants.ErrorInQueryBuilder);
        exception.InnerException
            .Should()
            .NotBeNull()
            .And
            .BeOfType<IndexOutOfRangeException>();
    }

    [Fact]
    public void CreateByConstructor_Message()
    {
        // Arrange.
        string errorMessage = "Test error message";

        // Act.
        VelocipedeQueryBuilderException exception = new(errorMessage);

        // Assert.
        exception
            .Should()
            .NotBeNull();
        exception.Message
            .Should()
            .Be(errorMessage);
        exception.InnerException
            .Should()
            .BeNull();
    }

    [Fact]
    public void CreateByConstructor_MessageAndInnerException()
    {
        // Arrange.
        string errorMessage = "Test error message";
        IndexOutOfRangeException innerException = new();

        // Act.
        VelocipedeQueryBuilderException exception = new(errorMessage, innerException);

        // Assert.
        exception
            .Should()
            .NotBeNull();
        exception.Message
            .Should()
            .Be(errorMessage);
        exception.InnerException
            .Should()
            .NotBeNull()
            .And
            .BeOfType<IndexOutOfRangeException>();
    }

    [Theory]
    [InlineData(DatabaseType.SQLite)]
    [InlineData(DatabaseType.PostgreSQL)]
    [InlineData(DatabaseType.MSSQL)]
    [InlineData(DatabaseType.MySQL)]
    [InlineData(DatabaseType.MariaDB)]
    public void ThrowIfIncorrectDatabaseType_NoException(DatabaseType databaseType)
    {
        Action act = () => VelocipedeQueryBuilderException.ThrowIfIncorrectDatabaseType(databaseType);
        act
            .Should()
            .NotThrow();
    }

    [Theory]
    [InlineData(DatabaseType.None)]
    [InlineData(DatabaseType.InMemory)]
    public void ThrowIfIncorrectDatabaseType_ThrowsVelocipedeQueryBuilderException(DatabaseType databaseType)
    {
        Action act = () => VelocipedeQueryBuilderException.ThrowIfIncorrectDatabaseType(databaseType);
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.UnableToCreateQueryBuilderForDbType);
    }

    [Fact]
    public void ThrowIfBuilt_NoException()
    {
        // Arrange.
        bool built = false;
        Action act = () => VelocipedeQueryBuilderException.ThrowIfBuilt(built);

        // Act & Assert.
        act
            .Should()
            .NotThrow();
    }

    [Fact]
    public void ThrowIfBuilt_ThrowsVelocipedeQueryBuilderException()
    {
        // Arrange.
        bool built = true;
        Action act = () => VelocipedeQueryBuilderException.ThrowIfBuilt(built);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeQueryBuilderException>()
            .WithMessage(ErrorMessageConstants.QueryBuilderIsBuilt);
    }
}
