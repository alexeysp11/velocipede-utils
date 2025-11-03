namespace VelocipedeUtils.Shared.Models.Logging
{
    /// <summary>
    /// Debug logs.
    /// </summary>
    public class DbgLog
    {
        /// <summary>
        /// ID of the log.
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Name of the source component.
        /// </summary>
        public string? SourceName { get; set; }
        
        /// <summary>
        /// Details of the source component.
        /// </summary>
        public string? SourceDetails { get; set; }
        
        /// <summary>
        /// Status of the source component.
        /// </summary>
        public string? SourceStatus { get; set; }
        
        /// <summary>
        /// Date of creation.
        /// </summary>
        public DateTime? DateCreated { get; set; }
        
        /// <summary>
        /// Date of change.
        /// </summary>
        public DateTime? DateChanged { get; set; }
    }
}