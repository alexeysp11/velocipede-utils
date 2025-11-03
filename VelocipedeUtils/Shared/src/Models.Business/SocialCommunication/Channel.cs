namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication
{
    /// <summary>
    /// Channel in a chat app.
    /// </summary>
    public class Channel : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Title of a channel.
        /// </summary>
        public string? Title { get; set; }
        
        /// <summary>
        /// Photo of a channel.
        /// </summary>
        public byte[]? Photo { get; set; }

        /// <summary>
        /// Photo URL of a channel.
        /// </summary>
        public string? PhotoUrl { get; set; }
    }
}