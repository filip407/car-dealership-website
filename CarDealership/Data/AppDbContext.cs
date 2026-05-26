using CarDealership.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }
    public DbSet<TestDrive> TestDrives { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.Agent)
            .WithMany(u => u.Cars)
            .HasForeignKey(c => c.AgentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WishlistItem>()
            .HasOne(w => w.User)
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WishlistItem>()
            .HasOne(w => w.Car)
            .WithMany()
            .HasForeignKey(w => w.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TestDrive>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TestDrive>()
            .HasOne(t => t.Car)
            .WithMany()
            .HasForeignKey(t => t.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
