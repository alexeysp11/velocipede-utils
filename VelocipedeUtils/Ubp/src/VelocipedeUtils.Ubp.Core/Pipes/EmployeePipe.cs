using System.Collections.Generic;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Ubp.Core.Models.Pipes;
using VelocipedeUtils.Ubp.Core.Domain.DatasetGenerators;

namespace VelocipedeUtils.Ubp.Core.Pipes;

/// <summary>
/// Pipe component for generating a collection of employees 
/// </summary>
public class EmployeePipe : AbstractPipe
{
    /// <summary>
    /// Constructor of the pipe complonent 
    /// </summary>
    public EmployeePipe(AppSettings appSettings, System.Action<PipeResult> function) : base(appSettings, function)
    {
    }
    
    /// <summary>
    /// Method that implements a generating algorithm 
    /// </summary>
    public override void Handle(PipeResult result)
    {
        IEmployeeGenerator generator = new EmployeeGenerator(_appSettings);
        result.Employees = generator.GenerateEmployees(result.PipeParams.EmployeeQty, base.GenerateDate);
        _function(result);
    }
}