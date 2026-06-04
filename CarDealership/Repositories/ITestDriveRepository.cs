using CarDealership.Models;

namespace CarDealership.Repositories;

public interface ITestDriveRepository : IRepository<TestDrive>
{
    Task<List<TestDrive>> GetByUserIdWithDetailsAsync(string userId, CancellationToken cancellationToken = default);
    Task<List<TestDrive>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string userId, int carId, CancellationToken cancellationToken = default);
    Task<TestDrive?> GetByIdAndUserAsync(int id, string userId, CancellationToken cancellationToken = default);
    Task RemoveAllByCarIdAsync(int carId, CancellationToken cancellationToken = default);
}
