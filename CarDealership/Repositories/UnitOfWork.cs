using CarDealership.Data;

namespace CarDealership.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private ICarRepository? _cars;
    private IBrandRepository? _brands;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICarRepository Cars => _cars ??= new CarRepository(_context);
    public IBrandRepository Brands => _brands ??= new BrandRepository(_context);

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