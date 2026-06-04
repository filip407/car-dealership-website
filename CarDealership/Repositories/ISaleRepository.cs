using CarDealership.Models;

namespace CarDealership.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<List<Sale>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
}
