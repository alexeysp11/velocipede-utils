using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Unit tests for <see cref="VelocipedeQueryBuilderException"/>.
/// </summary>
public sealed class VelocipedeQueryBuilderExceptionTests : BaseVelocipedeExceptionTests
{
    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase()
    {
        return new()
        {
            Exception = new VelocipedeQueryBuilderException(),
            ExceptionType = typeof(VelocipedeQueryBuilderException),
            ErrorMessage = ErrorMessageConstants.ErrorInQueryBuilder,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        Exception innerException,
        Type innerExceptionType)
    {
        return new()
        {
            Exception = new VelocipedeQueryBuilderException(innerException),
            ExceptionType = typeof(VelocipedeQueryBuilderException),
            ErrorMessage = ErrorMessageConstants.ErrorInQueryBuilder,
            InnerException = innerException,
            InnerExceptionType = innerExceptionType,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message)
    {
        return new()
        {
            Exception = new VelocipedeQueryBuilderException(message),
            ExceptionType = typeof(VelocipedeQueryBuilderException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeQueryBuilderException)),
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        string message,
        Exception innerException,
        Type innerExceptionType)
    {
        return new()
        {
            Exception = new VelocipedeQueryBuilderException(message, innerException),
            ExceptionType = typeof(VelocipedeQueryBuilderException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeQueryBuilderException)),
            InnerException = innerException,
        };
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    [InlineData(VelocipedeDatabaseType.MySQL)]
    [InlineData(VelocipedeDatabaseType.MariaDB)]
    public void ThrowIfIncorrectDatabaseType_NoException(VelocipedeDatabaseType databaseType)
    {
        Action act = () => VelocipedeQueryBuilderException.ThrowIfIncorrectDatabaseType(databaseType);
        act
            .Should()
            .NotThrow();
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.None)]
    [InlineData(VelocipedeDatabaseType.InMemory)]
    public void ThrowIfIncorrectDatabaseType_ThrowsVelocipedeQueryBuilderException(VelocipedeDatabaseType databaseType)
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
