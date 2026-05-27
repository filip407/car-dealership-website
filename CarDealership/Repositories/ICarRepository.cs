using CarDealership.Models;

namespace CarDealership.Repositories;

public interface ICarRepository : IRepository<Car>
{
    Task<List<Car>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<Car?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}