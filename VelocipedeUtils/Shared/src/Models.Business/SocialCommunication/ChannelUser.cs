namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication
{
    /// <summary>
    /// Channel/User.
    /// </summary>
    public class ChannelUser : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Channel ID.
        /// </summary>
        public long ChannelId { get; set; }
        
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