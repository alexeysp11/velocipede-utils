using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ProcPipes;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.Routing;

/// <summary>
/// Class for EBS routing configuration settings.
/// </summary>
public class EsbRoutingConfigs
{
    /// <summary>
    /// A dictionary where the key is the transition ID, and the value is the delegate to handle the call.
    /// </summary>
    public Dictionary<long, Action<IPipeDelegateParams>>? Transition2Delegate { get; set; }
}