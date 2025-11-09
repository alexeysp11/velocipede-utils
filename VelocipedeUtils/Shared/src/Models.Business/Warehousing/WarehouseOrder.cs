namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// Request for assembly and shipment of a certain number of goods.
/// </summary>
public class WarehouseOrder : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
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
    public WarehouseOrderType? WarehouseOrderType { get; set; }
    
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