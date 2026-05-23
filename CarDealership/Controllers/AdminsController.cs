using CarDealership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers;

[Authorize(Roles = "Admin")]
public class AdminsController : Controller
{
	private readonly UserManager<ApplicationUser> _userManager;

	public AdminsController(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}

	[HttpGet]
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(string email, string fullName, string password)
	{
		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(password))
		{
			ModelState.AddModelError(string.Empty, "Toate câmpurile sunt obligatorii.");
			return View();
		}

		var existingUser = await _userManager.FindByEmailAsync(email);
		if (existingUser != null)
		{
			ModelState.AddModelError(string.Empty, "Acest email este deja folosit de alt cont.");
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
			await _userManager.AddToRoleAsync(newUser, "Admin");
			return RedirectToAction("Index", "Home");
		}

		foreach (var error in result.Errors)
		{
			ModelState.AddModelError(string.Empty, error.Description);
		}

		return View();
	}
}