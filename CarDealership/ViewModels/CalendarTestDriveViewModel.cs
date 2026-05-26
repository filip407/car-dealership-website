namespace CarDealership.ViewModels;

public class CalendarTestDriveViewModel
{
    public int Id { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public string? Notes { get; set; }
    public bool IsConfirmed { get; set; }
}
