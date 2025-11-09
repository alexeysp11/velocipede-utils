using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

namespace VelocipedeUtils.Shared.Models.Business.Processes;

/// <summary>
/// Provides a many-to-many relationship between objects of the BusinessProcessState and EndpointCall classes.
/// </summary>
public class BPStateEndpointCall : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Gets or sets the associated business process state transition.
    /// </summary>
    public required BusinessProcessState BusinessProcessState { get; set; }

    /// <summary>
    /// Gets or sets the associated endpoint call.
    /// </summary>
    public required EndpointCall EndpointCall { get; set; }
}