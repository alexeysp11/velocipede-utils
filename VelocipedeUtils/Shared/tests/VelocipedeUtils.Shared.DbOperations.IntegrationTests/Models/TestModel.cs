using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;

/// <summary>
/// A simple test model.
/// </summary>
public sealed class TestModel
{
    /// <summary>
    /// The identifier of the test model instance.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The name of the test model instance.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The additional info about the test model instance.
    /// </summary>
    public string? AdditionalInfo { get; set; }
}
