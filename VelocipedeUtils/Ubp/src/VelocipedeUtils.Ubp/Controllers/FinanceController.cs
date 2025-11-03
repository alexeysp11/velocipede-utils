using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Ubp.Core.Domain.Filtering;
using VelocipedeUtils.Ubp.Core.Dto;
using VelocipedeUtils.Ubp.Core.Enums;
using VelocipedeUtils.Ubp.Core.Models;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Ubp.Core.DbContexts;
using VelocipedeUtils.Ubp.Core.Repositories;

namespace VelocipedeUtils.Ubp.Controllers;

public class FinanceController : Controller
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<FinanceController> _logger;
    private readonly UbpDbContext _context;

    public FinanceController(
        AppSettings appSettings,
        ILogger<FinanceController> logger,
        UbpDbContext context)
    {
        _appSettings = appSettings;
        _logger = logger;
        _context = context;
    }
    
    [Authorize]
    public IActionResult Investments()
    {
        return View();
    }
}