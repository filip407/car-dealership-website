using CarDealership.Models;

namespace CarDealership.Repositories;

public interface IWishlistRepository : IRepository<WishlistItem>
{
    Task<List<WishlistItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<WishlistItem?> GetByUserAndCarAsync(string userId, int carId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string userId, int carId, CancellationToken cancellationToken = default);
    Task RemoveAllByCarIdAsync(int carId, CancellationToken cancellationToken = default);
}
