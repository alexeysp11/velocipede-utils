using VelocipedeUtils.Shared.Models.Business.SocialCommunication;

namespace VelocipedeUtils.Shared.Models.Business.Processes;

/// <summary>
/// Represents an item used for tracking the progress of a workflow instance.
/// </summary>
public class WorkflowTrackingItem : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Gets or sets the workflow instance associated with this tracking item.
    /// </summary>
    public required WorkflowInstance WorkflowInstance { get; set; }

    /// <summary>
    /// Gets or sets the active task within the workflow instance.
    /// </summary>
    public required BusinessTask ActiveTask { get; set; }

    /// <summary>
    /// Connector for the workflow tracking item.
    /// </summary>
    public BDEConnector? BDEConnector { get; set; }
    
    /// <summary>
    /// Comment for the workflow tracking item.
    /// </summary>
    public Comment? Comment { get; set; }

    /// <summary>
    /// Gets or sets the status of the tracking item.
    /// </summary>
    public WorkflowTrackingItemStatus Status { get; set; }
}