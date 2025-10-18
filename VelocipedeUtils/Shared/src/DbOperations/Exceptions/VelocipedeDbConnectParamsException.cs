using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
    /// <summary>
    /// The exception that is thrown when one of the connection string parameters is specified incorrectly.
    /// </summary>
    [Serializable]
    public class VelocipedeDbConnectParamsException : Exception
    {
        public string InnerExceptionMessage => InnerException?.Message ?? "";

        public VelocipedeDbConnectParamsException()
            : base(ErrorMessageConstants.UnableToConnectToDatabase)
        {
        }

        public VelocipedeDbConnectParamsException(Exception innerException)
            : base(ErrorMessageConstants.UnableToConnectToDatabase, innerException)
        {
        }

        public VelocipedeDbConnectParamsException(string message)
            : base(message)
        {
        }

        public VelocipedeDbConnectParamsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
