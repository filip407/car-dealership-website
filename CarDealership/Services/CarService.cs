using CarDealership.Models;
using CarDealership.Repositories;
using Microsoft.AspNetCore.Http;

namespace CarDealership.Services;

public class CarService : ICarService
{
    private readonly IUnitOfWork _unitOfWork;

    public CarService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Car>> GetAllCarsAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Cars.GetAllWithDetailsAsync(cancellationToken);
    }

    public async Task<Car?> GetCarByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Cars.GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public async Task<List<Car>> GetCarsByBrandAsync(int brandId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Cars.GetByBrandAsync(brandId, cancellationToken);
    }

    public async Task CreateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken = default)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            car.ImagePath = await SaveImageAsync(imageFile, webRootPath);
        }

        await _unitOfWork.Cars.AddAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken = default)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            DeleteImage(car.ImagePath, webRootPath);
            car.ImagePath = await SaveImageAsync(imageFile, webRootPath);
        }

        _unitOfWork.Cars.Update(car);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCarAsync(int id, string webRootPath, CancellationToken cancellationToken = default)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(id, cancellationToken);
        if (car != null)
        {
            DeleteImage(car.ImagePath, webRootPath);
            _unitOfWork.Cars.Delete(car);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    private static async Task<string> SaveImageAsync(IFormFile imageFile, string webRootPath)
    {
        var uploadsFolder = Path.Combine(webRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(fileStream);

        return Path.Combine("uploads", uniqueFileName).Replace('\\', '/');
    }

    private static void DeleteImage(string? imagePath, string webRootPath)
    {
        if (!string.IsNullOrEmpty(imagePath))
        {
            var fullPath = Path.Combine(webRootPath, imagePath.Replace('/', '\\'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}