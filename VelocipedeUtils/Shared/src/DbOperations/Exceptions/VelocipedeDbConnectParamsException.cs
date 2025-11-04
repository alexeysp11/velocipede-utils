using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
    /// <summary>
    /// The exception that is thrown when one of the connection string parameters is specified incorrectly.
    /// </summary>
    [Serializable]
    public class VelocipedeDbConnectParamsException : Exception
    {
        /// <summary>
        /// Gets the value of <c>InnerException.Message</c>.
        /// </summary>
        /// <remarks>If <c>InnerException</c>, or <c>InnerException.Message</c> is null, then <see cref="string.Empty"/> is expected.</remarks>
        public string InnerExceptionMessage => InnerException?.Message ?? "";

        /// <summary>
        /// Constructor that sets the <see cref="Exception.Message"/> field to default value.
        /// </summary>
        /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.UnableToConnectToDatabase"/>.</remarks>
        public VelocipedeDbConnectParamsException()
            : base(ErrorMessageConstants.UnableToConnectToDatabase)
        {
        }

        /// <summary>
        /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to default value.
        /// </summary>
        /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.UnableToConnectToDatabase"/>.</remarks>
        public VelocipedeDbConnectParamsException(Exception innerException)
            : base(ErrorMessageConstants.UnableToConnectToDatabase, innerException)
        {
        }

        /// <summary>
        /// Constructor that sets the <see cref="Exception.Message"/> field to specified value.
        /// </summary>
        public VelocipedeDbConnectParamsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to specified value.
        /// </summary>
        public VelocipedeDbConnectParamsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
