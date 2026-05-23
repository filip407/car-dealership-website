using System.ComponentModel.DataAnnotations;

namespace CarDealership.DTOs;

public class FeatureDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}