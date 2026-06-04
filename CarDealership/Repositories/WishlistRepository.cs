using CarDealership.Data;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Repositories;

public class WishlistRepository : Repository<WishlistItem>, IWishlistRepository
{
    public WishlistRepository(AppDbContext context) : base(context) { }

    public async Task<List<WishlistItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.WishlistItems
            .Where(w => w.UserId == userId)
            .Include(w => w.Car)
            .ThenInclude(c => c!.Brand)
            .OrderByDescending(w => w.AddedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<WishlistItem?> GetByUserAndCarAsync(string userId, int carId, CancellationToken cancellationToken = default)
    {
        return await _context.WishlistItems
            .FirstOrDefaultAsync(w => w.UserId == userId && w.CarId == carId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string userId, int carId, CancellationToken cancellationToken = default)
    {
        return await _context.WishlistItems
            .AnyAsync(w => w.UserId == userId && w.CarId == carId, cancellationToken);
    }

    public async Task RemoveAllByCarIdAsync(int carId, CancellationToken cancellationToken = default)
    {
        await _context.WishlistItems
            .Where(w => w.CarId == carId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
