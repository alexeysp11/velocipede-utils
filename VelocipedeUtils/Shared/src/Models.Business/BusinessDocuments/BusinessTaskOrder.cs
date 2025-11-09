using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Associate table between business task and order.
/// </summary>
public class BusinessTaskOrder : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Business task.
    /// </summary>
    public BusinessTask? BusinessTask { get; set; }

    /// <summary>
    /// Order.
    /// </summary>
    public Order? Order { get; set; }
}