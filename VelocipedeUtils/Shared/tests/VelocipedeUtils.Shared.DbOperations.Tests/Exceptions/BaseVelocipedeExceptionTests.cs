using FluentAssertions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Base class for unit testing exceptions.
/// </summary>
public abstract class BaseVelocipedeExceptionTests
{
    /// <summary>
    /// Get test case for the parameterless exception.
    /// </summary>
    /// <returns>Instance of <see cref="Exception"/>.</returns>
    protected abstract TestCaseCreateByConstructor GetCreateByConstructorTestCase();

    /// <summary>
    /// Get test case for the exception with inner exception.
    /// </summary>
    /// <returns>Instance of <see cref="Exception"/>.</returns>
    protected abstract TestCaseCreateByConstructor GetCreateByConstructorTestCase(Exception innerException);

    /// <summary>
    /// Get test case for the exception with the specified message.
    /// </summary>
    /// <returns>Instance of <see cref="Exception"/>.</returns>
    protected abstract TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message);

    /// <summary>
    /// Get test case for the exception with the specified message and inner exception.
    /// </summary>
    /// <returns>Instance of <see cref="Exception"/>.</returns>
    protected abstract TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message, Exception innerException);

    public static TheoryData<Exception> GetInnerExceptions()
    {
        return new TheoryData<Exception>
        {
            { new Exception() },
            { new ArgumentException() },
            { new InvalidCastException() },
            { new InvalidOperationException() },
            { new OperationCanceledException() },
            { new TaskCanceledException() },
            { new ArgumentNullException() },
            { new ArgumentOutOfRangeException() },
            { new AggregateException() },
            { new IndexOutOfRangeException() },
        };
    }

    [Fact]
    public void CreateByConstructor_Parameterless()
    {
        // Arrange & Act.
        TestCaseCreateByConstructor testCase = GetCreateByConstructorTestCase();
        Exception exception = testCase.Exception;
        Type exceptionType = testCase.ExceptionType;
        string? expectedErrorMessage = testCase.ErrorMessage;

        exception
            .Should()
            .NotBeNull()
            .And
            .BeOfType(exceptionType);
        exception.Message
            .Should()
            .Be(expectedErrorMessage);
        exception.InnerException
            .Should()
            .BeNull();
    }

    [Theory]
    [MemberData(nameof(GetInnerExceptions))]
    public void CreateByConstructor_InnerException(Exception innerException)
    {
        // Arrange & Act.
        TestCaseCreateByConstructor testCase = GetCreateByConstructorTestCase(innerException);
        Exception exception = testCase.Exception;
        Type exceptionType = testCase.ExceptionType;
        string? expectedErrorMessage = testCase.ErrorMessage;

        // Assert.
        exception
            .Should()
            .NotBeNull()
            .And
            .BeOfType(exceptionType);
        exception.Message
            .Should()
            .Be(expectedErrorMessage);
        exception.InnerException
            .Should()
            .NotBeNull();
    }

    [Fact]
    public void CreateByConstructor_Message()
    {
        // Arrange & Act.
        string errorMessage = "Test error message";
        TestCaseCreateByConstructor testCase = GetCreateByConstructorTestCase(errorMessage);
        Exception exception = testCase.Exception;
        Type exceptionType = testCase.ExceptionType;
        string? expectedErrorMessage = testCase.ErrorMessage;

        // Assert.
        exception
            .Should()
            .NotBeNull()
            .And
            .BeOfType(exceptionType);
        exception.Message
            .Should()
            .Be(expectedErrorMessage);
        exception.InnerException
            .Should()
            .BeNull();
    }

    [Theory]
    [MemberData(nameof(GetInnerExceptions))]
    public void CreateByConstructor_MessageAndInnerException(Exception innerException)
    {
        // Arrange & Act.
        string errorMessage = "Test error message";
        TestCaseCreateByConstructor testCase = GetCreateByConstructorTestCase(errorMessage, innerException);
        Exception exception = testCase.Exception;
        Type exceptionType = testCase.ExceptionType;
        string? expectedErrorMessage = testCase.ErrorMessage;

        // Assert.
        exception
            .Should()
            .NotBeNull()
            .And
            .BeOfType(exceptionType);
        exception.Message
            .Should()
            .Be(expectedErrorMessage);
        exception.InnerException
            .Should()
            .NotBeNull();
    }
}
