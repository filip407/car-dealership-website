namespace CarDealership.ViewModels;

public class WishlistItemViewModel
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Year { get; set; }
    public string? ImagePath { get; set; }
    public DateTime AddedAt { get; set; }
}
