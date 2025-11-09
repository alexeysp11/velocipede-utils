namespace VelocipedeUtils.Shared.Models.Business.Monetary;

/// <summary>
/// Payment.
/// </summary>
public class Payment : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Payer.
    /// </summary>
    public string? Payer { get; set; }
    
    /// <summary>
    /// Receiver.
    /// </summary>
    public string? Receiver { get; set; }

    /// <summary>
    /// Card number.
    /// </summary>
    public string? CardNumber { get; set; }
    
    /// <summary>
    /// Amount of the payment.
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// Date of the payment.
    /// </summary>
    public DateTime? Date { get; private set; }

    /// <summary>
    /// Payment type.
    /// </summary>
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// Payment method.
    /// </summary>
    public string? PaymentMethod { get; set; }
    
    /// <summary>
    /// Status of the payment.
    /// </summary>
    public string? Status { get; set; }
}