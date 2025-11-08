using VelocipedeUtils.Shared.Models.Business.Delivery;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Delivery order.
/// </summary>
public class DeliveryOrder : Order, IWfBusinessEntity
{
    /// <summary>
    /// Parent delivery order.
    /// </summary>
    public DeliveryOrder? ParentDeliveryOrder { get; set; }
    
    /// <summary>
    /// Delivery method.
    /// </summary>
    public DeliveryMethod? DeliveryMethod { get; set; }
    
    /// <summary>
    /// Price for the delivery.
    /// </summary>
    public decimal? DeliveryPrice { get; set; }
    
    /// <summary>
    /// Initial address of the delivery.
    /// </summary>
    public string? Origin { get; set; }
    
    /// <summary>
    /// Final address of the delivery (destination).
    /// </summary>
    public string? Destination { get; set; }
}