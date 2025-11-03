using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;
using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ObjectPooling;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.LoadBalancers;

/// <summary>
/// Load balancer that selects the least loaded endpoint based on the current load.
/// </summary>
public class LeastLoadedLoadBalancer : BaseEsbLoadBalancer, IEsbLoadBalancer
{
    /// <summary>
    /// Initializes a new instance of the LeastLoadedLoadBalancer class with the specified initial load map.
    /// </summary>
    public LeastLoadedLoadBalancer(
        EndpointPool endpointPool)
    {
        m_endpointPool = endpointPool;
    }

    /// <summary>
    /// Get the least loaded endpoint based on the current load.
    /// </summary>
    public string? GetNextEndpoint()
    {
        CheckNullReferences();

        EndpointCollectionParameter? selectedEndpoint = m_endpointPool?.ActiveEndpointParameters
            .OrderBy(p => p.SystemLoad)
            .FirstOrDefault();
        return selectedEndpoint?.Endpoint?.Name;
    }
}