using CarDealership.Models;
using CarDealership.Repositories;

namespace CarDealership.Services;

public class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SaleService> _logger;

    public SaleService(IUnitOfWork unitOfWork, ILogger<SaleService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Sale>> GetAllSalesAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Sales.GetAllWithDetailsAsync(cancellationToken);
    }
}
