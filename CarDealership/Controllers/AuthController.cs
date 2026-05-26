using CarDealership.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password, bool rememberMe)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError(string.Empty, "Toate câmpurile sunt obligatorii.");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Date de autentificare invalide.");
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string fullName, string email, string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError(string.Empty, "Toate câmpurile sunt obligatorii.");
            return View();
        }

        if (password != confirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Parolele nu coincid.");
            return View();
        }

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Există deja un cont cu acest email.");
            return View();
        }

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
            await _userManager.AddToRoleAsync(newUser, "Customer");
            await _signInManager.SignInAsync(newUser, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
