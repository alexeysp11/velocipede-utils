using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Ubp.Core.Domain.Filtering;
using VelocipedeUtils.Ubp.Core.Dto;
using VelocipedeUtils.Ubp.Core.Enums;
using VelocipedeUtils.Ubp.Core.Models;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Ubp.Core.DbContexts;
using VelocipedeUtils.Ubp.Core.Repositories;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Controllers;

[Authorize]
public class HrmController : Controller
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<HrmController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommonDataFilter _commonFilter;
    private readonly UbpDbContext _context;

    public HrmController(
        AppSettings appSettings,
        ILogger<HrmController> logger,
        IUnitOfWork unitOfWork,
        ICommonDataFilter commonFilter,
        UbpDbContext context)
    {
        _appSettings = appSettings;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _commonFilter = commonFilter;
        _context = context;
    }

    public async Task<IActionResult> Employees()
    {
        IEnumerable<Employee> employees = null;
        try
        {
            employees = await _context.Employees.Include(x => x.OrganizationItems).ToListAsync();
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return View(employees);
    }

    public async Task<IActionResult> EmployeeDetails(long id)
    {
        Employee employee = null;
        try
        {
            employee = await _context.Employees.Include(x => x.OrganizationItems).FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return View(employee);
    }

    public async Task<IActionResult> Absenses()
    {
        IEnumerable<Absense> absenses = null;
        try
        {
            // Restore previously filtered elements.
            // var uidObj = TempData[CacheUidType.VacationsUid.ToString()];
            // if (uidObj != null && !string.IsNullOrEmpty(uidObj.ToString()))
            // {
            //     var vacationsFiltered = _unitOfWork.GetFilteredVacations(uidObj.ToString()).ToList();
            //     uidObj = string.Empty;
            //     return View(vacationsFiltered);
            // }

            // Set info about filters.
            TempData[CacheUidType.FilterInfoVacations.ToString()] = FilterOptionType.NoFiltersApplied;
            TempData[CacheUidType.EmployeeInfoVacations.ToString()] = FilterOptionType.NoFiltersApplied;
            TempData[CacheUidType.FilterOptionsVacations.ToString()] = FilterOptionType.NoFiltersApplied;

            // Get all elements in a current year.
            var now = DateTime.Now;
            var firstDay = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            absenses = await _context.Absenses
                .Where(x => x.DateStartActual >= firstDay && x.DateEndActual <= lastDay)
                .Include(x => x.Employee)
                    .ThenInclude(employee => employee.OrganizationItems)
                .ToListAsync();;
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return View(absenses);
    }

    public IActionResult NewVacation()
    {
        return View();
    }

    [HttpPost("[action]")]
    [Route("/Home")]
    public IActionResult FilterVacations(
        string fullName,
        int? ageMin,
        int? ageMax,
        string gender,
        string jobTitle,
        string department,
        string currentFullName,
        FilterOptionType filterOptions)
    {
        try
        {
            // Get filtered data.
            var employeeDto = new EmployeeDto
            {
                FullName = fullName,
                AgeMin = ageMin == null ? _appSettings.EmployeeMinAge : (int)ageMin,
                AgeMax = ageMax == null ? _appSettings.EmployeeMaxAge : (int)ageMax,
                Gender = gender,
                JobTitle = jobTitle,
                Department = department
            };
            var vacations = _commonFilter.FilterVacations(employeeDto, currentFullName, filterOptions, _unitOfWork.GetEmployees, _unitOfWork.GetVacations);

            // Insert filtered data and get UID.
            string uid = _unitOfWork.InsertFilteredVacations(vacations);

            // Store UID and  in views.
            TempData[CacheUidType.VacationsUid.ToString()] = uid;

            // Store info about filtering.
            TempData[CacheUidType.EmployeeFullname.ToString()] = fullName;
            TempData[CacheUidType.EmployeeMinAge.ToString()] = ageMin;
            TempData[CacheUidType.EmployeeMaxAge.ToString()] = ageMax;
            TempData[CacheUidType.EmployeeGender.ToString()] = gender;
            TempData[CacheUidType.EmployeeJobTitle.ToString()] = jobTitle;
            TempData[CacheUidType.EmployeeDepartment.ToString()] = department;
            TempData[CacheUidType.CurrentEmployeeFullname.ToString()] = currentFullName;
            TempData[CacheUidType.FilterOptionsVacations.ToString()] = filterOptions;
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("Absenses");
    }

    public IActionResult AddNewVaction(
        string fullName,
        System.DateTime beginDate,
        System.DateTime endDate)
    {
        try
        {
            if (endDate > beginDate && (endDate - beginDate).Days <= 14)
                _unitOfWork.InsertVacation(fullName ?? string.Empty, beginDate, endDate);
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("Absenses");
    }

    public IActionResult Hiring()
    {
        return View();
    }
}