using CarDealership.Models;
using CarDealership.Repositories;

namespace CarDealership.Services;

public class FeatureService : IFeatureService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FeatureService> _logger;

    public FeatureService(IUnitOfWork unitOfWork, ILogger<FeatureService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Feature>> GetAllFeaturesAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Features.GetAllAsync(cancellationToken);
    }

    public async Task<Feature?> GetFeatureByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Features.GetByIdAsync(id, cancellationToken);
    }

    public async Task<List<Feature>> GetFeaturesByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Features.GetByIdsAsync(ids, cancellationToken);
    }

    public async Task CreateFeatureAsync(Feature feature, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Features.AddAsync(feature, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Dotare noua adaugata: {Name}", feature.Name);
    }

    public async Task UpdateFeatureAsync(Feature feature, CancellationToken cancellationToken = default)
    {
        _unitOfWork.Features.Update(feature);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Dotarea cu ID-ul {Id} a fost actualizata.", feature.Id);
    }

    public async Task DeleteFeatureAsync(int id, CancellationToken cancellationToken = default)
    {
        var feature = await _unitOfWork.Features.GetByIdAsync(id, cancellationToken);
        if (feature != null)
        {
            _unitOfWork.Features.Delete(feature);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Dotarea cu ID-ul {Id} a fost stearsa.", id);
        }
    }
}
