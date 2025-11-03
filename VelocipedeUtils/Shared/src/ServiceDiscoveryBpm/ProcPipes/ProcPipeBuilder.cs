namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.ProcPipes;

/// <summary>
/// Builds the sequence of request processing.
/// </summary>
public class ProcPipeBuilder
{
    private readonly Action<IPipeDelegateParams> m_mainFunction;
    private readonly List<Type> m_pipeTypes;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ProcPipeBuilder(Action<IPipeDelegateParams> mainFunction)
    {
        m_mainFunction = mainFunction;
        m_pipeTypes = [];
    }

    /// <summary>
    /// Adds an element to the request processing sequence.
    /// </summary>
    public ProcPipeBuilder AddPipe(Type pipeType)
    {
        m_pipeTypes.Add(pipeType);
        return this;
    }

    /// <summary>
    /// Completes the process of constructing the request processing sequence.
    /// </summary>
    public Action<IPipeDelegateParams>? Build()
    {
        return CreatePipe(0);
    }

    private Action<IPipeDelegateParams>? CreatePipe(int index)
    {
        AbstractProcPipe? pipe;
        if (index < m_pipeTypes.Count - 1)
        {
            Action<IPipeDelegateParams>? childPipeHandle = CreatePipe(index + 1);
            pipe = (AbstractProcPipe?)Activator.CreateInstance(m_pipeTypes[index], childPipeHandle);
        }
        else
        {
            pipe = (AbstractProcPipe?)Activator.CreateInstance(m_pipeTypes[index], m_mainFunction);
        }
        return pipe == null ? null : pipe.Handle;
    }
}