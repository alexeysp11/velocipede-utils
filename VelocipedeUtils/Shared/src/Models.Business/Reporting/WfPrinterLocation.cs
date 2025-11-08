namespace VelocipedeUtils.Shared.Models.Business.Reporting;

/// <summary>
/// Location of the printer.
/// </summary>
public class WfPrinterLocation : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Number of the location.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Address of the location.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Determines whether the location is virtual.
    /// </summary>
    public bool? IsVirtual { get; set; }
}