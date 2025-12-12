using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Unit tests for <see cref="VelocipedeTableNameException"/>.
/// </summary>
public sealed class VelocipedeTableNameExceptionTests : BaseVelocipedeExceptionTests
{
    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase()
    {
        return new()
        {
            Exception = new VelocipedeTableNameException(),
            ExceptionType = typeof(VelocipedeTableNameException),
            ErrorMessage = ErrorMessageConstants.IncorrectTableName,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        Exception innerException,
        Type innerExceptionType)
    {
        return new()
        {
            Exception = new VelocipedeTableNameException(innerException),
            ExceptionType = typeof(VelocipedeTableNameException),
            ErrorMessage = ErrorMessageConstants.IncorrectTableName,
            InnerException = innerException,
            InnerExceptionType = innerExceptionType,
        };
    }

    /// <inheritdoc/>
    protected override TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message)
    {
        return new()
        {
            Exception = new VelocipedeTableNameException(message),
            ExceptionType = typeof(VelocipedeTableNameException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeTableNameException)),
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
            Exception = new VelocipedeTableNameException(message, innerException),
            ExceptionType = typeof(VelocipedeTableNameException),
            ErrorMessage = ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeTableNameException)),
            InnerException = innerException,
        };
    }
}
