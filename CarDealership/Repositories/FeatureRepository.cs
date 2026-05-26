using CarDealership.Data;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Repositories;

public class FeatureRepository : Repository<Feature>, IFeatureRepository
{
    public FeatureRepository(AppDbContext context) : base(context) { }

    public async Task<List<Feature>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Features
            .Where(f => ids.Contains(f.Id))
            .ToListAsync(cancellationToken);
    }
}
