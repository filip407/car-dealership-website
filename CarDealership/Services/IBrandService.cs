using CarDealership.Models;

namespace CarDealership.Services;

public interface IBrandService
{
    Task<List<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken = default);
    Task<Brand?> GetBrandByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateBrandAsync(Brand brand, CancellationToken cancellationToken = default);
    Task UpdateBrandAsync(Brand brand, CancellationToken cancellationToken = default);
    Task DeleteBrandAsync(int id, CancellationToken cancellationToken = default);
}