using System.Collections.Generic;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Core.Models.Pipes;

/// <summary>
/// Represents all the generated collections.
/// </summary>
public class PipeResult
{
    /// <summary>
    /// Parameters used for the purpose of generating initial datasets.
    /// </summary>
    public PipeParams PipeParams { get; }
    
    /// <summary>
    /// Generated dataset of employees.
    /// </summary>
    public List<Employee> Employees { get; set; }
    
    /// <summary>
    /// Generated dataset of vacations.
    /// </summary>
    public List<Absense> Vacations { get; set; }
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PipeResult(PipeParams pipeParams)
    {
        PipeParams = pipeParams;
        Employees = new List<Employee>();
        Vacations = new List<Absense>();
    }
}