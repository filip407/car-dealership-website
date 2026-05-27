using CarDealership.Models;
using CarDealership.Services;
using CarDealership.ViewModels;
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
        var viewModels = brands.Select(b => new BrandViewModel { Id = b.Id, Name = b.Name }).ToList();
        return View(viewModels);
    }

    public IActionResult Create()
    {
        return View(new CreateBrandViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBrandViewModel viewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var brand = new Brand { Name = viewModel.Name };
            await _brandService.CreateBrandAsync(brand, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var brand = await _brandService.GetBrandByIdAsync(id, cancellationToken);
        if (brand == null) return NotFound();

        var viewModel = new EditBrandViewModel { Id = brand.Id, Name = brand.Name };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditBrandViewModel viewModel, CancellationToken cancellationToken)
    {
        if (id != viewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var brand = await _brandService.GetBrandByIdAsync(id, cancellationToken);
            if (brand == null) return NotFound();

            brand.Name = viewModel.Name;
            await _brandService.UpdateBrandAsync(brand, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _brandService.DeleteBrandAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
