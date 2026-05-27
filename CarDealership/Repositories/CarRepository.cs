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

}
