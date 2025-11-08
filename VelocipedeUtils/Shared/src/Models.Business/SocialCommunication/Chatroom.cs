namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication;

/// <summary>
/// Chatroom.
/// </summary>
public class Chatroom : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Title of a chatroom.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Type of the chatroom.
    /// </summary>
    public ChatroomType Type { get; set; }
    
    /// <summary>
    /// Photo of the user.
    /// </summary>
    public byte[]? Photo { get; set; }

    /// <summary>
    /// Photo URL of the user.
    /// </summary>
    public string? PhotoUrl { get; set; }
}