using CarDealership.Models;

namespace CarDealership.Services;

public interface ISaleService
{
    Task<List<Sale>> GetAllSalesAsync(CancellationToken cancellationToken = default);
}
