using System.Collections.Generic;
using System.Linq;
using Xunit;
using VelocipedeUtils.UnifiedBusinessPlatform.Core.Domain.Filtering;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.UnifiedBusinessPlatform.Core.Repositories;

namespace Tests.VelocipedeUtils.UnifiedBusinessPlatform.Core.Domain.Filtering;

public class CommonDataFilterTest
{
    private FilterOptionsSettings FilterOptionsSettings = SettingsHelper.AppSettings.StringSettings.FilterOptionsSettings;

    #region FilterEmployees
    [Theory]
    [InlineData("")]
    [InlineData("Show")]
    [InlineData("Exclude")]
    public void FilterEmployees_EmptyParameters_ReturnsIdenticalDataset(string filterOptionsParams)
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = "";
        var department = "";
        var filterOptions = string.IsNullOrEmpty(filterOptionsParams) ? string.Empty 
            : (filterOptionsParams == "Exclude" ? FilterOptionsSettings.FindFilterOptionsExcludeEmployee : FilterOptionsSettings.FindFilterOptionsShowEmployee);
        var expectedQty = filterOptionsParams == "Exclude" ? 0 : SettingsHelper.AppSettings.EmployeeQty;

        // Act.
        var employees = filterOptionsParams == "Exclude" ? new List<Employee>() : unitOfWork.GetEmployees().ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(employees, filtered);

        // Assert.
        Assert.True(employees.Count == expectedQty);
        Assert.True(employees.Count == filtered.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_OnlyFullNameSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = "";
        var department = "";
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var fullName = employees.FirstOrDefault().FullName;
        var referenceList = employees.Where(x => x.FullName == fullName).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_OnlyAgeMinSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = 40;
        var ageMax = "";
        var gender = "";
        var jobTitle = "";
        var department = "";
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => x.BirthDate <= System.DateTime.Now.AddYears(-ageMin)).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin.ToString(), ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_OnlyAgeMaxSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = 50;
        var gender = "";
        var jobTitle = "";
        var department = "";
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => x.BirthDate >= System.DateTime.Now.AddYears(-ageMax)).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax.ToString(), gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_OnlyAgeSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = 30;
        var ageMax = 50;
        var gender = "";
        var jobTitle = "";
        var department = "";
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => 
            x.BirthDate <= System.DateTime.Now.AddYears(-ageMin)
            && x.BirthDate >= System.DateTime.Now.AddYears(-ageMax)).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin.ToString(), ageMax.ToString(), gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_OnlyGenderSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = Gender.Male.ToString();
        var jobTitle = "";
        var department = "";
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => x.Gender.ToString() == gender).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_OnlyJobTitleSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = JobTitle.DevelopmentLead.ToString();
        var department = "";
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => x.JobTitle.ToString() == jobTitle).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_OnlyDepartmentSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = "";
        var department = Department.CRMDevelopment.ToString();
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => x.Department.ToString() == department).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_JobTitleAndDepartmentSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = JobTitle.DevelopmentLead.ToString();
        var department = Department.CRMDevelopment.ToString();
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => 
            x.JobTitle.ToString() == jobTitle
            && x.Department.ToString() == department).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_GenderAndJobTitleSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = Gender.Female.ToString();
        var jobTitle = JobTitle.DevelopmentLead.ToString();
        var department = "";
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => 
            x.Gender.ToString() == gender
            && x.JobTitle.ToString() == jobTitle).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_GenderJobTitleAndDepartmentSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = Gender.Female.ToString();
        var jobTitle = JobTitle.DevelopmentLead.ToString();
        var department = Department.Administration.ToString();
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => 
            x.Gender.ToString() == gender
            && x.JobTitle.ToString() == jobTitle
            && x.Department.ToString() == department).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin, ageMax, gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterEmployees_AgeGenderJobTitleAndDepartmentSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = 34;
        var ageMax = 55;
        var gender = Gender.Male.ToString();
        var jobTitle = JobTitle.DevelopmentLead.ToString();
        var department = Department.Administration.ToString();
        var filterOptions = "";

        // Act.
        var employees = unitOfWork.GetEmployees().ToList();
        var referenceList = employees.Where(x => 
            x.BirthDate <= System.DateTime.Now.AddYears(-ageMin)
            && x.BirthDate >= System.DateTime.Now.AddYears(-ageMax)
            && x.Gender.ToString() == gender
            && x.JobTitle.ToString() == jobTitle
            && x.Department.ToString() == department).ToList();
        var filtered = commonDataFilter.FilterEmployees(fullName, ageMin.ToString(), ageMax.ToString(), gender, jobTitle, department, filterOptions, unitOfWork.GetEmployees).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }
    #endregion  // FilterEmployees

    #region FilterVacations
    [Fact]
    public void FilterVacations_EmptyParameters_ReturnsEntireDataset()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = "";
        var department = "";
        var currentFullName = "";
        var filterOptions = "";

        // Act.
        var vacations = unitOfWork.GetVacations().ToList();
        var filtered = commonDataFilter.FilterVacations(fullName, ageMin, ageMax, gender, jobTitle, department, currentFullName, filterOptions, unitOfWork.GetEmployees, unitOfWork.GetVacations).ToList();
        var identical = CompareLists(vacations, filtered);

        // Assert.
        Assert.True(vacations.Count == SettingsHelper.AppSettings.VacationQty);
        Assert.True(vacations.Count == filtered.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterVacations_OnlyCurrentFullNameSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = "";
        var department = "";
        var filterOptions = "";

        // Act.
        var vacations = unitOfWork.GetVacations().ToList();
        var currentFullName = vacations.First().Employee.FullName;
        var referenceList = vacations.Where(x => x.Employee.FullName == currentFullName).ToList();
        var filtered = commonDataFilter.FilterVacations(fullName, ageMin.ToString(), ageMax.ToString(), gender, jobTitle, department, currentFullName, filterOptions, unitOfWork.GetEmployees, unitOfWork.GetVacations).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterVacations_FullNameAndCurrentFullNameTheSameSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = "";
        var department = Department.Administration.ToString();
        var filterOptions = "";

        // Act.
        var vacations = unitOfWork.GetVacations().ToList();
        var fullName = vacations.First().Employee.FullName;
        var currentFullName = fullName;
        var referenceList = vacations.Where(x => x.Employee.FullName == fullName).ToList();
        var filtered = commonDataFilter.FilterVacations(fullName, ageMin, ageMax, gender, jobTitle, department, currentFullName, filterOptions, unitOfWork.GetEmployees, unitOfWork.GetVacations).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterVacations_FullNameAgeDepartmentAndCurrentFullNameSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var ageMin = 34;
        var ageMax = 68;
        var gender = "";
        var jobTitle = "";
        var department = Department.Administration.ToString();
        var filterOptions = "";

        // Act.
        var vacations = unitOfWork.GetVacations().ToList();
        var fullName = vacations.First().Employee.FullName;
        var currentFullName = vacations.Last().Employee.FullName;
        var referenceList = vacations.Where(x => 
            (
                x.Employee.FullName == fullName 
                && x.Employee.BirthDate <= System.DateTime.Now.AddYears(-ageMin)
                && x.Employee.BirthDate >= System.DateTime.Now.AddYears(-ageMax)
                && x.Employee.Department.ToString() == department
            )
            || x.Employee.FullName == currentFullName).ToList();
        var filtered = commonDataFilter.FilterVacations(fullName, ageMin.ToString(), ageMax.ToString(), gender, jobTitle, department, currentFullName, filterOptions, unitOfWork.GetEmployees, unitOfWork.GetVacations).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterVacations_AgeGenderAndCurrentFullNameSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = 34;
        var ageMax = 68;
        var gender = Gender.Female.ToString();
        var jobTitle = "";
        var department = "";
        var filterOptions = "";

        // Act.
        var vacations = unitOfWork.GetVacations().ToList();
        var currentFullName = vacations.First().Employee.FullName;
        var referenceList = vacations.Where(x => 
            (
                x.Employee.Gender.ToString() == gender
                && x.Employee.BirthDate <= System.DateTime.Now.AddYears(-ageMin)
                && x.Employee.BirthDate >= System.DateTime.Now.AddYears(-ageMax)
            )
            || x.Employee.FullName == currentFullName).ToList();
        var filtered = commonDataFilter.FilterVacations(fullName, ageMin.ToString(), ageMax.ToString(), gender, jobTitle, department, currentFullName, filterOptions, unitOfWork.GetEmployees, unitOfWork.GetVacations).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterVacations_OnlyCurrentFullNameAndDepartmentSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = "";
        var ageMax = "";
        var gender = "";
        var jobTitle = "";
        var department = Department.Administration.ToString();
        var filterOptions = "";

        // Act.
        var vacations = unitOfWork.GetVacations().ToList();
        var currentFullName = vacations.First().Employee.FullName;
        var referenceList = vacations.Where(x => 
            x.Employee.FullName == currentFullName
            || x.Employee.Department.ToString() == department).ToList();
        var filtered = commonDataFilter.FilterVacations(fullName, ageMin.ToString(), ageMax.ToString(), gender, jobTitle, department, currentFullName, filterOptions, unitOfWork.GetEmployees, unitOfWork.GetVacations).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }

    [Fact]
    public void FilterVacations_AgeGenderJobTitleAndDepartmentSpecified()
    {
        // Arrange.
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var commonDataFilter = new CommonDataFilter(SettingsHelper.AppSettings);
        var fullName = "";
        var ageMin = 34;
        var ageMax = 55;
        var gender = Gender.Male.ToString();
        var jobTitle = JobTitle.DevelopmentLead.ToString();
        var department = Department.Administration.ToString();
        var currentFullName = "";
        var filterOptions = "";

        // Act.
        var vacations = unitOfWork.GetVacations().ToList();
        var referenceList = vacations.Where(x => 
            x.Employee.BirthDate <= System.DateTime.Now.AddYears(-ageMin)
            && x.Employee.BirthDate >= System.DateTime.Now.AddYears(-ageMax)
            && x.Employee.Gender.ToString() == gender
            && x.Employee.JobTitle.ToString() == jobTitle
            && x.Employee.Department.ToString() == department).ToList();
        var filtered = commonDataFilter.FilterVacations(fullName, ageMin.ToString(), ageMax.ToString(), gender, jobTitle, department, currentFullName, filterOptions, unitOfWork.GetEmployees, unitOfWork.GetVacations).ToList();
        var identical = CompareLists(referenceList, filtered);

        // Assert.
        Assert.True(filtered.Count == referenceList.Count);
        Assert.True(identical);
    }
    #endregion  // FilterVacations

    #region Private methods 
    private bool CompareLists(List<Employee> list1, List<Employee> list2)
    {
        if (list1.Count != list2.Count) 
            return false;
        
        foreach (var item in list1)
        {
            if (list2.Where(x => 
                x.FullName == item.FullName 
                && x.Gender == item.Gender
                && x.JobTitle == item.JobTitle
                && x.Department == item.Department
                && x.BirthDate == item.BirthDate).ToList().Count != 1) 
            {
                return false;
            }
        }
        foreach (var item in list2)
        {
            if (list1.Where(x => 
                x.FullName == item.FullName 
                && x.Gender == item.Gender
                && x.JobTitle == item.JobTitle
                && x.Department == item.Department
                && x.BirthDate == item.BirthDate).ToList().Count != 1) 
            {
                return false;
            }
        }
        return true;
    }
    private bool CompareLists(List<Absense> list1, List<Absense> list2)
    {
        if (list1.Count != list2.Count) 
            return false;
        
        foreach (var item in list1)
        {
            if (list2.Where(x => 
                x.Employee.FullName == item.Employee.FullName 
                && x.Employee.Gender == item.Employee.Gender
                && x.Employee.JobTitle == item.Employee.JobTitle
                && x.Employee.Department == item.Employee.Department
                && x.Employee.BirthDate == item.Employee.BirthDate
                && x.BeginDate == item.BeginDate
                && x.EndDate == item.EndDate).ToList().Count != 1) 
            {
                return false;
            }
        }
        foreach (var item in list2)
        {
            if (list1.Where(x => 
                x.Employee.FullName == item.Employee.FullName 
                && x.Employee.Gender == item.Employee.Gender
                && x.Employee.JobTitle == item.Employee.JobTitle
                && x.Employee.Department == item.Employee.Department
                && x.Employee.BirthDate == item.Employee.BirthDate
                && x.BeginDate == item.BeginDate
                && x.EndDate == item.EndDate).ToList().Count != 1) 
            {
                return false;
            }
        }
        return true;
    }
    #endregion  // Private methods 
}