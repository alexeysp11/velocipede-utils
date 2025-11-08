namespace VelocipedeUtils.Shared.Models.Business.Projects;

/// <summary>
/// Project phase.
/// </summary>
public class ProjectPhase : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Project.
    /// </summary>
    public Project? Project { get; set; }

    /// <summary>
    /// Project plan item.
    /// </summary>
    public ProjectPlanItem? ProjectPlanItem { get; set; }
}