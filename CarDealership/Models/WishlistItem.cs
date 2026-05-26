namespace CarDealership.Models;

public class WishlistItem : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public int CarId { get; set; }
    public Car? Car { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
