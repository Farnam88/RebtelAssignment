using Microsoft.EntityFrameworkCore;
using RebTelAssignment.Domain.Models.BaseModels;
using RebtelAssignment.Infrastructure.Data.EntityConfigs;

namespace RebtelAssignment.Infrastructure.Data;

public interface IDbContext : IDisposable
{
    DbSet<TEntity> DbSet<TEntity>()
        where TEntity : class, IEntityFlag;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class ApplicationDbContext : DbContext, IDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }

    public DbSet<TEntity> DbSet<TEntity>() where TEntity : class, IEntityFlag
    {
        return base.Set<TEntity>();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookConfig).Assembly);
    }
}