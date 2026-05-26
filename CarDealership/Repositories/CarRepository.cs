using CarDealership.Data;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Repositories;

public class CarRepository : Repository<Car>, ICarRepository
{
    public CarRepository(AppDbContext context) : base(context) { }

    public async Task<List<Car>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Agent)
            .Include(c => c.Features)
            .OrderByDescending(c => c.AddedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Car?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Agent)
            .Include(c => c.Features)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<List<Car>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Where(c => c.BrandId == brandId)
            .Include(c => c.Brand)
            .Include(c => c.Agent)
            .Include(c => c.Features)
            .OrderByDescending(c => c.AddedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cars.CountAsync(cancellationToken);
    }

    public async Task<List<Car>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Agent)
            .Include(c => c.Features)
            .OrderByDescending(c => c.AddedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
