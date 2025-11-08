namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Message to user.
/// </summary>
public class MessageToUser : WfBusinessEntity, IWfBusinessEntity, ISendableBusinessEntity, IReceivableBusinessEntity
{
    /// <summary>
    /// Subect.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Body.
    /// </summary>
    public string? Body { get; set; }
    
    /// <summary>
    /// Message category.
    /// </summary>
    public MessageCategory? MessageCategory { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date the business entity was received.
    /// </summary>
    public DateTime? DateReceived { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required ICollection<string> RecipientIDs { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual required ICollection<UserAccount> Recipients { get; set; }

    /// <summary>
    /// Date the business entity was sent.
    /// </summary>
    public DateTime? DateSent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long SenderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual required UserAccount Sender { get; set; }
}