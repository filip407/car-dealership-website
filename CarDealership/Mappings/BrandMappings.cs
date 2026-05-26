using CarDealership.DTOs;
using CarDealership.Models;

namespace CarDealership.Mappings;

public static class BrandMappings
{
    public static BrandDto ToDto(this Brand brand) => new(
        Id: brand.Id,
        Name: brand.Name);

    public static List<BrandDto> ToDtoList(this IEnumerable<Brand> brands)
        => brands.Select(b => b.ToDto()).ToList();
}
