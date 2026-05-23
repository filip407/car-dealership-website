using System.ComponentModel.DataAnnotations;

namespace CarDealership.Models;

public class Sale : BaseEntity
{
    public int CarId { get; set; }
    public Car? Car { get; set; }

    public string AgentId { get; set; } = string.Empty;
    public ApplicationUser? Agent { get; set; }

    [Required]
    public decimal SalePrice { get; set; }

    [DataType(DataType.Date)]
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
}