using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Shared.Models.Business.SocialCommunication;
using VelocipedeUtils.Shared.Models.Business.RiskManagement;

namespace VelocipedeUtils.Shared.Models.Business.Processes
{
    /// <summary>
    /// Actual business task that was assigned to a specific employee.
    /// </summary>
    public class BusinessTask : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
    {
        /// <summary>
        /// Subject.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Parent task.
        /// </summary>
        public BusinessTask? ParentTask { get; set; }

        /// <summary>
        /// Child tasks.
        /// </summary>
        public required ICollection<BusinessTask> ChildTasks { get; set; }
        
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
        /// Executor.
        /// </summary>
        public UserAccount? Executor { get; set; }

        /// <summary>
        /// Executor by default in the emulation mode.
        /// </summary>
        public UserAccount? ExecutorIsEmulation { get; set; }

        /// <summary>
        /// Executor after replacement.
        /// </summary>
        public UserAccount? ExecutorReplaced { get; set; }

        /// <summary>
        /// Priority.
        /// </summary>
        public TaskPriority Priority { get; set; }
        
        /// <summary>
        /// Business process state.
        /// </summary>
        public BusinessProcessState? BusinessProcessState { get; set; }

        /// <summary>
        /// Collection of risks related to the business task.
        /// </summary>
        [Obsolete("It's better to use BusinessTaskRisk object")]
        public required ICollection<Risk> Risks { get; set; }

        /// <summary>
        /// Collection of comments associated with the task.
        /// </summary>
        [Obsolete("It's better to use BusinessTaskComment object")]
        public required ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Shows if it is necessary to send notifications if the task is expired and not completed.
        /// </summary>
        public bool ExpiredNotificationSent { get; set; }

        /// <summary>
        /// Shows if the task is launched in the emulation mode.
        /// </summary>
        public bool IsEmulation { get; set; }
        
        /// <summary>
        /// Status of the business task.
        /// </summary>
        public BusinessTaskStatus Status { get; set; }
    }
}