namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Associate table between any executor and order.
/// </summary>
public class OrderExecutor : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
{
    /// <summary>
    /// Executor UID of the order.
    /// </summary>
    public string? ExecutorUid { get; set; }

    /// <summary>
    /// Executor name of the order.
    /// </summary>
    public string? ExecutorName { get; set; }

    /// <summary>
    /// Type of the executor in the order.
    /// </summary>
    public OrderExecutorType? OrderExecutorType { get; set; }

    /// <summary>
    /// Importance of the executor in the order.
    /// </summary>
    public OrderExecutorImportance? OrderExecutorImportance { get; set; }

    /// <summary>
    /// Order.
    /// </summary>
    public Order? Order { get; set; }
    
    /// <summary>
    /// Actual start date.
    /// </summary>
    public DateTime? DateStartActual { get; set; }
    
    /// <summary>
    /// Actual end date.
    /// </summary>
    public DateTime? DateEndActual { get; set; }
    
    /// <summary>
    /// Expected start date.
    /// </summary>
    public DateTime? DateStartExpected { get; set; }
    
    /// <summary>
    /// Expected end date.
    /// </summary>
    public DateTime? DateEndExpected { get; set; }
}