using CarDealership.Models;

namespace CarDealership.Services;

public interface ITestDriveService
{
    Task<List<TestDrive>> GetUserTestDrivesAsync(string userId, CancellationToken cancellationToken = default);
    Task<List<TestDrive>> GetAllTestDrivesAsync(CancellationToken cancellationToken = default);
    Task<bool> IsAlreadyBookedAsync(string userId, int carId, CancellationToken cancellationToken = default);
    Task BookAsync(string userId, int carId, DateTime scheduledAt, string? notes, CancellationToken cancellationToken = default);
    Task<bool> CancelAsync(int id, string userId, CancellationToken cancellationToken = default);
    Task<bool> ConfirmAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
