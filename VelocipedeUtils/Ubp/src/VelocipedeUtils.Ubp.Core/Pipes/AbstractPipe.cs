using System.Collections.Generic;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Ubp.Core.Models.Pipes;

namespace VelocipedeUtils.Ubp.Core.Pipes;

/// <summary>
/// Represent an abstract pipe component that is used for composing sequences of the generators of initial datasets 
/// </summary>
public abstract class AbstractPipe
{
    protected AppSettings _appSettings;

    /// <summary>
    /// The function that is going to be invoked next 
    /// </summary>
    protected System.Action<PipeResult> _function;

    /// <summary>
    /// Constructor of the abstract dataset generator 
    /// </summary>
    public AbstractPipe(AppSettings appSettings, System.Action<PipeResult> function)
    {
        _appSettings = appSettings;
        _function = function;
    }

    /// <summary>
    /// Generates a DateTime object 
    /// </summary>
    protected System.DateTime GenerateDate(System.DateTime start, System.DateTime end)
    {
        var range = (end - start).Days;
        return start.AddDays(new Random().Next(range));
    }

    /// <summary>
    /// Method that implements a generating algorithm 
    /// </summary>
    public abstract void Handle(PipeResult result);
}