namespace VelocipedeUtils.Shared.Models.Business.Processes;

/// <summary>
/// Business process diagram
/// </summary>
public class BusinessDiagram : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Collection of business diagram elements.
    /// </summary>
    public required ICollection<BusinessDiagramElement> Elements { get; set; }
}