using CarDealership.Data;
using CarDealership.Models;

namespace CarDealership.Repositories;

public class BrandRepository : Repository<Brand>
{
    public BrandRepository(AppDbContext context) : base(context) { }
}
