using CarDealership.Models;

namespace CarDealership.Services;

public interface IWishlistService
{
    Task<List<WishlistItem>> GetUserWishlistAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> IsInWishlistAsync(string userId, int carId, CancellationToken cancellationToken = default);
    Task AddAsync(string userId, int carId, CancellationToken cancellationToken = default);
    Task RemoveByCarIdAsync(string userId, int carId, CancellationToken cancellationToken = default);
    Task RemoveByIdAsync(int itemId, string userId, CancellationToken cancellationToken = default);
}
