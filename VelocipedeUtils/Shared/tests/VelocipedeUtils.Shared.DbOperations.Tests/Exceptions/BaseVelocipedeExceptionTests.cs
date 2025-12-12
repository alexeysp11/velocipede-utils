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
    protected abstract TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        Exception innerException,
        Type innerExceptionType);

    /// <summary>
    /// Get test case for the exception with the specified message.
    /// </summary>
    /// <returns>Instance of <see cref="Exception"/>.</returns>
    protected abstract TestCaseCreateByConstructor GetCreateByConstructorTestCase(string message);

    /// <summary>
    /// Get test case for the exception with the specified message and inner exception.
    /// </summary>
    /// <returns>Instance of <see cref="Exception"/>.</returns>
    protected abstract TestCaseCreateByConstructor GetCreateByConstructorTestCase(
        string message,
        Exception innerException,
        Type innerExceptionType);

    public static TheoryData<Exception, Type> GetInnerExceptions()
    {
        return new TheoryData<Exception, Type>
        {
            { new Exception(), typeof(Exception) },
            { new ArgumentException(), typeof(ArgumentException) },
            { new InvalidCastException(), typeof(InvalidCastException) },
            { new InvalidOperationException(), typeof(InvalidOperationException) },
            { new OperationCanceledException(), typeof(OperationCanceledException) },
            { new TaskCanceledException(), typeof(TaskCanceledException) },
            { new ArgumentNullException(), typeof(ArgumentNullException) },
            { new ArgumentOutOfRangeException(), typeof(ArgumentOutOfRangeException) },
            { new AggregateException(), typeof(AggregateException) },
            { new IndexOutOfRangeException(), typeof(IndexOutOfRangeException) },
        };
    }

    public static TheoryData<string?, Exception, Type> GetInnerExceptionsWithMessage()
    {
        return new TheoryData<string?, Exception, Type>
        {
            { null, new Exception(), typeof(Exception) },
            { "", new Exception(), typeof(Exception) },
            { "Test exception", new Exception(), typeof(Exception) },
            { null, new ArgumentException(), typeof(ArgumentException) },
            { "", new ArgumentException(), typeof(ArgumentException) },
            { "Test exception", new ArgumentException(), typeof(ArgumentException) },
            { null, new InvalidCastException(), typeof(InvalidCastException) },
            { "", new InvalidCastException(), typeof(InvalidCastException) },
            { "Test exception", new InvalidCastException(), typeof(InvalidCastException) },
            { null, new InvalidOperationException(), typeof(InvalidOperationException) },
            { "", new InvalidOperationException(), typeof(InvalidOperationException) },
            { "Test exception", new InvalidOperationException(), typeof(InvalidOperationException) },
            { null, new OperationCanceledException(), typeof(OperationCanceledException) },
            { "", new OperationCanceledException(), typeof(OperationCanceledException) },
            { "Test exception", new OperationCanceledException(), typeof(OperationCanceledException) },
            { null, new TaskCanceledException(), typeof(TaskCanceledException) },
            { "", new TaskCanceledException(), typeof(TaskCanceledException) },
            { "Test exception", new TaskCanceledException(), typeof(TaskCanceledException) },
            { null, new ArgumentNullException(), typeof(ArgumentNullException) },
            { "", new ArgumentNullException(), typeof(ArgumentNullException) },
            { "Test exception", new ArgumentNullException(), typeof(ArgumentNullException) },
            { null, new ArgumentOutOfRangeException(), typeof(ArgumentOutOfRangeException) },
            { "", new ArgumentOutOfRangeException(), typeof(ArgumentOutOfRangeException) },
            { "Test exception", new ArgumentOutOfRangeException(), typeof(ArgumentOutOfRangeException) },
            { null, new AggregateException(), typeof(AggregateException) },
            { "", new AggregateException(), typeof(AggregateException) },
            { "Test exception", new AggregateException(), typeof(AggregateException) },
            { null, new IndexOutOfRangeException(), typeof(IndexOutOfRangeException) },
            { "", new IndexOutOfRangeException(), typeof(IndexOutOfRangeException) },
            { "Test exception", new IndexOutOfRangeException(), typeof(IndexOutOfRangeException) },
        };
    }

    [Fact]
    public void CreateByConstructor_Parameterless()
    {
        TestCaseCreateByConstructor testCase = GetCreateByConstructorTestCase();

        ValidateCreateByConstructorTest(testCase);
    }

    [Theory]
    [MemberData(nameof(GetInnerExceptions))]
    public void CreateByConstructor_InnerException(Exception innerException, Type innerExceptionType)
    {
        TestCaseCreateByConstructor testCase = GetCreateByConstructorTestCase(
            innerException,
            innerExceptionType);

        ValidateCreateByConstructorTest(testCase);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Test error message")]
    [InlineData("----")]
    public void CreateByConstructor_Message(string? errorMessage)
    {
#nullable disable
        TestCaseCreateByConstructor testCase = GetCreateByConstructorTestCase(errorMessage);
#nullable restore

        ValidateCreateByConstructorTest(testCase);
    }

    [Theory]
    [MemberData(nameof(GetInnerExceptionsWithMessage))]
    public void CreateByConstructor_MessageAndInnerException(
        string? errorMessage,
        Exception innerException,
        Type innerExceptionType)
    {
#nullable disable
        TestCaseCreateByConstructor testCase = GetCreateByConstructorTestCase(
            errorMessage,
            innerException,
            innerExceptionType);
#nullable restore

        ValidateCreateByConstructorTest(testCase);
    }

    private static void ValidateCreateByConstructorTest(TestCaseCreateByConstructor testCase)
    {
        Exception exception = testCase.Exception;

        // Exception.
        exception
            .Should()
            .NotBeNull()
            .And
            .BeOfType(testCase.ExceptionType);

        // Exception message.
        exception.Message
            .Should()
            .Be(testCase.ErrorMessage);
        
        // Inner exception.
        exception.InnerException
            .Should()
            .Be(testCase.InnerException);
        if (testCase.InnerExceptionType is not null)
        {
            exception.InnerException
                .Should()
                .BeOfType(testCase.InnerExceptionType);
        }
    }
}
