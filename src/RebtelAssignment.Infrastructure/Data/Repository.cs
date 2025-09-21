using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models.BaseModels;

namespace RebtelAssignment.Infrastructure.Data;

/// <summary>
/// Encapsulating interactions with database by Dapper ORM
/// </summary>
internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityFlag
{
    private readonly DbSet<TEntity> _dbSet;

    public Repository(IDbContext dbContext)
    {
        _dbSet = dbContext.DbSet<TEntity>();
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> spec,
        CancellationToken cancellationToken = default)
    {
        return (await _dbSet.WithSpecification(spec).FirstOrDefaultAsync(cancellationToken))!;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
    {
        return (await _dbSet.WithSpecification(spec).FirstOrDefaultAsync(cancellationToken))!;
    }

    public async Task<TResult> FirstAsync<TResult>(ISpecification<TEntity, TResult> spec,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.WithSpecification(spec).FirstAsync(cancellationToken);
    }

    public async Task<TEntity> FirstAsync(ISpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.WithSpecification(spec).FirstAsync(cancellationToken);
    }

    public async Task<IList<TResult>> ToListAsync<TResult>(ISpecification<TEntity, TResult> spec,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.WithSpecification(spec).ToListAsync(cancellationToken);
    }

    public async Task<IList<TEntity>> ToListAsync(ISpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.WithSpecification(spec).ToListAsync(cancellationToken)??new List<TEntity>();;
    }

    public async Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
    {
        return await _dbSet.WithSpecification(spec).CountAsync(cancellationToken);
    }
    
    public async Task<bool> AnyAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
    {
        return await _dbSet.WithSpecification(spec).AnyAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Task.FromResult(_dbSet.Remove(entity));
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}