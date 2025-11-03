using System.Data;

namespace VelocipedeUtils.Shared.Models.Business.Reporting
{
    /// <summary>
    /// Container for report data.
    /// </summary>
    public class WfReportData
    {
        public Guid Guid { get; set; }
        public required Dictionary<string, object> Parameters { get; set; }
        public required DataTable DataTable { get; set; }
    }
}