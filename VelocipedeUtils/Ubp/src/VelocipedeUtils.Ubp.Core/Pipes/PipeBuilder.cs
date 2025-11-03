using System.Collections.Generic;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Ubp.Core.Models.Pipes;

namespace VelocipedeUtils.Ubp.Core.Pipes;

/// <summary>
/// Builds the sequence of the generators of initial datasets.
/// </summary>
public class PipeBuilder
{
    /// <summary>
    /// Application settings.
    /// </summary>
    private AppSettings _appSettings;

    /// <summary>
    /// The function that is going to be invoked in the end.
    /// </summary>
    private System.Action<PipeResult> _mainFunction;
    
    /// <summary>
    /// Types of pipe component components.
    /// </summary>
    private List<System.Type> _pipeTypes;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public PipeBuilder(AppSettings appSettings, System.Action<PipeResult> mainFunction)
    {
        _appSettings = appSettings;
        _mainFunction = mainFunction;
        _pipeTypes = new List<System.Type>();
    }

    /// <summary>
    /// Registers a pipe component.
    /// </summary>
    public PipeBuilder AddPipe(System.Type pipeType)
    {
        // if (!pipeType.IsInstanceOfType(typeof(AbstractPipe))) 
        //     throw new System.Exception("Incorrect pipe type");
        _pipeTypes.Add(pipeType);
        return this;
    }
    /// <summary>
    /// Registers a generating pipe component 
    /// </summary>
    public PipeBuilder AddGenerating(System.Type pipeType)
    {
        return AddPipe(pipeType);
    }

    /// <summary>
    /// Builds the pipe 
    /// </summary>
    public System.Action<PipeResult> Build()
    {
        return CreatePipe(0);
    }

    /// <summary>
    /// Creates instances of pipe components 
    /// </summary>
    private System.Action<PipeResult> CreatePipe(int index)
    {
        if (index < _pipeTypes.Count - 1)
        {
            var childPipeHandle = CreatePipe(index + 1);
            var pipe = (AbstractPipe) System.Activator.CreateInstance(_pipeTypes[index], _appSettings, childPipeHandle);
            return pipe.Handle;
        }
        else 
        {
            var finalPipe = (AbstractPipe) System.Activator.CreateInstance(_pipeTypes[index], _appSettings, _mainFunction);
            return finalPipe.Handle;
        }
    }
}