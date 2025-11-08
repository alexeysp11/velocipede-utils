namespace VelocipedeUtils.Shared.Models.Business.Reporting;

/// <summary>
/// Report template description.
/// </summary>
public class WfPrintDocLayout : WfBusinessEntity, IWfBusinessEntity
{
    public string? Layout { get; set; }
    public byte[]? TemplateContent { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? MarginTop { get; set; }
    public int? MarginRight { get; set; }
    public int? MarginLeft { get; set; }
    public int? MarginBottom { get; set; }
    public int? BorderTop { get; set; }
    public int? BorderRight { get; set; }
    public int? BorderLeft { get; set; }
    public int? BorderBottom { get; set; }
}