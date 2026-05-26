using System.ComponentModel.DataAnnotations;

namespace CarDealership.DTOs;

public class TestDriveDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string CarName { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public string? Notes { get; set; }
    public bool IsConfirmed { get; set; }
}
