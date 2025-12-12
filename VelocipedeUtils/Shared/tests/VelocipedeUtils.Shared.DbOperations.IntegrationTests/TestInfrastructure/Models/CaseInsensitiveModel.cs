namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Models;

/// <summary>
/// Class used to define case insensitive table.
/// </summary>
internal sealed class CaseInsensitiveModel
{
    /// <summary>
    /// Identifier of the record.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Value of the record.
    /// </summary>
    public required string Value { get; init; }
}
