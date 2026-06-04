using System.Security.Claims;
using CarDealership.Services;
using CarDealership.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers;

[Authorize]
public class TestDrivesController : Controller
{
    private readonly ITestDriveService _testDriveService;
    private readonly ICarService _carService;

    public TestDrivesController(ITestDriveService testDriveService, ICarService carService)
    {
        _testDriveService = testDriveService;
        _carService = carService;
    }

    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> MyTestDrives(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var testDrives = await _testDriveService.GetUserTestDrivesAsync(userId, cancellationToken);

        var viewModels = testDrives.Select(t => new MyTestDriveViewModel
        {
            Id = t.Id,
            BrandName = t.Car?.Brand?.Name ?? string.Empty,
            ModelName = t.Car?.ModelName ?? string.Empty,
            ScheduledAt = t.ScheduledAt,
            Notes = t.Notes,
            IsConfirmed = t.IsConfirmed
        }).ToList();

        return View(viewModels);
    }

    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Book(int carId, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(carId, cancellationToken);
        if (car == null || car.IsSold) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        if (await _testDriveService.IsAlreadyBookedAsync(userId, carId, cancellationToken))
        {
            TempData["Error"] = "Ai deja un test drive programat pentru această mașină.";
            return RedirectToAction("Details", "Cars", new { id = carId });
        }

        return View(new BookTestDriveViewModel
        {
            CarId = car.Id,
            BrandName = car.Brand?.Name ?? string.Empty,
            ModelName = car.ModelName,
            Year = car.Year,
            Price = car.Price
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Book(int carId, DateTime scheduledAt, string? notes, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(carId, cancellationToken);
        if (car == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        if (await _testDriveService.IsAlreadyBookedAsync(userId, carId, cancellationToken))
        {
            TempData["Error"] = "Ai deja un test drive programat pentru această mașină.";
            return RedirectToAction("Details", "Cars", new { id = carId });
        }

        await _testDriveService.BookAsync(userId, carId, scheduledAt, notes, cancellationToken);

        TempData["Success"] = "Test drive programat cu succes!";
        return RedirectToAction(nameof(MyTestDrives));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Calendar(CancellationToken cancellationToken)
    {
        var testDrives = await _testDriveService.GetAllTestDrivesAsync(cancellationToken);

        var viewModels = testDrives.Select(t => new CalendarTestDriveViewModel
        {
            Id = t.Id,
            BrandName = t.Car?.Brand?.Name ?? string.Empty,
            ModelName = t.Car?.ModelName ?? string.Empty,
            UserFullName = t.User?.FullName ?? string.Empty,
            UserEmail = t.User?.Email ?? string.Empty,
            ScheduledAt = t.ScheduledAt,
            Notes = t.Notes,
            IsConfirmed = t.IsConfirmed
        }).ToList();

        return View(viewModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var success = await _testDriveService.CancelAsync(id, userId, cancellationToken);
        if (!success) return NotFound();

        TempData["Success"] = "Test drive-ul a fost anulat cu succes.";
        return RedirectToAction(nameof(MyTestDrives));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Confirm(int id, CancellationToken cancellationToken)
    {
        await _testDriveService.ConfirmAsync(id, cancellationToken);
        return RedirectToAction(nameof(Calendar));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _testDriveService.DeleteAsync(id, cancellationToken);
        return RedirectToAction(nameof(Calendar));
    }
}
