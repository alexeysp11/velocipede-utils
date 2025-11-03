using System.Linq.Expressions;
using VelocipedeUtils.Ubp.Core.Dto;
using VelocipedeUtils.Ubp.Core.Enums;
using VelocipedeUtils.Ubp.Core.Extensions;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Core.Domain.Filtering;

/// <summary>
/// Class for applying filters against initial datasets.
/// </summary>
public class CommonDataFilter : ICommonDataFilter
{
    private AppSettings _appSettings;

    public CommonDataFilter(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    /// <summary>
    /// Applies filter to the employees collection.
    /// </summary>
    public IEnumerable<Employee> FilterEmployees(
        EmployeeDto employeeDto,
        FilterOptionType filterOptions,
        Func<Expression<Func<Employee, bool>>, List<Employee>> getEmployees)
    {
        // 1. Build the main filter expression.
        Expression<Func<Employee, bool>> filterExpression = x => 
            x.BirthDate >= employeeDto.DateMin && x.BirthDate <= employeeDto.DateMax;

        // 2. Add additional filters based on provided criteria.
        if (!string.IsNullOrEmpty(employeeDto.FullName))
        {
            filterExpression = filterExpression.AndAlso(x => x.FullName.Contains(employeeDto.FullName));
        }
        if (!string.IsNullOrEmpty(employeeDto.Gender))
        {
            filterExpression = filterExpression.AndAlso(x => x.Gender.ToString() == employeeDto.Gender);
        }
        // if (!string.IsNullOrEmpty(employeeDto.JobTitle))
        // {
        //     filterExpression = filterExpression.AndAlso(x => x.JobTitle.ToString() == employeeDto.JobTitle);
        // }
        // if (!string.IsNullOrEmpty(employeeDto.Department))
        // {
        //     filterExpression = filterExpression.AndAlso(x => x.Department.ToString() == employeeDto.Department);
        // }

        // 3. Apply the filter.
        var filteredEmployees = getEmployees(filterExpression);

        // 4. Handle exclude filter.
        if (filterOptions == FilterOptionType.ExcludeEmployee)
        {
            var allEmployees = getEmployees(x => true);
            return allEmployees.Except(filteredEmployees);
        }

        return filteredEmployees;
    }

    /// <summary>
    /// Applies filters to the vacation collection.
    /// </summary>
    public IEnumerable<Absense> FilterVacations(
        EmployeeDto employeeDto,
        string currentFullName,
        FilterOptionType filterOptions,
        Func<Expression<Func<Employee, bool>>, List<Employee>> getEmployees,
        Func<Expression<Func<Absense, bool>>, List<Absense>> getVacations)
    {
        if (employeeDto.IsEmpty() && !string.IsNullOrEmpty(currentFullName))
        {
            return FilterVacationsByCurrentEmployee(currentFullName, getVacations, filterOptions);
        }
        var employees = FilterEmployees(employeeDto, FilterOptionType.NoFiltersApplied, getEmployees);
        var vacations = employees.SelectMany(e => getVacations(v => v.Employee.FullName == e.FullName)).ToList();
        return FilterVacationsByCurrentEmployee(currentFullName, getVacations, filterOptions, vacations);
    }

    /// <summary>
    /// Filter vacations by current employee name.
    /// </summary>
    private IEnumerable<Absense> FilterVacationsByCurrentEmployee(
        string currentFullName,
        Func<Expression<Func<Absense, bool>>, List<Absense>> getVacations,
        FilterOptionType filterOptions,
        List<Absense> existingVacations = null)
    {
        // Get vacations of the current employee.
        if (!string.IsNullOrEmpty(currentFullName))
        {
            var currentVacations = getVacations(v => v.Employee.FullName.Contains(currentFullName));
            if (existingVacations == null)
            {
                existingVacations = currentVacations.ToList();
            }
            else
            {
                // Add current vacations that are not already in the list.
                existingVacations.AddRange(currentVacations.Where(v => !existingVacations.Any(x =>
                    x.DateStartActual == v.DateStartActual && x.DateEndActual == v.DateEndActual && x.Employee.FullName == v.Employee.FullName)));
            }
        }

        // Apply filter options.
        if (filterOptions == FilterOptionType.ShowIntersectionsVacations)
            return GetIntersections(existingVacations ?? new List<Absense>(), currentFullName);
        if (filterOptions == FilterOptionType.ExcludeIntersectionsVacations)
            return ExcludeIntersections(existingVacations ?? new List<Absense>(), currentFullName);
        return existingVacations ?? new List<Absense>();
    }

    /// <summary>
    /// Gets a list of intersections between the specified employee's vacations and the vacations of other employees.
    /// </summary>
    private List<Absense> GetIntersections(
        List<Absense> vacations,
        string currentFullName)
    {
        if (string.IsNullOrEmpty(currentFullName))
            return new List<Absense>();
        
        var filteredVacations = new List<Absense>();
        foreach (var vacation in vacations)
        {
            if (!vacation.Employee.FullName.Contains(currentFullName))
            {
                continue;
            }
            filteredVacations.Add(vacation);
            var filtered = vacations
                .Where(x => !x.Employee.FullName.Contains(currentFullName)
                    && x.DateStartActual < vacation.DateEndActual
                    && vacation.DateStartActual < x.DateEndActual);
            filteredVacations.AddRange(filtered);
        }
        return filteredVacations;
    }

    /// <summary>
    /// Gets a list of vacations that do not intersect with the specified employee's vacations.
    /// </summary>
    private List<Absense> ExcludeIntersections(
        List<Absense> vacations,
        string currentFullName)
    {
        if (string.IsNullOrEmpty(currentFullName))
            return new List<Absense>();

        var excludeList = new List<Absense>();
        foreach (var vacation in vacations)
        {
            if (vacation.Employee.FullName.Contains(currentFullName))
            {
                excludeList.Add(vacation);
                continue;
            }
            var hasIntersection = vacations
                .Where(x => x.Employee.FullName.Contains(currentFullName)
                    && x.DateStartActual < vacation.DateEndActual && vacation.DateStartActual < x.DateEndActual)
                .Any();
            if (!hasIntersection)
            {
                excludeList.Add(vacation);
            }
        }
        return excludeList;
    }
}