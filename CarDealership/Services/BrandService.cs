using CarDealership.Models;
using CarDealership.Repositories;

namespace CarDealership.Services;

public class BrandService : IBrandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BrandService> _logger;

    public BrandService(IUnitOfWork unitOfWork, ILogger<BrandService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Brands.GetAllAsync(cancellationToken);
    }

    public async Task<Brand?> GetBrandByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Brands.GetByIdAsync(id, cancellationToken);
    }

    public async Task CreateBrandAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Brands.AddAsync(brand, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Marca noua adaugata: {Name}", brand.Name);
    }

    public async Task UpdateBrandAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Marca cu ID-ul {Id} a fost actualizata.", brand.Id);
    }

    public async Task DeleteBrandAsync(int id, CancellationToken cancellationToken = default)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id, cancellationToken);
        if (brand != null)
        {
            _unitOfWork.Brands.Delete(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Marca cu ID-ul {Id} a fost stearsa.", id);
        }
    }
}