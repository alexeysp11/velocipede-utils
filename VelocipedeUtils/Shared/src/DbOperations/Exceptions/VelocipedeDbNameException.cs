using System;
using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
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
