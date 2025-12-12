using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Exceptions;

/// <summary>
/// The exception that is thrown when an invalid database name is specified in the connection string.
/// </summary>
[Serializable]
public class VelocipedeQueryBuilderException : Exception
{
    /// <summary>
    /// Constructor that sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.ErrorInQueryBuilder"/>.</remarks>
    public VelocipedeQueryBuilderException()
        : base(ErrorMessageConstants.ErrorInQueryBuilder)
    {
    }

    /// <summary>
    /// Constructor that includes info about inner exception, and sets the <see cref="Exception.Message"/> field to default value.
    /// </summary>
    /// <remarks>Default value of the exception is specified as <see cref="ErrorMessageConstants.ErrorInQueryBuilder"/>.</remarks>
    public VelocipedeQueryBuilderException(Exception innerException)
        : base(ErrorMessageConstants.ErrorInQueryBuilder, innerException)
    {
    }

    /// <inheritdoc/>
    public VelocipedeQueryBuilderException(string message)
        : base(ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeQueryBuilderException)))
    {
    }

    /// <inheritdoc/>
    public VelocipedeQueryBuilderException(string message, Exception innerException)
        : base(ExceptionHelper.WrapMessageIfNull(message, typeof(VelocipedeQueryBuilderException)), innerException)
    {
    }

    /// <summary>
    /// Throws <see cref="VelocipedeQueryBuilderException"/> if <paramref name="databaseType"/> has an incorrect database type value:
    /// <list type="bullet">
    /// <item><description><see cref="VelocipedeDatabaseType.None"/></description></item>
    /// <item><description><see cref="VelocipedeDatabaseType.InMemory"/></description></item>
    /// </list>
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    /// <exception cref="VelocipedeQueryBuilderException"><paramref name="databaseType"/> has incorrect database type value.</exception>
    public static void ThrowIfIncorrectDatabaseType(VelocipedeDatabaseType databaseType)
    {
        if (databaseType is VelocipedeDatabaseType.None or VelocipedeDatabaseType.InMemory)
        {
            throw new VelocipedeQueryBuilderException(ErrorMessageConstants.UnableToCreateQueryBuilderForDbType);
        }
    }

    /// <summary>
    /// Throws <see cref="VelocipedeQueryBuilderException"/> if <paramref name="built"/> is true.
    /// </summary>
    /// <param name="built">Whether the query builder is built.</param>
    /// <exception cref="VelocipedeQueryBuilderException"><paramref name="built"/> is true.</exception>
    public static void ThrowIfBuilt(bool built)
    {
        if (built)
        {
            throw new VelocipedeQueryBuilderException(ErrorMessageConstants.QueryBuilderIsBuilt);
        }
    }
}
