using CarDealership.Models;
using CarDealership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers;

[Authorize(Roles = "Admin")]
public class BrandsController : Controller
{
	private readonly IBrandService _brandService;

	public BrandsController(IBrandService brandService)
	{
		_brandService = brandService;
	}

	[AllowAnonymous]
	public async Task<IActionResult> Index(CancellationToken cancellationToken)
	{
		var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
		return View(brands);
	}

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Brand brand, CancellationToken cancellationToken)
	{
		if (ModelState.IsValid)
		{
			await _brandService.CreateBrandAsync(brand, cancellationToken);
			return RedirectToAction(nameof(Index));
		}
		return View(brand);
	}

	public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
	{
		var brand = await _brandService.GetBrandByIdAsync(id, cancellationToken);
		if (brand == null)
		{
			return NotFound();
		}
		return View(brand);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, Brand brand, CancellationToken cancellationToken)
	{
		if (id != brand.Id)
		{
			return NotFound();
		}

		if (ModelState.IsValid)
		{
			await _brandService.UpdateBrandAsync(brand, cancellationToken);
			return RedirectToAction(nameof(Index));
		}
		return View(brand);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
	{
		await _brandService.DeleteBrandAsync(id, cancellationToken);
		return RedirectToAction(nameof(Index));
	}
}