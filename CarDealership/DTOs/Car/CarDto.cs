namespace CarDealership.DTOs;

public record CarDto(
    int Id,
    string ModelName,
    string BrandName,
    int Year,
    decimal Price,
    string? ImagePath,
    bool IsSold,
    List<string> Features);
