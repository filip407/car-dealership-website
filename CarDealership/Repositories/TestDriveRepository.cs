using CarDealership.Data;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Repositories;

public class TestDriveRepository : Repository<TestDrive>, ITestDriveRepository
{
    public TestDriveRepository(AppDbContext context) : base(context) { }

    public async Task<List<TestDrive>> GetByUserIdWithDetailsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.TestDrives
            .Where(t => t.UserId == userId)
            .Include(t => t.Car)
            .ThenInclude(c => c!.Brand)
            .OrderByDescending(t => t.ScheduledAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TestDrive>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TestDrives
            .Include(t => t.Car)
            .ThenInclude(c => c!.Brand)
            .Include(t => t.User)
            .OrderBy(t => t.ScheduledAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string userId, int carId, CancellationToken cancellationToken = default)
    {
        return await _context.TestDrives
            .AnyAsync(t => t.CarId == carId && t.UserId == userId, cancellationToken);
    }

    public async Task<TestDrive?> GetByIdAndUserAsync(int id, string userId, CancellationToken cancellationToken = default)
    {
        return await _context.TestDrives
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellationToken);
    }

    public async Task RemoveAllByCarIdAsync(int carId, CancellationToken cancellationToken = default)
    {
        await _context.TestDrives
            .Where(t => t.CarId == carId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
