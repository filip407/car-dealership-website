using CarDealership.Data;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Repositories;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(AppDbContext context) : base(context) { }

    public async Task<List<Sale>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Agent)
            .Include(s => s.Car)
            .ThenInclude(c => c!.Brand)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync(cancellationToken);
    }
}
