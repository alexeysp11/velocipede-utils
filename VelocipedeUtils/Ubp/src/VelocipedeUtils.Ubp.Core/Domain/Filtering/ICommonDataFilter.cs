using System.Linq.Expressions;
using VelocipedeUtils.Ubp.Core.Dto;
using VelocipedeUtils.Ubp.Core.Enums;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Core.Domain.Filtering;

public interface ICommonDataFilter
{
    IEnumerable<Employee> FilterEmployees(
        EmployeeDto employeeDto,
        FilterOptionType filterOptions,
        Func<Expression<Func<Employee, bool>>, List<Employee>> getEmployees);
    
    IEnumerable<Absense> FilterVacations(
        EmployeeDto employeeDto,
        string currentFullName,
        FilterOptionType filterOptions,
        Func<Expression<Func<Employee, bool>>, List<Employee>> getEmployees,
        Func<Expression<Func<Absense, bool>>, List<Absense>> getVacations);  
}