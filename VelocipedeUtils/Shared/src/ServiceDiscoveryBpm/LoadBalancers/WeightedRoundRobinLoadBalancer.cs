using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;
using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ObjectPooling;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.LoadBalancers;

/// <summary>
/// Load balancer that uses weighted round-robin to select endpoints based on their weights.
/// </summary>
public class WeightedRoundRobinLoadBalancer : BaseEsbLoadBalancer, IEsbLoadBalancer
{
    private readonly object m_lock = new object();
    private readonly Random m_random;
    private int m_currentIndex;

    /// <summary>
    /// Initializes a new instance of the WeightedRoundRobinLoadBalancer class with the specified list of endpoints and weights.
    /// </summary>
    public WeightedRoundRobinLoadBalancer(
        EndpointPool endpointPool)
    {
        m_random = new Random();
        m_currentIndex = 0;
        m_endpointPool = endpointPool;
    }

    /// <summary>
    /// Get the next endpoint based on weighted round-robin selection.
    /// </summary>
    public string? GetNextEndpoint()
    {
        CheckNullReferences();

        IReadOnlyList<EndpointCollectionParameter>? endpointParameters = m_endpointPool?.ActiveEndpointParameters;
        if (endpointParameters == null || endpointParameters.Count == 0)
            throw new Exception("Collection of endpoint parameters is null or empty");
        
        var totalWeight = endpointParameters.Sum(p => p.EndpointWeight);
        var randomWeight = m_random.Next(totalWeight);

        var endpointParameter = endpointParameters.Last();
        lock (m_lock)
        {
            if (m_currentIndex >= endpointParameters.Count)
            {
                // Reset to 0 if currentIndex is out of bounds.
                m_currentIndex = 0;
            }

            int currentIndex = m_currentIndex;
            while (randomWeight >= 0)
            {
                randomWeight -= endpointParameters[currentIndex].EndpointWeight;
                if (randomWeight < 0)
                {
                    m_currentIndex = (currentIndex + 1) % endpointParameters.Count;
                    endpointParameter = endpointParameters[currentIndex];
                    break;
                }
                currentIndex = (currentIndex + 1) % endpointParameters.Count;
            }
        }

        return endpointParameter?.Endpoint?.Name;
    }
}
