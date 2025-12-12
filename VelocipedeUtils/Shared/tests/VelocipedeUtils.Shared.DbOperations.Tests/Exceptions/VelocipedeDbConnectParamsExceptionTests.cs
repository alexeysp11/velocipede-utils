using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Unit tests for <see cref="VelocipedeDbConnectParamsException"/>.
/// </summary>
public sealed class VelocipedeDbConnectParamsExceptionTests : BaseVelocipedeExceptionTests
{
    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase()
    {
        return new()
        {
            Exception = new VelocipedeDbConnectParamsException(),
            ExceptionType = typeof(VelocipedeDbConnectParamsException),
            ErrorMessage = ErrorMessageConstants.UnableToConnectToDatabase,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        Exception innerException,
        Type innerExceptionType)
    {
        return new()
        {
            Exception = new VelocipedeDbConnectParamsException(innerException),
            ExceptionType = typeof(VelocipedeDbConnectParamsException),
            ErrorMessage = ErrorMessageConstants.UnableToConnectToDatabase,
            InnerException = innerException,
            InnerExceptionType = innerExceptionType,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message)
    {
        return new()
        {
            Exception = new VelocipedeDbConnectParamsException(message),
            ExceptionType = typeof(VelocipedeDbConnectParamsException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeDbConnectParamsException)),
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
            Exception = new VelocipedeDbConnectParamsException(message, innerException),
            ExceptionType = typeof(VelocipedeDbConnectParamsException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeDbConnectParamsException)),
            InnerException = innerException,
        };
    }
}
