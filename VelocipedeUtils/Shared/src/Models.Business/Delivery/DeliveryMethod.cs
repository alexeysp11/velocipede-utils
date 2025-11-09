namespace VelocipedeUtils.Shared.Models.Business.Delivery;

/// <summary>
/// Delivery method
/// </summary>
public class DeliveryMethod : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Type of the delivery method.
    /// </summary>
    public DeliveryMethodType Type { get; set; }

    /// <summary>
    /// Price of the delivery method.
    /// </summary>
    public decimal? Price { get; set; }
}