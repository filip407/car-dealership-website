using CarDealership.Models;

namespace CarDealership.Repositories;

public interface IBrandRepository : IRepository<Brand>
{
    Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}