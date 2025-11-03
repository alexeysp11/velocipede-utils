using System.Collections.Concurrent;
using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.ObjectPooling;

/// <summary>
/// Class for managing a pool of business process transitions.
/// </summary>
public sealed class TransitionPool
{
    private ConcurrentDictionary<long, BusinessProcessStateTransition>? m_prev2NextTransitions;
    private IReadOnlyDictionary<long, BusinessProcessStateTransition>? m_cachedPrev2NextTransitions;

    /// <summary>
    /// Cached copy of collection of business process transitions.
    /// </summary>
    public IReadOnlyDictionary<long, BusinessProcessStateTransition> Prev2NextTranstions
    {
        get
        {
            if (m_cachedPrev2NextTransitions == null)
            {
                m_cachedPrev2NextTransitions = m_prev2NextTransitions == null
                    ? new Dictionary<long, BusinessProcessStateTransition>()
                    : new Dictionary<long, BusinessProcessStateTransition>(m_prev2NextTransitions);
            }
            return m_cachedPrev2NextTransitions;
        }
    }

    /// <summary>
    /// Public constructor for the TransitionPool class.
    /// </summary>
    public TransitionPool()
    {
        m_prev2NextTransitions = new ConcurrentDictionary<long, BusinessProcessStateTransition>();
    }

    
    /// <summary>
    /// Add an transition to the pool.
    /// </summary>
    /// <param name="transition">Transition to add to the pool.</param>
    public void AddTransitionToPool(BusinessProcessStateTransition transition)
    {
        var previousId = (transition.Previous == null ? 0 : transition.Previous.Id);
        
        if (m_prev2NextTransitions == null)
            m_prev2NextTransitions = [];

        if (!m_prev2NextTransitions.ContainsKey(previousId))
        {
            m_prev2NextTransitions.TryAdd(previousId, transition);
            ClearCacheFields();
        }
    }

    /// <summary>
    /// Get next transition from the pool.
    /// </summary>
    public BusinessProcessStateTransition? GetNextTransitionById(long transitionId)
    {
        m_prev2NextTransitions ??= [];

        if (!m_prev2NextTransitions.ContainsKey(transitionId))
            return null;

        return m_prev2NextTransitions[transitionId];
    }

    /// <summary>
    /// Clears fields that contain cached values.
    /// </summary>
    private void ClearCacheFields()
    {
        m_cachedPrev2NextTransitions = null;
    }
}