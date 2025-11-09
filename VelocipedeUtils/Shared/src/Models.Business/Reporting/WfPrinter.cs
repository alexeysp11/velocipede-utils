namespace VelocipedeUtils.Shared.Models.Business.Reporting;

/// <summary>
/// Printer object.
/// </summary>
public class WfPrinter : WfBusinessEntity, IWfBusinessEntity
{
    public WfPrinterLocation? PrinterLocation { get; set; }
}