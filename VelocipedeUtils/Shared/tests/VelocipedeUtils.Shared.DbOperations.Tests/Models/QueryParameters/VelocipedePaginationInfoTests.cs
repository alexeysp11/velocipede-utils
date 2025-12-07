using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Models.QueryParameters;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Models.QueryParameters;

/// <summary>
/// Unit tests for <see cref="VelocipedePaginationInfo"/>
/// </summary>
public sealed class VelocipedePaginationInfoTests
{
    [Fact]
    public void CreateByDefaultConstructor()
    {
        // Arrange.
        int expectedLimit = int.MaxValue;
        int expectedIndex = 0;
        int expectedOffset = 0;

        // Act.
        VelocipedePaginationInfo paginationInfo = new();

        // Assert.
        paginationInfo.Limit.Should().Be(expectedLimit);
        paginationInfo.Index.Should().Be(expectedIndex);
        paginationInfo.Offset.Should().Be(expectedOffset);
    }

    [Theory]
    [InlineData(50, 0, 0)]
    [InlineData(50, 1, 50)]
    [InlineData(50, 2, 100)]
    [InlineData(50, 3, 150)]
    [InlineData(50, 4, 200)]
    [InlineData(100, 0, 0)]
    [InlineData(100, 1, 100)]
    [InlineData(100, 2, 200)]
    [InlineData(100, 3, 300)]
    [InlineData(100, 4, 400)]
    public void CreateByParameterizedConstructor(int expectedLimit, int expectedIndex, int expectedOffset)
    {
        // Arrange & Act.
        VelocipedePaginationInfo paginationInfo = new(expectedLimit, expectedIndex, expectedOffset);

        // Assert.
        paginationInfo.Limit.Should().Be(expectedLimit);
        paginationInfo.Index.Should().Be(expectedIndex);
        paginationInfo.Offset.Should().Be(expectedOffset);
    }

    [Theory]
    [InlineData(50, 0, 0)]
    [InlineData(50, 1, 50)]
    [InlineData(50, 2, 100)]
    [InlineData(50, 3, 150)]
    [InlineData(50, 4, 200)]
    [InlineData(100, 0, 0)]
    [InlineData(100, 1, 100)]
    [InlineData(100, 2, 200)]
    [InlineData(100, 3, 300)]
    [InlineData(100, 4, 400)]
    public void CreateByIndex(int expectedLimit, int expectedIndex, int expectedOffset)
    {
        // Arrange & Act.
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByIndex(expectedLimit, expectedIndex);

        // Assert.
        paginationInfo.Limit.Should().Be(expectedLimit);
        paginationInfo.Index.Should().Be(expectedIndex);
        paginationInfo.Offset.Should().Be(expectedOffset);
    }

    [Theory]
    [InlineData(50, 0, 0)]
    [InlineData(50, 1, 50)]
    [InlineData(50, 2, 100)]
    [InlineData(50, 3, 150)]
    [InlineData(50, 4, 200)]
    [InlineData(100, 0, 0)]
    [InlineData(100, 1, 100)]
    [InlineData(100, 2, 200)]
    [InlineData(100, 3, 300)]
    [InlineData(100, 4, 400)]
    public void CreateByOffset(int expectedLimit, int expectedIndex, int expectedOffset)
    {
        // Arrange & Act.
        VelocipedePaginationInfo paginationInfo = VelocipedePaginationInfo.CreateByOffset(expectedLimit, expectedOffset);

        // Assert.
        paginationInfo.Limit.Should().Be(expectedLimit);
        paginationInfo.Index.Should().Be(expectedIndex);
        paginationInfo.Offset.Should().Be(expectedOffset);
    }
}
