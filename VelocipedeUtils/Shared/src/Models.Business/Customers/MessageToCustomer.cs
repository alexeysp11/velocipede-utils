using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.Customers;

/// <summary>
/// Message to the customer.
/// </summary>
public class MessageToCustomer : WfBusinessEntity, IWfBusinessEntity, ISendableBusinessEntity, IReceivableBusinessEntity
{
    /// <summary>
    /// Subject of the message.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Body of the message.
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// Category of the message.
    /// </summary>
    public MessageCategory MessageCategory { get; set; }

    /// <summary>
    /// Is the message new.
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// Is the message deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date the business entity was received.
    /// </summary>
    public DateTime? DateReceived { get; set; }
    
    /// <summary>
    /// Collection of recipient IDs.
    /// </summary>
    public required ICollection<string> RecipientIDs { get; set; }

    /// <summary>
    /// Collection of recipients.
    /// </summary>
    public virtual required ICollection<Customer> Recipients { get; set; }

    /// <summary>
    /// Is the message received.
    /// </summary>
    public bool IsReceived { get; set; }

    /// <summary>
    /// Date the business entity was sent.
    /// </summary>
    public DateTime? DateSent { get; set; }

    /// <summary>
    /// Sende ID.
    /// </summary>
    public long SenderId { get; set; }

    /// <summary>
    /// User account of the sender.
    /// </summary>
    public virtual required UserAccount Sender { get; set; }
}