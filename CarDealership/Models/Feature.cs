using System.ComponentModel.DataAnnotations;

namespace CarDealership.Models;

public class Feature : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Relația Many-to-Many către Mașini
    public ICollection<Car> Cars { get; set; } = new List<Car>();
}