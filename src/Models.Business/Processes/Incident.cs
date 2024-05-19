using WorkflowLib.Models.Business;

namespace WorkflowLib.Models.Business.Processes
{
    /// <summary>
    /// Incident.
    /// </summary>
    public class Incident : BusinessEntityWF, IBusinessEntityWF
    {
        /// <summary>
        /// Incident type.
        /// </summary>
        public IncidentType IncidentType { get; set; }

        /// <summary>
        /// Location.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Date.
        /// </summary>
        public System.DateTime Date { get; set; }

        /// <summary>
        /// Incident severity.
        /// </summary>
        public IncidentSeverity Severity { get; set; }
    }
}