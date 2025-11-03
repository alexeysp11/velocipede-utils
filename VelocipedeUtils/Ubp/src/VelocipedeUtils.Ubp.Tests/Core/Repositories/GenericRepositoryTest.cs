using System;
using System.Linq;
using Xunit;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.UnifiedBusinessPlatform.Core.Repositories;

namespace Tests.VelocipedeUtils.UnifiedBusinessPlatform.Core.Repositories;

public class GenericRepositoryTest
{
    [Fact]
    public void Get_UsingTheFunctionAfterConstructor_ReturnsEmptyCollections()
    {
        // Arrange 
        var employees = new GenericRepository<Employee>();
        var vacations = new GenericRepository<Absense>();

        // Act 
        var employeeCollection = employees.Get();
        var vacationCollection = vacations.Get();

        // Assert 
        Assert.True(employeeCollection.ToList().Count == 0);
        Assert.True(vacationCollection.ToList().Count == 0);
    }

    public void Insert_InsertOneRecord_OneElementsInsideReturnedCollection()
    {
        // Arrange 
        var employees = new GenericRepository<Employee>();
        var vacations = new GenericRepository<Absense>();
        var employee = new Employee() 
        {
            FullName = "Random FullName", 
            Gender = Gender.Male, 
            JobTitle = JobTitle.ChiefExecutiveOfficer, 
            Department = Department.Administration, 
            BirthDate = new System.DateTime(1978, 12, 01)
        };
        var vacation = new Absense() 
        {
            BeginDate = new System.DateTime(2023, 05, 11), 
            EndDate = new System.DateTime(2023, 05, 18), 
            Employee = employee 
        };

        // Act 
        employees.Insert(employee);
        vacations.Insert(vacation);
        var employeeCollection = employees.Get();
        var vacationCollection = vacations.Get();

        // Assert 
        Assert.True(employeeCollection.ToList().Count == 1);
        Assert.True(vacationCollection.ToList().Count == 1);
        Assert.True(employeeCollection.Where(x => x.FullName == employee.FullName).ToList().Count == 1);
        Assert.True(vacationCollection.Where(x => x.Employee.FullName == employee.FullName).ToList().Count == 1);
    }
}