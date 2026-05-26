using System.ComponentModel.DataAnnotations;

namespace CarDealership.ViewModels;

public class BookTestDriveViewModel
{
    public int CarId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Data și ora sunt obligatorii")]
    [Display(Name = "Data și ora dorită")]
    public DateTime ScheduledAt { get; set; }

    [MaxLength(500)]
    [Display(Name = "Observații (opțional)")]
    public string? Notes { get; set; }
}
