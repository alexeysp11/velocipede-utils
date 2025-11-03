using System.Collections.Generic;

namespace VelocipedeUtils.Ubp.Core.Models.Pipes;

/// <summary>
/// Parameters used for the purpose of generating initial datasets.
/// </summary>
public class PipeParams
{
    /// <summary>
    /// Number of employees in the initial dataset.
    /// </summary>
    public int EmployeeQty { get; }
    
    /// <summary>
    /// Number of valid vacations, mapped to an employee.
    /// </summary>
    public List<int> VacationIntervals { get; }
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PipeParams(int employeeQty, List<int> vacationIntervals)
    {
        EmployeeQty = employeeQty;
        VacationIntervals = vacationIntervals;
    }
}