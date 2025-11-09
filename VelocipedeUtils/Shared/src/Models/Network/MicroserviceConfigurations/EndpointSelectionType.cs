namespace VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

/// <summary>
/// Represents the types of endpoint selection algorithms that can be used in a load balancer.
/// </summary>
public enum EndpointSelectionType
{
    /// <summary>
    /// Randomly selects an endpoint.
    /// </summary>
    Random,

    /// <summary>
    /// Selects endpoints in a round-robin fashion.
    /// </summary>
    RoundRobin,

    /// <summary>
    /// Selects the least loaded endpoint.
    /// </summary>
    LeastLoaded,

    /// <summary>
    /// Selects the endpoint with the least number of active connections.
    /// </summary>
    LeastConnections,

    /// <summary>
    /// Uses weighted round-robin to select endpoints based on their weights.
    /// </summary>
    WeightedRoundRobin,

    /// <summary>
    /// Selects an endpoint based on the hash of the client's IP address.
    /// </summary>
    IPHashing,

    /// <summary>
    /// Selects the endpoint with the least response time.
    /// </summary>
    LeastResponseTime
}