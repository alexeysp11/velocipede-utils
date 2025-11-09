namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.ProcPipes;

/// <summary>
/// Unit of work as part of a request processing sequence.
/// </summary>
public abstract class AbstractProcPipe
{
    protected Action<IPipeDelegateParams> m_function;

    /// <summary>
    /// Default constructor.
    /// </summary>
    protected AbstractProcPipe(Action<IPipeDelegateParams> function)
    {
        m_function = function;
    }

    /// <summary>
    /// An abstract method for performing a unit of work as part of a request processing sequence.
    /// </summary>
    public abstract void Handle(IPipeDelegateParams parameters);
}