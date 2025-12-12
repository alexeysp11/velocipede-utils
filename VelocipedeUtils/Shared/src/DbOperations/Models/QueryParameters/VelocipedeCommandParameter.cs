using System.Data;

namespace VelocipedeUtils.Shared.DbOperations.Models.QueryParameters;

/// <summary>
/// Parameter used within a SQL command.
/// </summary>
public sealed class VelocipedeCommandParameter
{
    /// <summary>
    /// Name of the parameter.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Value of the parameter.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// The type of the parameter.
    /// </summary>
    public DbType? DbType { get; set; }

    /// <summary>
    /// The <c>in</c> or <c>out</c> direction of the parameter.
    /// </summary>
    public ParameterDirection? Direction { get; set; }

    /// <summary>
    /// The size of the parameter.
    /// </summary>
    public int? Size { get; set; }

    /// <summary>
    /// The precision of the parameter.
    /// </summary>
    public byte? Precision { get; set; }

    /// <summary>
    /// The scale of the parameter.
    /// </summary>
    public byte? Scale { get; set; }
}
