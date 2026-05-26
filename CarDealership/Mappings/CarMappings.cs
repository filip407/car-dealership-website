using CarDealership.DTOs;
using CarDealership.Models;
using CarDealership.ViewModels;

namespace CarDealership.Mappings;

public static class CarMappings
{
    public static CarDto ToDto(this Car car) => new(
        Id: car.Id,
        ModelName: car.ModelName,
        BrandName: car.Brand?.Name ?? string.Empty,
        Year: car.Year,
        Price: car.Price,
        ImagePath: car.ImagePath,
        IsSold: car.IsSold,
        Features: car.Features?.Select(f => f.Name).ToList() ?? new());

    public static List<CarDto> ToDtoList(this IEnumerable<Car> cars)
        => cars.Select(c => c.ToDto()).ToList();

    public static CarViewModel ToViewModel(this Car car) => new()
    {
        Id = car.Id,
        BrandName = car.Brand?.Name ?? string.Empty,
        ModelName = car.ModelName,
        Year = car.Year,
        Price = car.Price,
        ImagePath = car.ImagePath,
        IsSold = car.IsSold,
        AddedAt = car.AddedAt,
        FeatureNames = car.Features?.Select(f => f.Name).ToList() ?? new()
    };

    public static List<CarViewModel> ToViewModelList(this IEnumerable<Car> cars)
        => cars.Select(c => c.ToViewModel()).ToList();
}
