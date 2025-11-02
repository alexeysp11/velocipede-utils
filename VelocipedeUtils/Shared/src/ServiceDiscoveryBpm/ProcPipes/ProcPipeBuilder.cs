namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.ProcPipes;

/// <summary>
/// Builds the sequence of request processing.
/// </summary>
public class ProcPipeBuilder
{
    private System.Action<IPipeDelegateParams> m_mainFunction;
    private List<System.Type> m_pipeTypes;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ProcPipeBuilder(System.Action<IPipeDelegateParams> mainFunction)
    {
        m_mainFunction = mainFunction;
        m_pipeTypes = new List<System.Type>();
    }

    /// <summary>
    /// Adds an element to the request processing sequence.
    /// </summary>
    public ProcPipeBuilder AddPipe(System.Type pipeType)
    {
        // if (!pipeType.IsInstanceOfType(typeof(AbstractProcPipe))) 
        //     throw new System.Exception("Incorrect pipe type");
        m_pipeTypes.Add(pipeType);
        return this;
    }

    /// <summary>
    /// Completes the process of constructing the request processing sequence.
    /// </summary>
    public System.Action<IPipeDelegateParams> Build()
    {
        return CreatePipe(0);
    }

    private System.Action<IPipeDelegateParams>? CreatePipe(int index)
    {
        if (index < m_pipeTypes.Count - 1)
        {
            var childPipeHandle = CreatePipe(index + 1);
            var pipe = (AbstractProcPipe?) System.Activator.CreateInstance(m_pipeTypes[index], childPipeHandle);
            return pipe?.Handle;
        }
        else
        {
            var finalPipe = (AbstractProcPipe?) System.Activator.CreateInstance(m_pipeTypes[index], m_mainFunction);
            return finalPipe?.Handle;
        }
    }
}