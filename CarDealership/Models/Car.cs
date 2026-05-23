using System.ComponentModel.DataAnnotations;

namespace CarDealership.Models;

public class Car : BaseEntity
{
    [Required]
    [MinLength(2)]
    public string ModelName { get; set; } = string.Empty;

    [Required]
    [Range(1900, 2027)]
    public int Year { get; set; }

    [Required]
    [Range(1, 10000000)]
    public decimal Price { get; set; }

    [DataType(DataType.Date)]
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public int BrandId { get; set; }
    public Brand? Brand { get; set; }

    public string? AgentId { get; set; }
    public ApplicationUser? Agent { get; set; }

    public string? ImagePath { get; set; }
}