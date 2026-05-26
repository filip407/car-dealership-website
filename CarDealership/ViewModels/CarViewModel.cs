namespace CarDealership.ViewModels;

public class CarViewModel
{
    public int Id { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
    public bool IsSold { get; set; }
    public DateTime AddedAt { get; set; }
    public List<string> FeatureNames { get; set; } = new();
}
