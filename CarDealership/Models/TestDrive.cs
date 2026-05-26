using System.ComponentModel.DataAnnotations;

namespace CarDealership.Models;

public class TestDrive : BaseEntity
{
    public int CarId { get; set; }
    public Car? Car { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    [Required]
    public DateTime ScheduledAt { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public bool IsConfirmed { get; set; } = false;
}
