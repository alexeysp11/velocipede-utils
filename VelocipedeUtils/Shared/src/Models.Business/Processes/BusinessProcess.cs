namespace VelocipedeUtils.Shared.Models.Business.Processes
{
    /// <summary>
    /// General description of the business process (for example, candidate selection).
    /// </summary>
    public class BusinessProcess : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// The diagram associated with the business process.
        /// </summary>
        public BusinessDiagram? Diagram { get; set; }

        /// <summary>
        /// Parent instance of the business process.
        /// </summary>
        public BusinessProcess? Parent { get; set; }

        /// <summary>
        /// Collection of the sub-processes.
        /// </summary>
        public required ICollection<BusinessProcess> SubProcesses { get; set; }

        /// <summary>
        /// Version number of the business process.
        /// </summary>
        public string? VersionNumber { get; set; }
    }
}