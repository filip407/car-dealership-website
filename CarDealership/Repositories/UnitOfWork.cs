using CarDealership.Data;

namespace CarDealership.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private ICarRepository? _cars;
    private IBrandRepository? _brands;
    private IFeatureRepository? _features;
    private IWishlistRepository? _wishlists;
    private ITestDriveRepository? _testDrives;
    private ISaleRepository? _sales;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICarRepository Cars => _cars ??= new CarRepository(_context);
    public IBrandRepository Brands => _brands ??= new BrandRepository(_context);
    public IFeatureRepository Features => _features ??= new FeatureRepository(_context);
    public IWishlistRepository Wishlists => _wishlists ??= new WishlistRepository(_context);
    public ITestDriveRepository TestDrives => _testDrives ??= new TestDriveRepository(_context);
    public ISaleRepository Sales => _sales ??= new SaleRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
