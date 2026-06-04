using CarDealership.Models;

namespace CarDealership.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICarRepository Cars { get; }
    IRepository<Brand> Brands { get; }
    IFeatureRepository Features { get; }
    IWishlistRepository Wishlists { get; }
    ITestDriveRepository TestDrives { get; }
    ISaleRepository Sales { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
