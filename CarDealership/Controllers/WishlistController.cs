using System.Security.Claims;
using CarDealership.Services;
using CarDealership.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var items = await _wishlistService.GetUserWishlistAsync(userId, cancellationToken);

        var viewModels = items.Select(w => new WishlistItemViewModel
        {
            Id = w.Id,
            CarId = w.CarId,
            BrandName = w.Car?.Brand?.Name ?? string.Empty,
            ModelName = w.Car?.ModelName ?? string.Empty,
            Price = w.Car?.Price ?? 0,
            Year = w.Car?.Year ?? 0,
            ImagePath = w.Car?.ImagePath,
            AddedAt = w.AddedAt
        }).ToList();

        return View(viewModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int carId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _wishlistService.AddAsync(userId, carId, cancellationToken);
        return RedirectToAction("Details", "Cars", new { id = carId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int carId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _wishlistService.RemoveByCarIdAsync(userId, carId, cancellationToken);
        return RedirectToAction("Details", "Cars", new { id = carId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromList(int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _wishlistService.RemoveByIdAsync(id, userId, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
