using CarDealership.Models;
using CarDealership.Repositories;

namespace CarDealership.Services;

public class TestDriveService : ITestDriveService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TestDriveService> _logger;

    public TestDriveService(IUnitOfWork unitOfWork, ILogger<TestDriveService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<TestDrive>> GetUserTestDrivesAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.TestDrives.GetByUserIdWithDetailsAsync(userId, cancellationToken);
    }

    public async Task<List<TestDrive>> GetAllTestDrivesAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.TestDrives.GetAllWithDetailsAsync(cancellationToken);
    }

    public async Task<bool> IsAlreadyBookedAsync(string userId, int carId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.TestDrives.ExistsAsync(userId, carId, cancellationToken);
    }

    public async Task BookAsync(string userId, int carId, DateTime scheduledAt, string? notes, CancellationToken cancellationToken = default)
    {
        var testDrive = new TestDrive
        {
            CarId = carId,
            UserId = userId,
            ScheduledAt = scheduledAt.ToUniversalTime(),
            Notes = notes
        };
        await _unitOfWork.TestDrives.AddAsync(testDrive, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Test drive programat pentru masina {CarId} de catre {UserId}.", carId, userId);
    }

    public async Task<bool> CancelAsync(int id, string userId, CancellationToken cancellationToken = default)
    {
        var testDrive = await _unitOfWork.TestDrives.GetByIdAndUserAsync(id, userId, cancellationToken);
        if (testDrive == null) return false;

        _unitOfWork.TestDrives.Delete(testDrive);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Test drive {Id} anulat de utilizatorul {UserId}.", id, userId);
        return true;
    }

    public async Task<bool> ConfirmAsync(int id, CancellationToken cancellationToken = default)
    {
        var testDrive = await _unitOfWork.TestDrives.GetByIdAsync(id, cancellationToken);
        if (testDrive == null) return false;

        testDrive.IsConfirmed = true;
        _unitOfWork.TestDrives.Update(testDrive);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Test drive {Id} confirmat.", id);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var testDrive = await _unitOfWork.TestDrives.GetByIdAsync(id, cancellationToken);
        if (testDrive == null) return false;

        _unitOfWork.TestDrives.Delete(testDrive);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Test drive {Id} sters.", id);
        return true;
    }
}
