namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// A specific task that must be completed in the warehouse (e.g., receiving goods, placing goods, assembling an order).
/// May be associated with the movement of goods.
/// </summary>
public class WarehouseTask : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
{
    /// <summary>
    /// Number of the warehouse order.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Warehouse.
    /// </summary>
    public Warehouse? Warehouse { get; set; }

    /// <summary>
    /// Warehouse order type.
    /// </summary>
    public WarehouseOrder? WarehouseOrder { get; set; }
    
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