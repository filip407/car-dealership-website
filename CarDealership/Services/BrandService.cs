using CarDealership.Models;
using CarDealership.Repositories;

namespace CarDealership.Services;

public class BrandService : IBrandService
{
    private readonly IUnitOfWork _unitOfWork;

    public BrandService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
    }

    public async Task UpdateBrandAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBrandAsync(int id, CancellationToken cancellationToken = default)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id, cancellationToken);
        if (brand != null)
        {
            _unitOfWork.Brands.Delete(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}