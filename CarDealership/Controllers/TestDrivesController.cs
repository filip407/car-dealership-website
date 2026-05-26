using System.Security.Claims;
using CarDealership.Data;
using CarDealership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers;

[Authorize]
public class TestDrivesController : Controller
{
    private readonly AppDbContext _context;

    public TestDrivesController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> MyTestDrives(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var testDrives = await _context.TestDrives
            .Where(t => t.UserId == userId)
            .Include(t => t.Car)
            .ThenInclude(c => c!.Brand)
            .OrderByDescending(t => t.ScheduledAt)
            .ToListAsync(cancellationToken);

        return View(testDrives);
    }

    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Book(int carId, CancellationToken cancellationToken)
    {
        var car = await _context.Cars
            .Include(c => c.Brand)
            .FirstOrDefaultAsync(c => c.Id == carId && !c.IsSold, cancellationToken);

        if (car == null) return NotFound();

        ViewBag.Car = car;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Book(int carId, DateTime scheduledAt, string? notes, CancellationToken cancellationToken)
    {
        var car = await _context.Cars.FindAsync(new object[] { carId }, cancellationToken);
        if (car == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var testDrive = new TestDrive
        {
            CarId = carId,
            UserId = userId!,
            ScheduledAt = scheduledAt.ToUniversalTime(),
            Notes = notes
        };

        _context.TestDrives.Add(testDrive);
        await _context.SaveChangesAsync(cancellationToken);

        TempData["Success"] = "Test drive programat cu succes!";
        return RedirectToAction(nameof(MyTestDrives));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Calendar(CancellationToken cancellationToken)
    {
        var testDrives = await _context.TestDrives
            .Include(t => t.Car)
            .ThenInclude(c => c!.Brand)
            .Include(t => t.User)
            .OrderBy(t => t.ScheduledAt)
            .ToListAsync(cancellationToken);

        return View(testDrives);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Confirm(int id, CancellationToken cancellationToken)
    {
        var testDrive = await _context.TestDrives.FindAsync(new object[] { id }, cancellationToken);
        if (testDrive == null) return NotFound();

        testDrive.IsConfirmed = true;
        await _context.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Calendar));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var testDrive = await _context.TestDrives.FindAsync(new object[] { id }, cancellationToken);
        if (testDrive != null)
        {
            _context.TestDrives.Remove(testDrive);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Calendar));
    }
}
