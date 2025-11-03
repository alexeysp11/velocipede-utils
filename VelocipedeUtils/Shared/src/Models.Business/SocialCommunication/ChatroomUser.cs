namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication
{
    /// <summary>
    /// Chatroom/User.
    /// </summary>
    public class ChatroomUser : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Chatroom ID.
        /// </summary>
        public long ChatroomId { get; set; }

        /// <summary>
        /// Title of a chatroom, that is displayed for the user specifically.
        /// </summary>
        public string? ChatroomTitle { get; set; }
        
        /// <summary>
        /// User ID.
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// Is the user admin of the channel.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}