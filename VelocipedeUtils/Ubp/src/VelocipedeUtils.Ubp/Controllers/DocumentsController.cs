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

public class DocumentsController : Controller
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<DocumentsController> _logger;
    private readonly UbpDbContext _context;

    public DocumentsController(
        AppSettings appSettings,
        ILogger<DocumentsController> logger,
        UbpDbContext context)
    {
        _appSettings = appSettings;
        _logger = logger;
        _context = context;
    }
    
    [Authorize]
    public IActionResult Internal()
    {
        return View();
    }
    
    [Authorize]
    public IActionResult KnowledgeBase()
    {
        return View();
    }
}