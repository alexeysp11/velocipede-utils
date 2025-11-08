namespace VelocipedeUtils.Shared.Models.Business.Customers;

/// <summary>
/// Notification.
/// </summary>
public class Notification : WfBusinessEntity, IWfBusinessEntity, ISendableBusinessEntity, IReceivableBusinessEntity
{
    /// <summary>
    /// Date the business entity was sent.
    /// </summary>
    public DateTime? DateSent { get; set; }
    
    /// <summary>
    /// Sender ID.
    /// </summary>
    public long SenderId { get; set; }

    /// <summary>
    /// Date the business entity was received.
    /// </summary>
    public DateTime? DateReceived { get; set; }

    /// <summary>
    /// Receiver ID.
    /// </summary>
    public long ReceiverId { get; set; }

    /// <summary>
    /// Title text.
    /// </summary>
    public string? TitleText { get; set; }

    /// <summary>
    /// Body text.
    /// </summary>
    public string? BodyText { get; set; }

    /// <summary>
    /// Notes.
    /// </summary>
    public string? Notes { get; set; }
}
