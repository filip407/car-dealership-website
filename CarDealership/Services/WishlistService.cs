using CarDealership.Models;
using CarDealership.Repositories;

namespace CarDealership.Services;

public class WishlistService : IWishlistService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WishlistService> _logger;

    public WishlistService(IUnitOfWork unitOfWork, ILogger<WishlistService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<WishlistItem>> GetUserWishlistAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Wishlists.GetByUserIdAsync(userId, cancellationToken);
    }

    public async Task<bool> IsInWishlistAsync(string userId, int carId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Wishlists.ExistsAsync(userId, carId, cancellationToken);
    }

    public async Task AddAsync(string userId, int carId, CancellationToken cancellationToken = default)
    {
        var alreadyAdded = await _unitOfWork.Wishlists.ExistsAsync(userId, carId, cancellationToken);
        if (alreadyAdded) return;

        await _unitOfWork.Wishlists.AddAsync(new WishlistItem { UserId = userId, CarId = carId }, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Masina {CarId} adaugata la wishlist-ul utilizatorului {UserId}.", carId, userId);
    }

    public async Task RemoveByCarIdAsync(string userId, int carId, CancellationToken cancellationToken = default)
    {
        var item = await _unitOfWork.Wishlists.GetByUserAndCarAsync(userId, carId, cancellationToken);
        if (item == null) return;

        _unitOfWork.Wishlists.Delete(item);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Masina {CarId} eliminata din wishlist-ul utilizatorului {UserId}.", carId, userId);
    }

    public async Task RemoveByIdAsync(int itemId, string userId, CancellationToken cancellationToken = default)
    {
        var item = await _unitOfWork.Wishlists.GetByIdAsync(itemId, cancellationToken);
        if (item == null || item.UserId != userId) return;

        _unitOfWork.Wishlists.Delete(item);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Item {ItemId} eliminat din wishlist.", itemId);
    }
}
