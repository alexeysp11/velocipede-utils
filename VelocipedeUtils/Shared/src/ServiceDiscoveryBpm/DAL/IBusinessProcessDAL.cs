using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.DAL;

/// <summary>
/// An interface at the DAL level that performs operations related to business processes in the database.
/// </summary>
public interface IBusinessProcessDAL
{
    /// <summary>
    /// Returns the business process by its name.
    /// </summary>
    BusinessProcess GetBusinessProcessByName(
        string processName);
    
    /// <summary>
    /// Returns a workflow instance by its ID.
    /// </summary>
    WorkflowInstance GetWorkflowInstanceById(
        long id);

    /// <summary>
    /// Creates a workflow instance.
    /// </summary>
    WorkflowInstance CreateWorkflowInstance(
        BusinessProcess process,
        WorkflowInstance? parentInstance = null,
        bool isEmulation = false);

    /// <summary>
    /// Creates a business task.
    /// </summary>
    BusinessTask CreateBusinessTask(
        string taskName,
        string taskSubject,
        BusinessProcessState? processState = null,
        BusinessTask? parentTask = null,
        TaskPriority priority = TaskPriority.Low,
        bool isEmulation = false);

    /// <summary>
    /// Creates a workflow tracking item.
    /// </summary>
    WorkflowTrackingItem CreateWorkflowTrackingItem(
        WorkflowInstance workflowInstance,
        BusinessTask activeTask);

    /// <summary>
    /// Returns the next state of the business process by transition ID.
    /// </summary>
    BusinessProcessState GetBPStateByTransition(
        long transitionId,
        bool isNextTask = true);

    /// <summary>
    /// Returns a task instance for the given workflow instance ID and state transition ID.
    /// </summary>
    BusinessTask GetNextBusinessTask(
        long workflowInstanceId,
        long transitionId);
    
    /// <summary>
    /// Get all business process state transitions.
    /// </summary>
    IList<BusinessProcessStateTransition> GetBusinessProcessStateTransitions();

    /// <summary>
    /// Get a list of all business processes.
    /// </summary>
    List<BusinessProcess> GetBusinessProcesses(
        long userId);

    /// <summary>
    /// Get a list of workflow instances of the specified business process.
    /// </summary>
    List<WorkflowInstance> GetWorkflowInstances(
        long businessProcessId);
    
    /// <summary>
    /// Get the current task for a specific workflow instance.
    /// </summary>
    BusinessTask GetCurrentTask(
        long workflowInstanceId);
    
    /// <summary>
    /// Returns the state transition by business task ID.
    /// </summary>
    BusinessProcessStateTransition GetTransitionByTaskId(
        long businessTaskId);
}