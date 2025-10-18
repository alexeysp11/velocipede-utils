using System;
using VelocipedeUtils.Shared.DbOperations.Constants;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the database creation failed.
    /// </summary>
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
