using VelocipedeUtils.Shared.ServiceDiscoveryBpm.DAL;
using VelocipedeUtils.Shared.ServiceDiscoveryBpm.LoadBalancers;
using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ObjectPooling;
using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ProcPipes;
using VelocipedeUtils.Shared.Models.Business.Processes;
using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.ServiceRegistry;

/// <summary>
/// Receives information from the database where the request can be forwarded and uses load balancers to calculate the load.
/// </summary>
public class EsbServiceRegistry : IEsbServiceRegistry
{
    private IEndpointDAL m_endpointDAL;
    private IBusinessProcessDAL m_businessProcessDAL;
    private EndpointSelectionParameter m_endpointSelectionParameter;
    private IEsbLoadBalancer m_loadBalancer;
    private TransitionPool m_transitionPool;

    /// <summary>
    /// Constructor by default.
    /// </summary>
    public EsbServiceRegistry(
        IEndpointDAL endpointDAL,
        IBusinessProcessDAL businessProcessDAL,
        EndpointSelectionParameter endpointSelectionParameter,
        IEsbLoadBalancer loadBalancer,
        TransitionPool transitionPool)
    {
        m_endpointDAL = endpointDAL;
        m_businessProcessDAL = businessProcessDAL;
        m_endpointSelectionParameter = endpointSelectionParameter;
        m_loadBalancer = loadBalancer;
        m_transitionPool = transitionPool;
    }

    /// <summary>
    /// Method for selecting an endpoint as part of an implicit call to the next element of the microservice architecture.
    /// </summary>
    public Endpoint GetNextEndpoint(
        EndpointCallType endpointCallType,
        BusinessProcessState currentState,
        BusinessProcessStateTransition stateTransition)
    {
        if (currentState == null)
            throw new System.Exception("Current state could not be null");
        // if (currentState.EndpointCall == null)
        //     throw new System.Exception("Current state should reference to existing endpoint call");
        if (stateTransition == null)
            throw new System.Exception("State transition could not be null");
        if (stateTransition.EndpointCall == null)
            throw new System.Exception("State transition should reference to existing endpoint call");
        
        return m_endpointDAL.GetEndpoint(x => x.EndpointCallType == endpointCallType
            // && x.Id == currentState.EndpointCall.Id
            && x.Id == stateTransition.EndpointCall.Id);
    }

    /// <summary>
    /// Method for selecting an endpoint within an explicit call to an element of a microservice architecture (by endpoint types).
    /// </summary>
    public Endpoint GetEndpointExplicit(
        EndpointType endpointTypeFrom,
        EndpointType endpointTypeTo)
    {
        if (endpointTypeFrom == null) 
            throw new System.Exception("Source endpoint type could not be null");
        if (endpointTypeTo == null)
            throw new System.Exception("Destination endpoint type could not be null");

        return m_endpointDAL.GetEndpoint(x => x.EndpointTypeFrom.Id == endpointTypeFrom.Id
            && x.EndpointTypeTo.Id == endpointTypeTo.Id);
    }

    /// <summary>
    /// Returns a workflow instance by its ID.
    /// </summary>
    public WorkflowInstance GetWorkflowInstanceById(
        long workflowInstanceId)
    {
        if (workflowInstanceId <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(workflowInstanceId), $"Workflow instance ID should be positive, but '{workflowInstanceId}' was passed");
        
        var workflowInstance = m_businessProcessDAL.GetWorkflowInstanceById(workflowInstanceId);
        if (workflowInstance == null)
            throw new System.Exception($"Workflow instance with the specified ID does not exist (workflowInstanceId: {workflowInstanceId})");
        return workflowInstance;
    }

    /// <summary>
    /// Initializes a business process instance.
    /// </summary>
    public WorkflowInstance CreateInitialWI(
        string processName, 
        string taskName)
    {
        if (string.IsNullOrEmpty(processName))
            throw new System.ArgumentNullException(nameof(processName));
        
        // Get business process.
        var process = m_businessProcessDAL.GetBusinessProcessByName(processName);
        if (process == null)
            throw new System.Exception($"Business process could not be found by name: {processName}");

        // Create workflow instance.
        var workflowInstance = m_businessProcessDAL.CreateWorkflowInstance(process);
        if (workflowInstance == null)
            throw new System.Exception($"Workflow instance is not created (processName: {processName})");
        
        CreateBusinessTaskByWI(workflowInstance, taskName);
        
        return workflowInstance;
    }

    /// <summary>
    /// Create a task for a workflow instance.
    /// </summary>
    public BusinessTask CreateBusinessTaskByWI(
        WorkflowInstance workflowInstance,
        string taskName,
        long? transitionId = null,
        bool isNextTask = true)
    {
        if (workflowInstance == null)
            throw new System.ArgumentNullException(nameof(workflowInstance));
        if (string.IsNullOrEmpty(taskName))
            throw new System.ArgumentNullException(nameof(taskName));
        
        // Get business state by transition ID.
        BusinessProcessState processState = null;
        if (transitionId != null)
        {
            processState = m_businessProcessDAL.GetBPStateByTransition(transitionId.Value, isNextTask);
            if (processState == null)
                throw new System.Exception($"Process state could not be found with for the specified transition ID (transitionId: {transitionId.Value}, isNextTask: {isNextTask})");
        }
        
        // Create business task.
        string taskSubject = $"{workflowInstance.Id}. {taskName} ({workflowInstance.Name})";
        var businessTask = m_businessProcessDAL.CreateBusinessTask(taskName, taskSubject, processState);
        if (businessTask == null)
            throw new System.Exception($"Business task is not created (workflowInstance.Id: {workflowInstance.Id}, taskName: {taskName})");

        // Create workflow tracking item.
        var workflowTrackingItem = m_businessProcessDAL.CreateWorkflowTrackingItem(workflowInstance, businessTask);
        if (workflowTrackingItem == null)
            throw new System.Exception($"Workflow tracking item is not created (workflowInstance.Id: {workflowInstance.Id}, taskName: {taskName})");

        return businessTask;
    }

    /// <summary>
    /// Returns a next task instance for the given state transition ID.
    /// </summary>
    public BusinessTask GetNextBusinessTask(
        long workflowInstanceId,
        long transitionId)
    {
        var businessTask = m_businessProcessDAL.GetNextBusinessTask(workflowInstanceId, transitionId);
        if (businessTask == null)
            throw new System.Exception($"Business task could not be found by the state transition ID (workflowInstanceId: {workflowInstanceId}, transitionId: {transitionId})");
        return businessTask;
    }

    /// <summary>
    /// Returns the next state transition ID of the specified process.
    /// </summary>
    public long GetNextStateTransitionId(
        long transitionId)
    {
        var nextTransition = m_transitionPool.GetNextTransitionById(transitionId);
        if (nextTransition == null)
            throw new System.Exception($"Unable to find next transition for the specified transition ID: {transitionId}");
        return nextTransition.Id;
    }

    /// <summary>
    /// Returns the state transition by business task ID.
    /// </summary>
    public BusinessProcessStateTransition GetTransitionByTaskId(
        long businessTaskId)
    {
        return m_businessProcessDAL.GetTransitionByTaskId(businessTaskId);
    }
    
    /// <summary>
    /// Method for preserving the state of the service.
    /// </summary>
    public void PreserveServiceState(
        IPipeDelegateParams parameters)
    {
        // 
    }

    /// <summary>
    /// Get a list of all business processes.
    /// </summary>
    public List<BusinessProcess> GetBusinessProcesses(
        long userId)
    {
        return m_businessProcessDAL.GetBusinessProcesses(userId);
    }

    /// <summary>
    /// Get a list of workflow instances of the specified business process.
    /// </summary>
    public List<WorkflowInstance> GetWorkflowInstances(
        long businessProcessId)
    {
        return m_businessProcessDAL.GetWorkflowInstances(businessProcessId);
    }
    
    /// <summary>
    /// Get the current task for a specific workflow instance.
    /// </summary>
    public BusinessTask GetCurrentTask(
        long workflowInstanceId)
    {
        return m_businessProcessDAL.GetCurrentTask(workflowInstanceId);
    }
}