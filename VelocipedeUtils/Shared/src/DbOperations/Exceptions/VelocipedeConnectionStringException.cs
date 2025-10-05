using System;
using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
    [Serializable]
    public class VelocipedeConnectionStringException : VelocipedeDbConnectException
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
