using System;
using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
    [Serializable]
    public class VelocipedeDbConnectException : Exception
    {
        public string InnerExceptionMessage => InnerException?.Message ?? "";

        public VelocipedeDbConnectException()
            : base(ErrorMessageConstants.UnableToConnectToDatabase)
        {
        }

        public VelocipedeDbConnectException(Exception innerException)
            : base(ErrorMessageConstants.UnableToConnectToDatabase, innerException)
        {
        }

        public VelocipedeDbConnectException(string message)
            : base(message)
        {
        }

        public VelocipedeDbConnectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
