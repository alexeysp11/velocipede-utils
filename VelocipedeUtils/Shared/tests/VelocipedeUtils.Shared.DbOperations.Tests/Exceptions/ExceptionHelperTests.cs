using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Exceptions;

/// <summary>
/// Unit tests for <see cref="ExceptionHelper"/>.
/// </summary>
public sealed class ExceptionHelperTests
{
    public static TheoryData<Type> GetExceptionTypes()
    {
        return new TheoryData<Type>
        {
            { typeof(Exception) },
            { typeof(ArgumentException) },
            { typeof(InvalidCastException) },
            { typeof(InvalidOperationException) },
            { typeof(OperationCanceledException) },
            { typeof(TaskCanceledException) },
            { typeof(ArgumentNullException) },
            { typeof(ArgumentOutOfRangeException) },
            { typeof(AggregateException) },
            { typeof(IndexOutOfRangeException) },
        };
    }

    public static TheoryData<string, Type> GetExceptionTypesWithMessage()
    {
        return new TheoryData<string, Type>
        {
            { "", typeof(Exception) },
            { "Test exception", typeof(Exception) },
            { "", typeof(ArgumentException) },
            { "Test exception", typeof(ArgumentException) },
            { "", typeof(InvalidCastException) },
            { "Test exception", typeof(InvalidCastException) },
            { "", typeof(InvalidOperationException) },
            { "Test exception", typeof(InvalidOperationException) },
            { "", typeof(OperationCanceledException) },
            { "Test exception", typeof(OperationCanceledException) },
            { "", typeof(TaskCanceledException) },
            { "Test exception", typeof(TaskCanceledException) },
            { "", typeof(ArgumentNullException) },
            { "Test exception", typeof(ArgumentNullException) },
            { "", typeof(ArgumentOutOfRangeException) },
            { "Test exception", typeof(ArgumentOutOfRangeException) },
            { "", typeof(AggregateException) },
            { "Test exception", typeof(AggregateException) },
            { "", typeof(IndexOutOfRangeException) },
            { "Test exception", typeof(IndexOutOfRangeException) },
        };
    }

    [Theory]
    [MemberData(nameof(GetExceptionTypes))]
    public void WrapMessageIfNull_NullMessage(Type exceptionType)
    {
        string result = ExceptionHelper.WrapMessageIfNull(null, exceptionType);
        result
            .Should()
            .Be($"Exception of type '{exceptionType}' was thrown");
    }

    [Theory]
    [MemberData(nameof(GetExceptionTypesWithMessage))]
    public void WrapMessageIfNull_NotNullMessage(string message, Type exceptionType)
    {
        string result = ExceptionHelper.WrapMessageIfNull(message, exceptionType);
        result
            .Should()
            .Be(message);
    }
}
