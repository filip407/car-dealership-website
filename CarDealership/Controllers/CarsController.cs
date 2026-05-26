using System.Security.Claims;
using CarDealership.Data;
using CarDealership.Models;
using CarDealership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers;

public class CarsController : Controller
{
    private readonly ICarService _carService;
    private readonly IBrandService _brandService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly AppDbContext _context;

    public CarsController(ICarService carService, IBrandService brandService, IWebHostEnvironment webHostEnvironment, AppDbContext context)
    {
        _carService = carService;
        _brandService = brandService;
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    [Authorize]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var cars = await _carService.GetAllCarsAsync(cancellationToken);
        return View(cars);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        ViewBag.IsInWishlist = await _context.WishlistItems
            .AnyAsync(w => w.UserId == userId && w.CarId == id, cancellationToken);

        return View(car);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Brands = new SelectList(brands, "Id", "Name");
        ViewBag.AllFeatures = await _context.Features.ToListAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Car car, IFormFile? imageFile, int[] selectedFeatures, CancellationToken cancellationToken)
    {
        ModelState.Remove("Brand");
        ModelState.Remove("Agent");
        ModelState.Remove("AgentId");
        ModelState.Remove("Features");

        if (ModelState.IsValid)
        {
            car.AgentId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            if (selectedFeatures != null && selectedFeatures.Any())
            {
                car.Features = await _context.Features.Where(f => selectedFeatures.Contains(f.Id)).ToListAsync(cancellationToken);
            }
            await _carService.CreateCarAsync(car, imageFile, _webHostEnvironment.WebRootPath, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Brands = new SelectList(brands, "Id", "Name", car.BrandId);
        ViewBag.AllFeatures = await _context.Features.ToListAsync(cancellationToken);
        return View(car);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var car = await _context.Cars.Include(c => c.Features).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (car == null) return NotFound();
        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Brands = new SelectList(brands, "Id", "Name", car.BrandId);
        ViewBag.AllFeatures = await _context.Features.ToListAsync(cancellationToken);
        ViewBag.SelectedFeatureIds = car.Features.Select(f => f.Id).ToList();
        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, Car car, IFormFile? imageFile, int[] selectedFeatures, CancellationToken cancellationToken)
    {
        if (id != car.Id) return NotFound();
        ModelState.Remove("Brand");
        ModelState.Remove("Agent");
        ModelState.Remove("Features");
        if (ModelState.IsValid)
        {
            var existingCar = await _context.Cars.Include(c => c.Features).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (existingCar == null) return NotFound();
            existingCar.BrandId = car.BrandId;
            existingCar.ModelName = car.ModelName;
            existingCar.Year = car.Year;
            existingCar.Price = car.Price;
            if (selectedFeatures != null)
                existingCar.Features = await _context.Features.Where(f => selectedFeatures.Contains(f.Id)).ToListAsync(cancellationToken);
            else
                existingCar.Features.Clear();
            await _carService.UpdateCarAsync(existingCar, imageFile, _webHostEnvironment.WebRootPath, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Brands = new SelectList(brands, "Id", "Name", car.BrandId);
        ViewBag.AllFeatures = await _context.Features.ToListAsync(cancellationToken);
        return View(car);
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
        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sell(int id, decimal salePrice, [FromServices] AppDbContext context, CancellationToken cancellationToken)
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
        await context.SaveChangesAsync(cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
