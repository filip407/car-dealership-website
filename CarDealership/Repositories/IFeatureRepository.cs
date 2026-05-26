using CarDealership.Models;

namespace CarDealership.Repositories;

public interface IFeatureRepository : IRepository<Feature>
{
    Task<List<Feature>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
