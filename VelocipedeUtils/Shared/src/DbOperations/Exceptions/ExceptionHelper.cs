namespace VelocipedeUtils.Shared.DbOperations.Exceptions;

/// <summary>
/// Helper for <see cref="Exception"/>.
/// </summary>
public static class ExceptionHelper
{
    /// <summary>
    /// Wrap exception message if it's <c>null</c>.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="exceptionType">Exception type.</param>
    /// <returns>
    /// If <paramref name="message"/> is <c>null</c>, then <c>"Exception of type 'ExceptionType' was thrown"</c>;
    /// otherwise, the original message without change.
    /// </returns>
    public static string WrapMessageIfNull(string? message, Type exceptionType)
    {
        if (message is null)
        {
            return $"Exception of type '{exceptionType}' was thrown";
        }
        return (string)message;
    }
}
