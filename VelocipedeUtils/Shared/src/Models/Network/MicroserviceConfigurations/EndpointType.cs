namespace VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

/// <summary>
/// Endpoint type.
/// </summary>
public class EndpointType
{
    /// <summary>
    /// ID of the endpoint type.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Name of the endpoint type.
    /// </summary>
    public string? Name { get; set; }
}