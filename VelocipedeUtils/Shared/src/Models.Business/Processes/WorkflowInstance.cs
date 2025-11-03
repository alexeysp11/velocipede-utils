using System.ComponentModel.DataAnnotations.Schema;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Shared.Models.Business.SocialCommunication;

namespace VelocipedeUtils.Shared.Models.Business.Processes
{
    /// <summary>
    /// A specific entity (workflow instance) that implements a business process.
    /// </summary>
    public class WorkflowInstance : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
    {
        /// <summary>
        /// Gets or sets the business process associated with this workflow instance.
        /// </summary>
        public required BusinessProcess BusinessProcess { get; set; }
        
        /// <summary>
        /// Workflow instance context.
        /// </summary>
        [NotMapped]
        public WorkflowInstanceContext? Context { get; set; }
        
        /// <summary>
        /// Actual start date.
        /// </summary>
        public DateTime? DateStartActual { get; set; }
        
        /// <summary>
        /// Actual end date.
        /// </summary>
        public DateTime? DateEndActual { get; set; }
        
        /// <summary>
        /// Expected start date.
        /// </summary>
        public DateTime? DateStartExpected { get; set; }
        
        /// <summary>
        /// Expected end date.
        /// </summary>
        public DateTime? DateEndExpected { get; set; }

        /// <summary>
        /// Initiator.
        /// </summary>
        public UserAccount? Initiator { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the workflow instance is in emulation mode.
        /// </summary>
        public bool IsEmulation { get; set; }

        /// <summary>
        /// Collection of workflow instance members.
        /// </summary>
        public required ICollection<WorkflowInstanceMember> Members { get; set; }

        /// <summary>
        /// Gets or sets the parent workflow instance if applicable.
        /// </summary>
        public WorkflowInstance? ParentInstance { get; set; }

        /// <summary>
        /// Comments.
        /// </summary>
        public required ICollection<Comment> Comments { get; set; }
    }
}