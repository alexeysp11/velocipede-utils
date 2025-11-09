using VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

namespace VelocipedeUtils.Shared.Models.Business.Products;

/// <summary>
/// Product transfer.
/// </summary>
public class ProductTransfer : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Warehouse product.
    /// </summary>
    public WHProduct? WHProduct { get; set; }

    /// <summary>
    /// Order product.
    /// </summary>
    public OrderProduct? OrderProduct { get; set; }

    /// <summary>
    /// Order.
    /// </summary>
    public Order? Order { get; set; }

    /// <summary>
    /// Delivery order product.
    /// </summary>
    public DeliveryOrderProduct? DeliveryOrderProduct { get; set; }

    /// <summary>
    /// Delivery order.
    /// </summary>
    public DeliveryOrder? DeliveryOrder { get; set; }

    /// <summary>
    /// Parent element.
    /// </summary>
    public ProductTransfer? Parent { get; set; }
    
    /// <summary>
    /// Business operation ID.
    /// </summary>
    public long? BusinessOperationId { get; set; }

    /// <summary>
    /// Date.
    /// </summary>
    public DateTime? Date { get; set; }
    
    /// <summary>
    /// Old quantity.
    /// </summary>
    public double? OldQuantity { get; set; }
    
    /// <summary>
    /// New quantity.
    /// </summary>
    public double NewQuantity { get; set; }
    
    /// <summary>
    /// Quantity delta.
    /// </summary>
    public double QuantityDelta { get; set; }
    
    /// <summary>
    /// Old status.
    /// </summary>
    public string? OldStatus { get; set; }
    
    /// <summary>
    /// New status.
    /// </summary>
    public string? NewStatus { get; set; }
}