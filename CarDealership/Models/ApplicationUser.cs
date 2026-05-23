using Microsoft.AspNetCore.Identity;

namespace CarDealership.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public List<Car> Cars { get; set; } = [];
}