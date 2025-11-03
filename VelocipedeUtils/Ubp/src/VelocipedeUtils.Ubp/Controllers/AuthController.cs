using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Ubp.Core.DbContexts;
using VelocipedeUtils.Ubp.ViewModels;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Controllers;

public class AuthController : Controller
{
    private readonly UbpDbContext _context;

    public AuthController(UbpDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult SignIn()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home", null);
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(SignInViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccounts.FirstOrDefault(x => x.Login == model.Username && x.Password == model.Password);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Incorrect username or password";
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim("UserAccountId", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, "User"),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12),
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity), 
                        authProperties);

                    return RedirectToAction("Index", "Home", null);
                }
            }
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("SignIn");
    }

    [Authorize]
    public async Task<ActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<ActionResult> MyAccount()
    {
        try
        {
            if (long.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserAccountId")?.Value, out long userId) && userId > 0)
            {
                var employeeUserAccount = _context.EmployeeUserAccounts
                    .Where(x => x.UserAccount != null && x.UserAccount.Id == userId)
                    .Where(x => x.Employee != null)
                    .Include(x => x.UserAccount)
                    .Include(x => x.Employee)
                        .ThenInclude(x => x.OrganizationItems)
                    .FirstOrDefault();
                var userAccountGroups = _context.UserAccountGroups
                    .Where(x => x.UserAccount != null && x.UserAccount.Id == userId)
                    .Where(x => x.UserGroup != null)
                    .Include(x => x.UserAccount)
                    .Include(x => x.UserGroup)
                    .ToList();
                var result = new UserAccountViewModel
                {
                    UserAccountId = userId,
                    EmployeeUserAccount = employeeUserAccount,
                    UserAccountGroups = userAccountGroups
                };
                return View(result);
            }
            else
            {
                TempData["ErrorMessage"] = "User account ID not saved";
            }
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return View(null);
    }
}