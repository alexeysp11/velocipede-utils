using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication;

/// <summary>
/// Like.
/// </summary>
public class Like : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Liker.
    /// </summary>
    public required UserAccount Liker { get; set; }
    
    /// <summary>
    /// Likee.
    /// </summary>
    public UserAccount? Likee { get; set; }

    /// <summary>
    /// Message.
    /// </summary>
    public MessageWF? MessageWF { get; set; }

    /// <summary>
    /// Post.
    /// </summary>
    public Post? Post { get; set; }
    
    /// <summary>
    /// Like type.
    /// </summary>
    public LikeType LikeType { get; set; }
}