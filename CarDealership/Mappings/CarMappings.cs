using CarDealership.Models;
using CarDealership.ViewModels;

namespace CarDealership.Mappings;

public static class CarMappings
{
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
