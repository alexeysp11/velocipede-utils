using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.QueryBuilders;

/// <summary>
/// Test case for testing native column types.
/// </summary>
public sealed class TestCaseNativeColumnType
{
    public required VelocipedeColumnInfo ColumnInfo { get; init; }
    public required string ExpectedNativeType { get; init; }
}
