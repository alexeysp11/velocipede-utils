namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Test case for creating exception by constructor.
/// </summary>
/// <param name="Exception">Created exception.</param>
/// <param name="ExceptionType">Exception type.</param>
/// <param name="ErrorMessage">Error message.</param>
public record TestCaseCreateByConstructor(Exception Exception, Type ExceptionType, string? ErrorMessage);
