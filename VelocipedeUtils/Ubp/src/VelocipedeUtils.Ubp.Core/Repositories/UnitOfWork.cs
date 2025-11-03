using System.Collections.Generic;
using System.Linq.Expressions;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Core.Repositories;

/// <summary>
/// Implementation of the Unit of work pattern.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private AppSettings _appSettings;

    public GenericRepository<Employee> EmployeeRepository { get; }
    public GenericRepository<Absense> VacationRepository { get; }
    public FilteredRepository<Employee> EmployeeRepositoryFiltered { get; }
    public FilteredRepository<Absense> VacationRepositoryFiltered { get; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public UnitOfWork(AppSettings appSettings)
    {
        _appSettings = appSettings;

        EmployeeRepository = new GenericRepository<Employee>();
        VacationRepository = new GenericRepository<Absense>();
        EmployeeRepositoryFiltered = new FilteredRepository<Employee>(_appSettings);
        VacationRepositoryFiltered = new FilteredRepository<Absense>(_appSettings);
    }

    /// <summary>
    /// Gets a collection of employees using the specified filter.
    /// </summary>
    public List<Employee> GetEmployees(Expression<Func<Employee, bool>> filter = null)
    {
        return EmployeeRepository.Get(filter: filter).ToList();
    }

    /// <summary>
    /// Gets a collection of vacations using the specified filter.
    /// </summary>
    public List<Absense> GetVacations(Expression<Func<Absense, bool>> filter = null)
    {
        return VacationRepository.Get(filter: filter).ToList();
    }

    /// <summary>
    /// Inserts vacation for the specified user.
    /// </summary>
    public void InsertVacation(string fullName, System.DateTime begin, System.DateTime end)
    {
        // Find employee 
        var employees = EmployeeRepository.Get(filter: x => x.FullName == fullName).ToList();
        if (employees.Count == 0) 
            return;
        
        // Check if the vacations overlap.
        var vacations = VacationRepository
            .Get(filter: x => x.Employee.FullName == fullName
                            && (
                                (x.DateStartActual <= begin && x.DateEndActual > begin) 
                                || (x.DateStartActual <= end && x.DateEndActual > end)
                            )).ToList();
        if (vacations.Count == 0)
        {
            VacationRepository.Insert(new Absense
            {
                DateStartActual = begin, 
                DateEndActual = end,
                Employee = employees.First()
            });
        }
    }
    
    /// <summary>
    /// Saves filtered employees.
    /// </summary>
    public string InsertFilteredEmployees(IEnumerable<Employee> entities)
    {
        return EmployeeRepositoryFiltered.InsertFiltered(entities);
    }

    /// <summary>
    /// Saves filtered vacations.
    /// </summary>
    public string InsertFilteredVacations(IEnumerable<Absense> entities)
    {
        return VacationRepositoryFiltered.InsertFiltered(entities);
    }

    /// <summary>
    /// Gets filtered employees.
    /// </summary>
    public IEnumerable<Employee> GetFilteredEmployees(string uid)
    {
        return EmployeeRepositoryFiltered.GetFiltered(uid);
    }

    /// <summary>
    /// Gets filtered vacations.
    /// </summary>
    public IEnumerable<Absense> GetFilteredVacations(string uid)
    {
        return VacationRepositoryFiltered.GetFiltered(uid);
    }
}