using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;
using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ObjectPooling;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.LoadBalancers;

/// <summary>
/// Represents a load balancer that randomly selects endpoints from a collection.
/// </summary>
public class RandomLoadBalancer : BaseEsbLoadBalancer, IEsbLoadBalancer
{
    private readonly Random m_random;

    /// <summary>
    /// Initializes a new instance of the RandomLoadBalancer class with the specified endpoints.
    /// </summary>
    public RandomLoadBalancer(
        EndpointPool endpointPool)
    {
        m_random = new Random();
        m_endpointPool = endpointPool;
    }

    /// <summary>
    /// Gets the next available endpoint randomly from the collection.
    /// </summary>
    public string? GetNextEndpoint()
    {
        CheckNullReferences();

        IReadOnlyList<EndpointCollectionParameter>? endpointParameters = m_endpointPool?.ActiveEndpointParameters;
        if (endpointParameters == null || endpointParameters.Count == 0)
            throw new Exception("Collection of endpoint parameters is null or empty");
        
        int index = m_random.Next(endpointParameters.Count);
        var value = endpointParameters[index];
        return value?.Endpoint?.Name;
    }
}