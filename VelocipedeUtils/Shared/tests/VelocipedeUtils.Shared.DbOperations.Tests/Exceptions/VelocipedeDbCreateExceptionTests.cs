using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Unit tests for <see cref="VelocipedeDbCreateException"/>.
/// </summary>
public sealed class VelocipedeDbCreateExceptionTests : BaseVelocipedeExceptionTests
{
    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase()
    {
        return new()
        {
            Exception = new VelocipedeDbCreateException(),
            ExceptionType = typeof(VelocipedeDbCreateException),
            ErrorMessage = ErrorMessageConstants.UnableToCreateDatabase,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        Exception innerException,
        Type innerExceptionType)
    {
        return new()
        {
            Exception = new VelocipedeDbCreateException(innerException),
            ExceptionType = typeof(VelocipedeDbCreateException),
            ErrorMessage = ErrorMessageConstants.UnableToCreateDatabase,
            InnerException = innerException,
            InnerExceptionType = innerExceptionType,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message)
    {
        return new()
        {
            Exception = new VelocipedeDbCreateException(message),
            ExceptionType = typeof(VelocipedeDbCreateException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeDbCreateException)),
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
            Exception = new VelocipedeDbCreateException(message, innerException),
            ExceptionType = typeof(VelocipedeDbCreateException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeDbCreateException)),
            InnerException = innerException,
        };
    }
}
