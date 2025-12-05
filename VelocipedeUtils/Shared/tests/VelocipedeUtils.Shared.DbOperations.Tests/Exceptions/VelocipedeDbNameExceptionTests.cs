using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Unit tests for <see cref="VelocipedeDbNameException"/>.
/// </summary>
public sealed class VelocipedeDbNameExceptionTests : BaseVelocipedeExceptionTests
{
    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase()
    {
        return new()
        {
            Exception = new VelocipedeDbNameException(),
            ExceptionType = typeof(VelocipedeDbNameException),
            ErrorMessage = ErrorMessageConstants.IncorrectDatabaseName,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        Exception innerException,
        Type innerExceptionType)
    {
        return new()
        {
            Exception = new VelocipedeDbNameException(innerException),
            ExceptionType = typeof(VelocipedeDbNameException),
            ErrorMessage = ErrorMessageConstants.IncorrectDatabaseName,
            InnerException = innerException,
            InnerExceptionType = innerExceptionType,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message)
    {
        return new()
        {
            Exception = new VelocipedeDbNameException(message),
            ExceptionType = typeof(VelocipedeDbNameException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeDbNameException)),
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
            Exception = new VelocipedeDbNameException(message, innerException),
            ExceptionType = typeof(VelocipedeDbNameException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeDbNameException)),
            InnerException = innerException,
        };
    }
}
