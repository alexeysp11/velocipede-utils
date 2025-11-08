namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// A record of the amount of a product in a specific location.
/// Displays the current stock balance of the product in the warehouse.
/// Used for planning, control, and inventory management.
/// </summary>
public class StockTaking : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
{
    /// <summary>
    /// Warehouse task.
    /// </summary>
    public WarehouseTask? WarehouseTask { get; set; }
    
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