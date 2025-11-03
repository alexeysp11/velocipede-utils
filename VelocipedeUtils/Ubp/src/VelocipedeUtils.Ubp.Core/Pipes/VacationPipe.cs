using System.Linq;
using System.Collections.Generic;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Ubp.Core.Models.Pipes;
using VelocipedeUtils.Ubp.Core.Domain.DatasetGenerators;

namespace VelocipedeUtils.Ubp.Core.Pipes;

/// <summary>
/// Pipe component for generating a collection of employees.
/// </summary>
public class VacationPipe : AbstractPipe
{
    /// <summary>
    /// Constructor of the pipe complonent.
    /// </summary>
    public VacationPipe(AppSettings appSettings, System.Action<PipeResult> function) : base(appSettings, function)
    {
    }

    /// <summary>
    /// Generate vacations.
    /// </summary>
    private List<Absense> GenerateVacations(List<Employee> employees, List<int> vacationIntervals)
    {
        var vacations = new List<Absense>();
        foreach (var employee in employees)
        {
            IVacationGenerator generator = new VacationGenerator();
            var employeeVacations = generator.GenerateVacations(employee, vacationIntervals, GenerateDate);
            vacations.AddRange(employeeVacations);
        }
        return vacations;
    }

    /// <summary>
    /// Add vacation.
    /// </summary>
    public static void AddVacation(PipeResult result, string fullName, System.DateTime begin, System.DateTime end)
    {
        // Get available slots for the employee.
        var slots = result.Vacations
            .Where(x => x.Employee.FullName == fullName 
                        && x.DateStartActual <= begin 
                        && x.DateEndActual > begin 
                        && x.DateStartActual < end 
                        && x.DateEndActual >= end).ToList();
        if (slots.Count == 0)
            return;

        // Generate for the employee.
        var employee = result.Employees.FirstOrDefault(x => x.FullName == fullName);
        var vacation = new Absense
        {
            DateStartActual = begin,
            DateEndActual = end,
            Employee = employee
        };
        result.Vacations.Add(vacation);
    }
    
    /// <summary>
    /// Method that implements a generating algorithm.
    /// </summary>
    public override void Handle(PipeResult result)
    {
        result.Vacations = GenerateVacations(result.Employees, result.PipeParams.VacationIntervals);
        _function(result);
    }
}