using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Shared.Models.Documents;

namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication;

/// <summary>
/// Respresents a message that is specific for the velocipede-utils.
/// </summary>
public class MessageWF : WfBusinessEntity, IWfBusinessEntity, ISendableBusinessEntity, IReceivableBusinessEntity
{
    /// <summary>
    /// Category of a message.
    /// </summary>
    public MessageCategory MessageCategory { get; set; }

    /// <summary>
    /// Subject of a message.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Body of a message.
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// Channel ID.
    /// </summary>
    public long ChannelId { get; set; }

    /// <summary>
    /// Instance of a channel.
    /// </summary>
    public required Channel Channel { get; set; }

    /// <summary>
    /// Chatroom ID.
    /// </summary>
    public long ChatroomId { get; set; }

    /// <summary>
    /// Instance of a chatroom.
    /// </summary>
    public required Chatroom Chatroom { get; set; }

    /// <summary>
    /// Date the business entity was sent.
    /// </summary>
    public DateTime? DateSent { get; set; }

    /// <summary>
    /// Sender ID.
    /// </summary>
    public long SenderId { get; set; }
    
    /// <summary>
    /// Instance of a user, that has sent the message.
    /// </summary>
    public virtual required UserAccount Sender { get; set; }

    /// <summary>
    /// Date the business entity was received.
    /// </summary>
    public DateTime? DateReceived { get; set; }
    
    /// <summary>
    /// Collection of recipient IDs.
    /// </summary>
    public required ICollection<long> RecipientIDs { get; set; }

    /// <summary>
    /// Collection of instances of a user, that has sent the message.
    /// </summary>
    public virtual required ICollection<UserAccount> UserRecipients { get; set; }

    /// <summary>
    /// Boolean variable to indicate if the message is received.
    /// </summary>
    public bool IsReceived { get; set; }
    
    /// <summary>
    /// Message status.
    /// </summary>
    public MessageStatus MessageStatus { get; set; }
    
    /// <summary>
    /// Deleted message status.
    /// </summary>
    public DeletedMessageStatus DeletedMessageStatus { get; set; }
    
    /// <summary>
    /// Boolean variable to indicate if the message is new.
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// Collection of attachments of the message.
    /// </summary>
    public required ICollection<Attachment> Attachments { get; set; }

    /// <summary>
    /// Boolean variable to indicate if the message is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}