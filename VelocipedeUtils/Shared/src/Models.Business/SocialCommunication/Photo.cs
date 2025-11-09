using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication;

/// <summary>
/// Photo.
/// </summary>
public class Photo : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Date added.
    /// </summary>
    public DateTime DateAdded { get; set; }
    
    /// <summary>
    /// Boolean variable that shows whether this is a main photo of the user account.
    /// </summary>
    public bool IsMain { get; set; }
    
    /// <summary>
    /// Public ID of the photo.
    /// </summary>
    public string? PublicId { get; set; }
    
    /// <summary>
    /// User ID.
    /// </summary>
    public long UserId { get; set; }
    
    /// <summary>
    /// User account.
    /// </summary>
    public UserAccount? User { get; set; }

    /// <summary>
    /// Comments.
    /// </summary>
    public required ICollection<Comment> Comments { get; set; }
}