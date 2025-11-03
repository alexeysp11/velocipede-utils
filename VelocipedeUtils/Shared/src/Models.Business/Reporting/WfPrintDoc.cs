using System.Text.Json;

namespace VelocipedeUtils.Shared.Models.Business.Reporting
{
    /// <summary>
    /// The main object of a printed document.
    /// </summary>
    public class WfPrintDoc : WfBusinessEntity, IWfBusinessEntity
    {
        public WfPrinter? Printer { get; set; }
        public WfPrintDocLayout? PrintDocLayout { get; set; }
        public WfExportFormatType? ExportFormat { get; set; }
        public WfReportStatus? ReportStatus { get; set; }
        public string? ParametersJson { get; set; }
        public DateTime? DateLastModified { get; set; }

        public Dictionary<string, object>? GetParameters()
        {
            return string.IsNullOrEmpty(ParametersJson) 
                ? null 
                : JsonSerializer.Deserialize<Dictionary<string, object>>(ParametersJson);
        }

        public void SetParameters(Dictionary<string, object> parameters)
        {
            ParametersJson = JsonSerializer.Serialize(parameters);
        }
    }
}
