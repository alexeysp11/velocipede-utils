namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// Warehouse order type.
/// </summary>
public enum WarehouseOrderType
{
    None = 0,
    Assembly = 1,
    Shipment = 2,
    Placement = 3,
    Receipt = 4,
    Return = 5,
    StockTaking = 6
}