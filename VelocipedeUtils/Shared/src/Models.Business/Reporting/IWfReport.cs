namespace VelocipedeUtils.Shared.Models.Business.Reporting;

/// <summary>
/// Report interface.
/// </summary>
public interface IWfReport
{
    WfPrintDoc? PrintDoc { get; set; }

    void Generate();
    void Export(WfExportFormatType format);
    void Print();
}