using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Models.Metadata.VelocipedeColumnInfoTests;

/// <summary>
/// Test case for testing formatting default value.
/// </summary>
public sealed class TestCaseFormatDefaultValue
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required DbType ColumnType { get; init; }
    public required object? DefaultValue { get; init; }
    public required string Expected { get; init; }
}
