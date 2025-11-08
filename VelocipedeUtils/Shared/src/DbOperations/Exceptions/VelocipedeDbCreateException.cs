using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions;

/// <summary>
/// The exception that is thrown when the database creation failed.
/// </summary>
[Serializable]
public class VelocipedeDbCreateException : Exception
{
    /// <summary>
    /// Gets the value of <c>InnerException.Message</c>.
    /// </summary>
    /// <remarks>If <c>InnerException</c>, or <c>InnerException.Message</c> is null, then <see cref="string.Empty"/> is expected.</remarks>
    public string InnerExceptionMessage => InnerException?.Message ?? "";

    /// <summary>
    /// Constructor that sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.UnableToCreateDatabase"/>.</remarks>
    public VelocipedeDbCreateException()
        : base(ErrorMessageConstants.UnableToCreateDatabase)
    {
    }

    /// <summary>
    /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.UnableToCreateDatabase"/>.</remarks>
    public VelocipedeDbCreateException(Exception innerException)
        : base(ErrorMessageConstants.UnableToCreateDatabase, innerException)
    {
    }

    /// <summary>
    /// Constructor that sets the <see cref="Exception.Message"/> field to specified value.
    /// </summary>
    public VelocipedeDbCreateException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to specified value.
    /// </summary>
    public VelocipedeDbCreateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
