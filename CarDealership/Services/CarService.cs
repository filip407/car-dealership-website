using CarDealership.Models;
using CarDealership.Repositories;
using Microsoft.AspNetCore.Http;

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
        car.ImagePath = await SaveImageAsync(imageFile, webRootPath, cancellationToken);
        await _unitOfWork.Cars.AddAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("O masina noua a fost adaugata: {Model}", car.ModelName);
    }

    public async Task UpdateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken = default)
    {
        var newPath = await SaveImageAsync(imageFile, webRootPath, cancellationToken);
        if (newPath != null)
            car.ImagePath = newPath;

        _unitOfWork.Cars.Update(car);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Masina cu ID-ul {Id} a fost actualizata.", car.Id);
    }

    private static async Task<string?> SaveImageAsync(IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        var uploadsPath = Path.Combine(webRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);

        using var stream = new FileStream(Path.Combine(uploadsPath, fileName), FileMode.Create);
        await imageFile.CopyToAsync(stream, cancellationToken);

        return "/uploads/" + fileName;
    }

    public async Task DeleteCarAsync(int id, string webRootPath, CancellationToken cancellationToken = default)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(id, cancellationToken);
        if (car == null) return;

        _unitOfWork.Cars.Delete(car);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Masina cu ID-ul {Id} a fost stearsa.", id);
    }

    public async Task<bool> SellCarAsync(int carId, string agentId, decimal salePrice, CancellationToken cancellationToken = default)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(carId, cancellationToken);
        if (car == null || car.IsSold) return false;

        var sale = new Sale
        {
            CarId = carId,
            AgentId = agentId,
            SalePrice = salePrice,
            SaleDate = DateTime.UtcNow
        };

        await _unitOfWork.Sales.AddAsync(sale, cancellationToken);
        car.IsSold = true;
        _unitOfWork.Cars.Update(car);

        await _unitOfWork.Wishlists.RemoveAllByCarIdAsync(carId, cancellationToken);
        await _unitOfWork.TestDrives.RemoveAllByCarIdAsync(carId, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Masina {CarId} vanduta de agentul {AgentId} la pretul {Price}.", carId, agentId, salePrice);
        return true;
    }
}
