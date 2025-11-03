using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication
{
    /// <summary>
    /// Comment.
    /// </summary>
    public class Comment : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Text.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Creation author.
        /// </summary>
        public required UserAccount AuthorCreated { get; set; }
    }
}