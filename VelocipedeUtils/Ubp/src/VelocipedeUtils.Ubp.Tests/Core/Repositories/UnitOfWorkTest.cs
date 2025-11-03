using System.Linq;
using Xunit;
using VelocipedeUtils.UnifiedBusinessPlatform.Core.Models.Configurations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.UnifiedBusinessPlatform.Core.Repositories;

namespace Tests.VelocipedeUtils.UnifiedBusinessPlatform.Core.Repositories;

public class UnitOfWorkTest
{
    [Fact]
    public void Constructor_NoParameters_CorrectNumberOfGeneratedElements()
    {
        // Arrange
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);

        // Act 
        var employees = unitOfWork.GetEmployees();
        var vacations = unitOfWork.GetVacations();

        // Assert 
        Assert.Equal(employees.Count, SettingsHelper.AppSettings.EmployeeQty);
        Assert.Equal(vacations.Count, SettingsHelper.AppSettings.VacationQty);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("IncorrectFullnameOfTheEmployee")]
    public void InsertVacation_IncorrectFullName_RecordIsNotInserted(string fullName)
    {
        // Arrange
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var beginDate = System.DateTime.Now;
        var endDate = beginDate.AddDays(14);

        // Act 
        unitOfWork.InsertVacation(fullName, beginDate, endDate);
        var vacations = unitOfWork.GetVacations();
        var isInserted = vacations.Where(x => 
                x.Employee.FullName == fullName 
                && x.BeginDate == beginDate 
                && x.EndDate == endDate)
            .ToList().Count > 0;

        // Assert 
        Assert.False(isInserted);
        Assert.Equal(vacations.Count, SettingsHelper.AppSettings.VacationQty);
    }

    [Fact]
    public void InsertVacation_CorrectParameters_OnlyOneRecordWasInsertedAndTotalQtyIncrementedByOne()
    {
        // Arrange
        var unitOfWork = new UnitOfWork(SettingsHelper.AppSettings);
        var beginDate = System.DateTime.Now;
        var endDate = beginDate.AddDays(14);
        var fullName = unitOfWork.GetVacations().First().Employee.FullName;

        // Act 
        unitOfWork.InsertVacation(fullName, beginDate, endDate);
        var vacations = unitOfWork.GetVacations();
        var isInserted = vacations.Where(x => 
                x.Employee.FullName == fullName 
                && x.BeginDate == beginDate 
                && x.EndDate == endDate)
            .ToList().Count == 1;

        // Assert 
        Assert.True(isInserted);
        Assert.Equal(vacations.Count, SettingsHelper.AppSettings.VacationQty + 1);
    }
}