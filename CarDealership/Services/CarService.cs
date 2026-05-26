using CarDealership.Data;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Services;

public class CarService : ICarService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CarService> _logger;

    public CarService(AppDbContext context, ILogger<CarService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Car>> GetAllCarsAsync(CancellationToken cancellationToken)
    {
        return await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Features)
            .Where(c => !c.IsSold)
            .ToListAsync(cancellationToken);
    }

    public async Task<Car?> GetCarByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Features)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task CreateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(webRootPath, "uploads", fileName);

            Directory.CreateDirectory(Path.Combine(webRootPath, "uploads"));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream, cancellationToken);
            }
            car.ImagePath = "/uploads/" + fileName;
        }

        _context.Cars.Add(car);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("O masina noua a fost adaugata in baza de date: {Model}", car.ModelName);
    }

    public async Task UpdateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var uploadsPath = Path.Combine(webRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream, cancellationToken);
            }
            car.ImagePath = "/uploads/" + fileName;
        }

        _context.Cars.Update(car);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Masina cu ID-ul {Id} a fost actualizata.", car.Id);
    }

    public async Task DeleteCarAsync(int id, string webRootPath, CancellationToken cancellationToken)
    {
        var car = await _context.Cars.FindAsync(new object[] { id }, cancellationToken);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Masina cu ID-ul {Id} a fost stearsa.", id);
        }
    }
    public async Task<List<Car>> GetCarsByBrandAsync(int brandId, CancellationToken cancellationToken)
    {
        return await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Features)
            .Where(c => c.BrandId == brandId && !c.IsSold)
            .ToListAsync(cancellationToken);
    }
}