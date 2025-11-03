namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication
{
    /// <summary>
    /// Friendship change.
    /// </summary>
    public class FriendshipChange : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public required Friendship Friendship { get; set; }

        /// <summary>
        /// Friendship status.
        /// </summary>
        public FriendshipStatus Status { get; set; }
    }
}