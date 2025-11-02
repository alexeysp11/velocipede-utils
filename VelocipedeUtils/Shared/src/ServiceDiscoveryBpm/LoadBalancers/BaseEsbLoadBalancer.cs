using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ObjectPooling;
using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.LoadBalancers;

/// <summary>
/// A base abstract class that implements a load balancer.
/// </summary>
public abstract class BaseEsbLoadBalancer
{
    private protected EndpointPool m_endpointPool;

    /// <summary>
    /// Update a specific endpoint in the list of endpoints.
    /// </summary>
    public void UpdateEndpoints(EndpointCollectionParameter endpointParameter)
    {
        if (endpointParameter == null)
            throw new System.ArgumentNullException(nameof(endpointParameter));
        if (endpointParameter.Endpoint == null)
            throw new System.ArgumentNullException(nameof(endpointParameter.Endpoint));
        
        CheckNullReferences();

        m_endpointPool.AddEndpointToPool(endpointParameter);
    }

    /// <summary>
    /// Get an endpoint from the pool collection.
    /// </summary>
    public EndpointCollectionParameter GetEndpointFromPool(string endpoint)
    {
        if (string.IsNullOrEmpty(endpoint))
            throw new System.ArgumentNullException(nameof(endpoint));
        
        CheckNullReferences();

        var existingEndpoint = m_endpointPool.EndpointParameters.FirstOrDefault(p => p.Value.Endpoint.Name == endpoint).Value;
        if (existingEndpoint == null)
        {
            // Return a new endpoint.
            return new EndpointCollectionParameter
            {
                Endpoint = new Endpoint
                {
                    Name = endpoint
                }
            };
        }
        return existingEndpoint;
    }

    /// <summary>
    /// Remove the specified endpoint from the collection.
    /// </summary>
    public virtual void RemoveEndpoint(string endpoint)
    {
        CheckNullReferences();

        var endpointToRemove = m_endpointPool.EndpointParameters.FirstOrDefault(p => p.Value?.Endpoint?.Name == endpoint);
        if (endpointToRemove.Value != null)
        {
            m_endpointPool.RemoveEndpointFromPool(endpointToRemove.Key);
        }
    }

    /// <summary>
    /// Checks that there are no null references.
    /// </summary>
    private protected void CheckNullReferences()
    {
        if (m_endpointPool == null)
            throw new System.ArgumentNullException(nameof(m_endpointPool));
        
        var endpointParameters = m_endpointPool.EndpointParameters;
        if (endpointParameters == null || endpointParameters.Count == 0)
            throw new InvalidOperationException("No endpoints available");
    }
}