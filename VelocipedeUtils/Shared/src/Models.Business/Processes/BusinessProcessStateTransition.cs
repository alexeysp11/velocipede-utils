using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

namespace VelocipedeUtils.Shared.Models.Business.Processes;

/// <summary>
/// Represents a transition between states of a business process with workflow functionality.
/// </summary>
public class BusinessProcessStateTransition : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Gets or sets the associated business process.
    /// </summary>
    public BusinessProcess? BusinessProcess { get; set; }

    /// <summary>
    /// Gets or sets the state the transition is coming from.
    /// </summary>
    public BusinessProcessState? FromState { get; set; }

    /// <summary>
    /// Gets or sets the state the transition is going to.
    /// </summary>
    public BusinessProcessState? ToState { get; set; }

    /// <summary>
    /// Gets or sets the previous transition.
    /// </summary>
    public BusinessProcessStateTransition? Previous { get; set; }
    
    /// <summary>
    /// Gets or sets the associated endpoint call.
    /// </summary>
    public EndpointCall? EndpointCall { get; set; }
}