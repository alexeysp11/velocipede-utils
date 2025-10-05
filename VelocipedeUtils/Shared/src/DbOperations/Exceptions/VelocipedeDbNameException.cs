using System;
using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an invalid database name is specified in the connection string.
    /// </summary>
    [Serializable]
    public class VelocipedeDbNameException : VelocipedeDbConnectParamsException
    {
        public VelocipedeDbNameException()
            : base(ErrorMessageConstants.IncorrectDatabaseName)
        {
        }

        public VelocipedeDbNameException(Exception innerException)
            : base(ErrorMessageConstants.IncorrectDatabaseName, innerException)
        {
        }

        public VelocipedeDbNameException(string message)
            : base(message)
        {
        }

        public VelocipedeDbNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
