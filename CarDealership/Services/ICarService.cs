using CarDealership.Models;
using Microsoft.AspNetCore.Http;

namespace CarDealership.Services;

public interface ICarService
{
    Task<List<Car>> GetAllCarsAsync(CancellationToken cancellationToken = default);
    Task<Car?> GetCarByIdAsync(int id, CancellationToken cancellationToken = default);
Task CreateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken = default);
    Task UpdateCarAsync(Car car, IFormFile? imageFile, string webRootPath, CancellationToken cancellationToken = default);
    Task DeleteCarAsync(int id, string webRootPath, CancellationToken cancellationToken = default);
}