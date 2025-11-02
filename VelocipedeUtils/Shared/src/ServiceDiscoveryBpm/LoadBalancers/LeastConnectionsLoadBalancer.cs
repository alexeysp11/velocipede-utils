using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ObjectPooling;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.LoadBalancers;

/// <summary>
/// Load balancer that selects the endpoint with the least number of active connections.
/// </summary>
public class LeastConnectionsLoadBalancer : BaseEsbLoadBalancer, IEsbLoadBalancer
{
    /// <summary>
    /// Initializes a new instance of the LeastConnectionsLoadBalancer class with the specified initial connection map.
    /// </summary>
    public LeastConnectionsLoadBalancer(
        EndpointPool endpointPool)
    {
        m_endpointPool = endpointPool;
    }

    /// <summary>
    /// Get the endpoint with the least number of active connections.
    /// </summary>
    public string GetNextEndpoint()
    {
        CheckNullReferences();

        var selectedEndpoint = m_endpointPool.ActiveEndpointParameters
            .OrderBy(p => p.ActiveConnectionCount)
            .FirstOrDefault();
        return selectedEndpoint.Endpoint.Name;
    }
}