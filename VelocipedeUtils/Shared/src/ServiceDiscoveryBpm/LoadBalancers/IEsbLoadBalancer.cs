using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.LoadBalancers;

/// <summary>
/// Represents a load balancer that manages endpoints based on their current load.
/// </summary>
public interface IEsbLoadBalancer
{
    /// <summary>
    /// Gets the next available endpoint based on the load balancing strategy.
    /// </summary>
    string? GetNextEndpoint();

    /// <summary>
    /// Update a specific endpoint in the list of endpoints.
    /// </summary>
    void UpdateEndpoints(EndpointCollectionParameter endpointParameter);

    /// <summary>
    /// Get an endpoint from the pool collection.
    /// </summary>
    EndpointCollectionParameter GetEndpointFromPool(string endpoint);

    /// <summary>
    /// Removes a specific endpoint from the load balancer.
    /// </summary>
    void RemoveEndpoint(string endpoint);
}
