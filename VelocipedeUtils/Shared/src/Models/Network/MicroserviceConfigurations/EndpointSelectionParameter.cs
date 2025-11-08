namespace VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

/// <summary>
/// Endpoint selection parameter.
/// </summary>
public class EndpointSelectionParameter
{
    /// <summary>
    /// This parameter displays whether it is necessary to contact the database in order to get the next endpoint.
    /// </summary>
    public bool RetrieveFromDb { get; set; }

    /// <summary>
    /// Represents the types of endpoint selection algorithms that can be used in a load balancer.
    /// </summary>
    public EndpointSelectionType EndpointSelectionType { get; set; }

    /// <summary>
    /// Lifetime of an endpoint in memory in inactive status.
    /// </summary>
    public TimeSpan? InactiveTimeSpan { get; set; }
}