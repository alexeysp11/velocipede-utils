using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.Models.Business.Projects
{
    /// <summary>
    /// Project plan item.
    /// </summary>
    public class ProjectPlanItem : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Priority.
        /// </summary>
        public TaskPriority? Priority { get; set; }

        /// <summary>
        /// Items.
        /// </summary>
        public required ICollection<ProjectPlanItem> Items { get; set; }
    }
}