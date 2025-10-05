using System;
using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
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
