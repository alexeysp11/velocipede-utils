using System.Collections.Generic;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Core.Domain.DatasetGenerators;

/// <summary>
/// Randomly generates a set of vacations 
/// </summary>
public class VacationGenerator : IVacationGenerator
{
    /// <summary>
    /// Generates specified number of employees mapped to every employee 
    /// </summary>
    public List<Absense> GenerateVacations(
        Employee employee, 
        List<int> vacationIntervals, 
        System.Func<System.DateTime, System.DateTime, System.DateTime> generateDate)
    {
        var result = new List<Absense>();
        var year = System.DateTime.Now.Year;
        foreach (var interval in vacationIntervals)
        {
            System.DateTime start;
            System.DateTime end;
            do
            {
                start = generateDate(new System.DateTime(year, 1, 1), new System.DateTime(year, 12, 31));
                end = start.AddDays(interval);
            }
            while (end.Year != year);
            var vacation = new Absense
            {
                DateStartActual = start,
                DateEndActual = end,
                Employee = employee
            };
            result.Add(vacation);
        }
        return result;
    }
}