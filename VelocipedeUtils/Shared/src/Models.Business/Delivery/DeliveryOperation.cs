using VelocipedeUtils.Shared.Models.Business.Customers;
using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.Models.Business.Delivery;

/// <summary>
/// Delivery operation.
/// </summary>
public class DeliveryOperation : BusinessTask, IWfBusinessEntity
{
    /// <summary>
    /// Number of the order.
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// Generated QR code attached to the order.
    /// </summary>
    public string? GeneratedOrderQrCode { get; set; }

    /// <summary>
    /// Delivery method.
    /// </summary>
    public DeliveryMethod? DeliveryMethod { get; set; }

    /// <summary>
    /// Initial address of the delivery.
    /// </summary>
    public string? Origin { get; set; }
    
    /// <summary>
    /// Final address of the delivery (destination).
    /// </summary>
    public string? Destination { get; set; }

    /// <summary>
    /// Customer name of the order.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Customer name of the order.
    /// </summary>
    public Contact? Contact { get; set; }

    /// <summary>
    /// Determines whether the operation is internal.
    /// </summary>
    public bool? IsInternal { get; set; }

    /// <summary>
    /// Type of the delivery operation.
    /// </summary>
    public string? DeliveryOperationType { get; set; }
}