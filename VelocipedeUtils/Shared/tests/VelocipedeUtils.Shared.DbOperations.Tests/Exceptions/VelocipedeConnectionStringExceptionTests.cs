using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Unit tests for <see cref="VelocipedeConnectionStringException"/>.
/// </summary>
public sealed class VelocipedeConnectionStringExceptionTests : BaseVelocipedeExceptionTests
{
    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase()
    {
        return new()
        {
            Exception = new VelocipedeConnectionStringException(),
            ExceptionType = typeof(VelocipedeConnectionStringException),
            ErrorMessage = ErrorMessageConstants.IncorrectConnectionString,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        Exception innerException,
        Type innerExceptionType)
    {
        return new()
        {
            Exception = new VelocipedeConnectionStringException(innerException),
            ExceptionType = typeof(VelocipedeConnectionStringException),
            ErrorMessage = ErrorMessageConstants.IncorrectConnectionString,
            InnerException = innerException,
            InnerExceptionType = innerExceptionType,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message)
    {
        return new()
        {
            Exception = new VelocipedeConnectionStringException(message),
            ExceptionType = typeof(VelocipedeConnectionStringException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeConnectionStringException)),
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
            Exception = new VelocipedeConnectionStringException(message, innerException),
            ExceptionType = typeof(VelocipedeConnectionStringException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeConnectionStringException)),
            InnerException = innerException,
        };
    }
}
