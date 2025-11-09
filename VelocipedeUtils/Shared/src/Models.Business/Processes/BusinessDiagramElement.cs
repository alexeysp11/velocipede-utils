using System.ComponentModel.DataAnnotations.Schema;

namespace VelocipedeUtils.Shared.Models.Business.Processes;

/// <summary>
/// Business process diagram element.
/// </summary>
public class BusinessDiagramElement : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    [NotMapped]
    public required ICollection<BDEConnector> InputConnectors { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotMapped]
    public required ICollection<BDEConnector> OutputConnectors { get; set; }
}