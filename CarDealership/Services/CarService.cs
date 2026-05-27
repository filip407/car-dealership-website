using CarDealership.Models;
using CarDealership.Repositories;

namespace CarDealership.Services;

public class CarService : ICarService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CarService> _logger;

    public CarService(IUnitOfWork unitOfWork, ILogger<CarService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Car>> GetAllCarsAsync(CancellationToken cancellationToken = default)
    {
        var cars = await _unitOfWork.Cars.GetAllWithDetailsAsync(cancellationToken);
        return cars.Where(c => !c.IsSold).ToList();
    }

    public async Task<Car?> GetCarByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Cars.GetByIdWithDetailsAsync(id, cancellationToken);
    }


    public async Task CreateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken = default)
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

        await _unitOfWork.Cars.AddAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("O masina noua a fost adaugata: {Model}", car.ModelName);
    }

    public async Task UpdateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken = default)
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

        _unitOfWork.Cars.Update(car);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Masina cu ID-ul {Id} a fost actualizata.", car.Id);
    }

    public async Task DeleteCarAsync(int id, string webRootPath, CancellationToken cancellationToken = default)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(id, cancellationToken);
        if (car != null)
        {
            _unitOfWork.Cars.Delete(car);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Masina cu ID-ul {Id} a fost stearsa.", id);
        }
    }
}
