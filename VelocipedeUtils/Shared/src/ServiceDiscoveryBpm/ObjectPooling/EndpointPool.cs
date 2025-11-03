using System.Collections.Concurrent;
using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.ObjectPooling;

/// <summary>
/// Class for managing a pool of endpoints.
/// </summary>
public sealed class EndpointPool
{
    private readonly ConcurrentDictionary<long, EndpointCollectionParameter> m_endpointParameters;
    private IReadOnlyDictionary<long, EndpointCollectionParameter>? m_cachedEndpointParameters;
    private IReadOnlyList<EndpointCollectionParameter>? m_cachedActiveEndpointParameters;

    /// <summary>
    /// Collection of endpoint parameters.
    /// </summary>
    public IReadOnlyDictionary<long, EndpointCollectionParameter> EndpointParameters
    {
        get
        {
            if (m_cachedEndpointParameters == null)
            {
                m_cachedEndpointParameters = new Dictionary<long, EndpointCollectionParameter>(m_endpointParameters);
            }
            return m_cachedEndpointParameters;
        }
    }

    /// <summary>
    /// Collection of endpoint parameters.
    /// </summary>
    public IReadOnlyList<EndpointCollectionParameter> ActiveEndpointParameters
    {
        get
        {
            if (m_cachedActiveEndpointParameters == null)
            {
                m_cachedActiveEndpointParameters = EndpointParameters.Values
                    .Where(p => p != null && p.Endpoint != null && p.Endpoint.Status == EndpointStatus.Active)
                    .ToList();
            }
            return m_cachedActiveEndpointParameters;
        }
    }

    /// <summary>
    /// Public constructor for the EndpointPool class.
    /// </summary>
    public EndpointPool()
    {
        m_endpointParameters = new ConcurrentDictionary<long, EndpointCollectionParameter>();
    }

    /// <summary>
    /// Add an endpoint to the pool.
    /// </summary>
    /// <param name="endpoint">Endpoint to add to the pool.</param>
    public void AddEndpointToPool(EndpointCollectionParameter endpoint)
    {
        if (!m_endpointParameters.ContainsKey(endpoint.Id))
        {
            m_endpointParameters.TryAdd(endpoint.Id, endpoint);
            ClearCacheFields();
        }
    }

    /// <summary>
    /// Get an endpoint from the pool by ID.
    /// </summary>
    /// <param name="endpointId">Endpoint ID.</param>
    /// <returns>Endpoint from the pool or null if not found.</returns>
    public EndpointCollectionParameter? GetEndpointFromPool(long endpointId)
    {
        if (m_endpointParameters.ContainsKey(endpointId))
        {
            m_endpointParameters.TryGetValue(endpointId, out EndpointCollectionParameter? endpointParameter);
            return endpointParameter;
        }
        return null;
    }

    /// <summary>
    /// Remove an endpoint from the pool by ID.
    /// </summary>
    /// <param name="endpointId">Identifier of the endpoint to delete.</param>
    public void RemoveEndpointFromPool(long endpointId)
    {
        if (m_endpointParameters.ContainsKey(endpointId))
        {
            m_endpointParameters.TryRemove(endpointId, out _);
            ClearCacheFields();
        }
    }

    /// <summary>
    /// Clears fields that contain cached values.
    /// </summary>
    private void ClearCacheFields()
    {
        m_cachedEndpointParameters = null;
        m_cachedActiveEndpointParameters = null;
    }
}