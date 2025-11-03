using VelocipedeUtils.Shared.Models.Business.Projects;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.Responsibilities
{
    /// <summary>
    /// Employee responsibility.
    /// </summary>
    public class EmployeeResponsibility : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Project.
        /// </summary>
        public Project? Project { get; set; }

        /// <summary>
        /// Skills required for the project/responsibility.
        /// </summary>
        public required ICollection<Skill> Skills { get; set; }
    }
}