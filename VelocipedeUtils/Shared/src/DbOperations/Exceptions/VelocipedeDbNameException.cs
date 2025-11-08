using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions;

/// <summary>
/// The exception that is thrown when an invalid database name is specified in the connection string.
/// </summary>
[Serializable]
public class VelocipedeDbNameException : VelocipedeDbConnectParamsException
{
    /// <summary>
    /// Constructor that sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.IncorrectDatabaseName"/>.</remarks>
    public VelocipedeDbNameException()
        : base(ErrorMessageConstants.IncorrectDatabaseName)
    {
    }

    /// <summary>
    /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.IncorrectDatabaseName"/>.</remarks>
    public VelocipedeDbNameException(Exception innerException)
        : base(ErrorMessageConstants.IncorrectDatabaseName, innerException)
    {
    }

    /// <summary>
    /// Constructor that sets the <see cref="Exception.Message"/> field to specified value.
    /// </summary>
    public VelocipedeDbNameException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to specified value.
    /// </summary>
    public VelocipedeDbNameException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
