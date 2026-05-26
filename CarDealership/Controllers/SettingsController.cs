using CarDealership.Data;
using CarDealership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers;

[Authorize(Roles = "Admin")]
public class SettingsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SettingsController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Brands = await _context.Brands.OrderBy(b => b.Name).ToListAsync();
        ViewBag.Agents = await _userManager.GetUsersInRoleAsync("Admin");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            _context.Brands.Add(new Brand { Name = name });
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
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