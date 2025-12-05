namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Test case for creating exception by constructor.
/// </summary>
public class TestCaseCreateByConstructor
{
    /// <summary>
    /// Created exception.
    /// </summary>
    public required Exception Exception { get; init; }

    /// <summary>
    /// Exception type associated with the created exception, <see cref="Exception"/>.
    /// </summary>
    public required Type ExceptionType { get; init; }

    /// <summary>
    /// Error message associated with the created exception, <see cref="Exception"/>.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Inner exception.
    /// </summary>
    public Exception? InnerException { get; init; }

    /// <summary>
    /// Inner exception type.
    /// </summary>
    public Type? InnerExceptionType { get; init; }
}
