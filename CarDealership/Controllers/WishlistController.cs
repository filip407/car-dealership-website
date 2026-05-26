using System.Security.Claims;
using CarDealership.Data;
using CarDealership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly AppDbContext _context;

    public WishlistController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var items = await _context.WishlistItems
            .Where(w => w.UserId == userId)
            .Include(w => w.Car)
            .ThenInclude(c => c!.Brand)
            .OrderByDescending(w => w.AddedAt)
            .ToListAsync(cancellationToken);

        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int carId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var alreadyAdded = await _context.WishlistItems
            .AnyAsync(w => w.UserId == userId && w.CarId == carId, cancellationToken);

        if (!alreadyAdded)
        {
            _context.WishlistItems.Add(new WishlistItem
            {
                UserId = userId!,
                CarId = carId
            });
            await _context.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction("Details", "Cars", new { id = carId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int carId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var item = await _context.WishlistItems
            .FirstOrDefaultAsync(w => w.UserId == userId && w.CarId == carId, cancellationToken);

        if (item != null)
        {
            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction("Details", "Cars", new { id = carId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromList(int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var item = await _context.WishlistItems
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, cancellationToken);

        if (item != null)
        {
            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }
}
