using CarDealership.DTOs;
using CarDealership.Models;

namespace CarDealership.Mappings;

public static class FeatureMappings
{
    public static FeatureDto ToDto(this Feature feature) => new(
        Id: feature.Id,
        Name: feature.Name);

    public static List<FeatureDto> ToDtoList(this IEnumerable<Feature> features)
        => features.Select(f => f.ToDto()).ToList();

    public static Feature ToEntity(this CreateFeatureDto dto) => new()
    {
        Name = dto.Name
    };

    public static void ApplyTo(this UpdateFeatureDto dto, Feature feature)
    {
        feature.Name = dto.Name;
    }
}
