using System;
using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
    [Serializable]
    public class VelocipedeDbCreateException : Exception
    {
        public string InnerExceptionMessage => InnerException?.Message ?? "";

        public VelocipedeDbCreateException()
            : base(ErrorMessageConstants.UnableToCreateDatabase)
        {
        }

        public VelocipedeDbCreateException(Exception innerException)
            : base(ErrorMessageConstants.UnableToCreateDatabase, innerException)
        {
        }

        public VelocipedeDbCreateException(string message)
            : base(message)
        {
        }

        public VelocipedeDbCreateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
