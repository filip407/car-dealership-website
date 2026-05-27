using System.Security.Claims;
using CarDealership.Data;
using CarDealership.Mappings;
using CarDealership.Models;
using CarDealership.Services;
using CarDealership.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers;

public class CarsController : Controller
{
    private readonly ICarService _carService;
    private readonly IBrandService _brandService;
    private readonly IFeatureService _featureService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CarsController(
        ICarService carService,
        IBrandService brandService,
        IFeatureService featureService,
        IWebHostEnvironment webHostEnvironment)
    {
        _carService = carService;
        _brandService = brandService;
        _featureService = featureService;
        _webHostEnvironment = webHostEnvironment;
    }

    [Authorize]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var cars = await _carService.GetAllCarsAsync(cancellationToken);
        return View(cars.ToViewModelList());
    }

    [Authorize]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken,
        [FromServices] AppDbContext context, string? returnUrl = null)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        ViewBag.IsInWishlist = await context.WishlistItems
            .AnyAsync(w => w.UserId == userId && w.CarId == id, cancellationToken);
        ViewBag.ReturnUrl = returnUrl;

        return View(car.ToViewModel());
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        var allFeatures = await _featureService.GetAllFeaturesAsync(cancellationToken);

        var viewModel = new CreateCarViewModel
        {
            Brands = brands.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name }).ToList(),
            AllFeatures = allFeatures
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCarViewModel viewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var car = new Car
            {
                BrandId = viewModel.BrandId,
                ModelName = viewModel.ModelName,
                Year = viewModel.Year,
                Price = viewModel.Price,
                AgentId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty
            };

            if (viewModel.SelectedFeatureIds.Any())
            {
                car.Features = await _featureService.GetFeaturesByIdsAsync(viewModel.SelectedFeatureIds, cancellationToken);
            }

            await _carService.CreateCarAsync(car, viewModel.ImageFile, _webHostEnvironment.WebRootPath, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        viewModel.Brands = brands.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name }).ToList();
        viewModel.AllFeatures = await _featureService.GetAllFeaturesAsync(cancellationToken);
        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null) return NotFound();

        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        var allFeatures = await _featureService.GetAllFeaturesAsync(cancellationToken);

        var viewModel = new EditCarViewModel
        {
            Id = car.Id,
            BrandId = car.BrandId,
            ModelName = car.ModelName,
            Year = car.Year,
            Price = car.Price,
            ExistingImagePath = car.ImagePath,
            SelectedFeatureIds = car.Features?.Select(f => f.Id).ToArray() ?? Array.Empty<int>(),
            Brands = brands.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name }).ToList(),
            AllFeatures = allFeatures
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, EditCarViewModel viewModel, CancellationToken cancellationToken)
    {
        if (id != viewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var existingCar = await _carService.GetCarByIdAsync(id, cancellationToken);
            if (existingCar == null) return NotFound();

            existingCar.BrandId = viewModel.BrandId;
            existingCar.ModelName = viewModel.ModelName;
            existingCar.Year = viewModel.Year;
            existingCar.Price = viewModel.Price;

            if (viewModel.SelectedFeatureIds.Any())
                existingCar.Features = await _featureService.GetFeaturesByIdsAsync(viewModel.SelectedFeatureIds, cancellationToken);
            else
                existingCar.Features?.Clear();

            await _carService.UpdateCarAsync(existingCar, viewModel.ImageFile, _webHostEnvironment.WebRootPath, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        viewModel.Brands = brands.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name }).ToList();
        viewModel.AllFeatures = await _featureService.GetAllFeaturesAsync(cancellationToken);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _carService.DeleteCarAsync(id, _webHostEnvironment.WebRootPath, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sell(int id, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null || car.IsSold) return NotFound();
        return View(car.ToViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sell(int id, decimal salePrice,
        [FromServices] AppDbContext context, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null) return NotFound();

        var sale = new Sale
        {
            CarId = car.Id,
            AgentId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
            SalePrice = salePrice,
            SaleDate = DateTime.UtcNow
        };

        context.Sales.Add(sale);
        car.IsSold = true;
        await _carService.UpdateCarAsync(car, null, _webHostEnvironment.WebRootPath, cancellationToken);

        // Curata wishlist-ul si test drive-urile pentru masina vanduta
        var wishlistItems = context.WishlistItems.Where(w => w.CarId == id);
        context.WishlistItems.RemoveRange(wishlistItems);

        var testDrives = context.TestDrives.Where(t => t.CarId == id);
        context.TestDrives.RemoveRange(testDrives);

        await context.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}
