using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions;

/// <summary>
/// The exception that is thrown when the connection string is specified incorrectly.
/// </summary>
[Serializable]
public class VelocipedeConnectionStringException : VelocipedeDbConnectParamsException
{
    /// <summary>
    /// Constructor that sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.IncorrectConnectionString"/>.</remarks>
    public VelocipedeConnectionStringException()
        : base(ErrorMessageConstants.IncorrectConnectionString)
    {
    }

    /// <summary>
    /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.IncorrectConnectionString"/>.</remarks>
    public VelocipedeConnectionStringException(Exception innerException)
        : base(ErrorMessageConstants.IncorrectConnectionString, innerException)
    {
    }

    /// <summary>
    /// Constructor that sets the <see cref="Exception.Message"/> field to specified value.
    /// </summary>
    public VelocipedeConnectionStringException(string message)
        : base(ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeConnectionStringException)))
    {
    }

    /// <summary>
    /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to specified value.
    /// </summary>
    public VelocipedeConnectionStringException(string message, Exception innerException)
        : base(ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeConnectionStringException)), innerException)
    {
    }
}
