using System.Security.Claims;
using CarDealership.Models;
using CarDealership.Services;
using CarDealership.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarDealership.Controllers;

[Authorize(Roles = "Admin")]
public class CarsController : Controller
{
    private readonly ICarService _carService;
    private readonly IBrandService _brandService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CarsController(ICarService carService, IBrandService brandService, IWebHostEnvironment webHostEnvironment)
    {
        _carService = carService;
        _brandService = brandService;
        _webHostEnvironment = webHostEnvironment;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(int? brandId, CancellationToken cancellationToken)
    {
        List<Car> cars;
        if (brandId.HasValue)
        {
            cars = await _carService.GetCarsByBrandAsync(brandId.Value, cancellationToken);
        }
        else
        {
            cars = await _carService.GetAllCarsAsync(cancellationToken);
        }
        return View(cars);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Brands = new SelectList(brands, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Car car, IFormFile? imageFile, CancellationToken cancellationToken)
    {
        ModelState.Remove("Brand");
        ModelState.Remove("Agent");
        ModelState.Remove("AgentId");

        if (ModelState.IsValid)
        {
            car.AgentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _carService.CreateCarAsync(car, imageFile, _webHostEnvironment.WebRootPath, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Brands = new SelectList(brands, "Id", "Name", car.BrandId);
        return View(car);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null)
        {
            return NotFound();
        }

        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Brands = new SelectList(brands, "Id", "Name", car.BrandId);
        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Car car, IFormFile? imageFile, CancellationToken cancellationToken)
    {
        if (id != car.Id)
        {
            return NotFound();
        }

        ModelState.Remove("Brand");
        ModelState.Remove("Agent");

        if (ModelState.IsValid)
        {
            var existingCar = await _carService.GetCarByIdAsync(id, cancellationToken);
            if (existingCar == null) return NotFound();

            car.AgentId = existingCar.AgentId;
            car.ImagePath = existingCar.ImagePath;
            car.AddedAt = existingCar.AddedAt;

            await _carService.UpdateCarAsync(car, imageFile, _webHostEnvironment.WebRootPath, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        ViewBag.Brands = new SelectList(brands, "Id", "Name", car.BrandId);
        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _carService.DeleteCarAsync(id, _webHostEnvironment.WebRootPath, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Sell(int id, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null || car.IsSold)
        {
            return NotFound();
        }
        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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