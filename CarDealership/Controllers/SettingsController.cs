using CarDealership.Models;
using CarDealership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers;

[Authorize(Roles = "Admin")]
public class SettingsController : Controller
{
    private readonly IBrandService _brandService;
    private readonly UserManager<ApplicationUser> _userManager;

    public SettingsController(IBrandService brandService, UserManager<ApplicationUser> userManager)
    {
        _brandService = brandService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Agents = await _userManager.GetUsersInRoleAsync("Admin");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBrand(string name, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(name))
        {
            await _brandService.CreateBrandAsync(new Brand { Name = name }, cancellationToken);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAgent(string email, string fullName, string password)
    {
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                TempData["Error"] = "Eroare la creare: " + errors;
                return RedirectToAction(nameof(Index));
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAgent(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id != currentUser?.Id)
            {
                await _userManager.DeleteAsync(user);
            }
        }
        return RedirectToAction(nameof(Index));
    }
}