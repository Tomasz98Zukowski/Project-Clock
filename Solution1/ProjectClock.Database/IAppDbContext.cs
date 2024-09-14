using Microsoft.EntityFrameworkCore;

namespace ProjectClock.Database;

public interface IAppDbContext : IDisposable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
