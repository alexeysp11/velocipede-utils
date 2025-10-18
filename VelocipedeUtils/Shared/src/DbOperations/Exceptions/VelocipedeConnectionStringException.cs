using System;
using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the connection string is specified incorrectly.
    /// </summary>
    [Serializable]
    public class VelocipedeConnectionStringException : VelocipedeDbConnectParamsException
    {
        public VelocipedeConnectionStringException()
            : base(ErrorMessageConstants.IncorrectConnectionString)
        {
        }

        public VelocipedeConnectionStringException(Exception innerException)
            : base(ErrorMessageConstants.IncorrectConnectionString, innerException)
        {
        }

        public VelocipedeConnectionStringException(string message)
            : base(message)
        {
        }

        public VelocipedeConnectionStringException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
