using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions;

/// <summary>
/// The exception that is thrown when an invalid table name is specified.
/// </summary>
[Serializable]
public class VelocipedeTableNameException : Exception
{
    /// <summary>
    /// Constructor that sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.IncorrectTableName"/>.</remarks>
    public VelocipedeTableNameException()
        : base(ErrorMessageConstants.IncorrectTableName)
    {
    }

    /// <summary>
    /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.IncorrectTableName"/>.</remarks>
    public VelocipedeTableNameException(Exception innerException)
        : base(ErrorMessageConstants.IncorrectTableName, innerException)
    {
    }

    /// <inheritdoc/>
    public VelocipedeTableNameException(string message)
        : base(ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeTableNameException)))
    {
    }

    /// <inheritdoc/>
    public VelocipedeTableNameException(string message, Exception innerException)
        : base(ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeTableNameException)), innerException)
    {
    }
}
