using CarDealership.Models;

namespace CarDealership.Repositories;

public interface ICarRepository : IRepository<Car>
{
    Task<List<Car>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<Car?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<List<Car>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}