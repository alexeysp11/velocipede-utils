using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ProcPipes;
using VelocipedeUtils.Shared.ServiceDiscoveryBpm.ServiceRegistry;
using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.Routing;

/// <summary>
/// Provides functionality for the service mesh control plane (receives information where to redirect the process 
/// and causes a redirect using EsbProxies).
/// </summary>
public class EsbControlPlane
{
    private readonly EsbRoutingConfigs m_esbRoutingConfigs;
    private readonly IEsbServiceRegistry m_esbServiceRegistry;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public EsbControlPlane(
        EsbRoutingConfigs esbRoutingConfigs,
        IEsbServiceRegistry esbServiceRegistry)
    {
        m_esbRoutingConfigs = esbRoutingConfigs;
        m_esbServiceRegistry = esbServiceRegistry;
    }

    /// <summary>
    /// Returns the next state transition ID.
    /// </summary>
    public long GetNextStateTransitionId(
        long transitionId)
    {
        return m_esbServiceRegistry.GetNextStateTransitionId(transitionId);
    }

    /// <summary>
    /// Returns the state transition by business task ID.
    /// </summary>
    public BusinessProcessStateTransition GetTransitionByTaskId(long businessTaskId)
    {
        return m_esbServiceRegistry.GetTransitionByTaskId(businessTaskId);
    }

    /// <summary>
    /// Get a list of all business processes.
    /// </summary>
    public List<BusinessProcess> GetBusinessProcesses(
        long userId)
    {
        return m_esbServiceRegistry.GetBusinessProcesses(userId);
    }

    /// <summary>
    /// Get a list of workflow instances of the specified business process.
    /// </summary>
    public List<WorkflowInstance> GetWorkflowInstances(
        long businessProcessId)
    {
        return m_esbServiceRegistry.GetWorkflowInstances(businessProcessId);
    }
    
    /// <summary>
    /// Get the workflow instance by its ID.
    /// </summary>
    public WorkflowInstance GetWorkflowInstanceById(
        long workflowInstanceId)
    {
        return m_esbServiceRegistry.GetWorkflowInstanceById(workflowInstanceId);
    }
    
    /// <summary>
    /// Get the current task for a specific workflow instance.
    /// </summary>
    public BusinessTask GetCurrentTask(
        long workflowInstanceId)
    {
        return m_esbServiceRegistry.GetCurrentTask(workflowInstanceId);
    }

    /// <summary>
    /// Method for preserving the state of the service.
    /// </summary>
    public void PreserveServiceState(
        IPipeDelegateParams parameters)
    {
        m_esbServiceRegistry.PreserveServiceState(parameters);
    }

    /// <summary>
    /// Method for processing the previous service depending on the current state of the process.
    /// </summary>
    public void MoveWorkflowInstanceNext(
        IPipeDelegateParams parameters)
    {
        var transition2Delegate = m_esbRoutingConfigs.Transition2Delegate;
        if (transition2Delegate == null)
            throw new System.InvalidOperationException("The dictionary that binds transition ID to delegate could not be null");
        
        var workflowInstanceId = parameters.WorkflowInstanceId;
        var transitionId = parameters.BPStateTransitionId;

        // Get endpoint call ID.
        if (!transition2Delegate.ContainsKey(transitionId))
            throw new Exception($"Specified transition ID does not exist in the dictionary (transition ID = {transitionId})");
        var function = transition2Delegate[transitionId];
        if (function == null)
            throw new Exception("ESB delegate could not be null");
        
        function(parameters);
    }
}