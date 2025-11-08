namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// A record of the movement of a product between locations, zones, or warehouses.
/// Records all operations with goods (receipt, placement, assembly, shipment).
/// Provides traceability of goods in the warehouse.
/// </summary>
public class StockMovement : WfBusinessEntity, IWfBusinessEntity
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