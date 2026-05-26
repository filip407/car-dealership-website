using CarDealership.Models;

namespace CarDealership.Services;

public interface IFeatureService
{
    Task<List<Feature>> GetAllFeaturesAsync(CancellationToken cancellationToken = default);
    Task<Feature?> GetFeatureByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Feature>> GetFeaturesByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task CreateFeatureAsync(Feature feature, CancellationToken cancellationToken = default);
    Task UpdateFeatureAsync(Feature feature, CancellationToken cancellationToken = default);
    Task DeleteFeatureAsync(int id, CancellationToken cancellationToken = default);
}
