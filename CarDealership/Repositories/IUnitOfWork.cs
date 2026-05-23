namespace CarDealership.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICarRepository Cars { get; }
    IBrandRepository Brands { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}