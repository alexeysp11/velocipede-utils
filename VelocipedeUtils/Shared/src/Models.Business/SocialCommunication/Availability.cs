namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication
{
    /// <summary>
    /// 
    /// </summary>
    public class Availability : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool ModifiedManually { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? StatusMessage { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public AvailabilityStatus Status { get; set; }
    }
}