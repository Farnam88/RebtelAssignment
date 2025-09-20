using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models.BaseModels;

namespace RebtelAssignment.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _context;
    private bool _alreadyDisposed;

    public UnitOfWork(IDbContext context)
    {
        _context = context;
    }


    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }


    public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntityFlag
    {
        return new Repository<TEntity>(_context);
    }

    #region IDisposable

    public void Dispose()
    {
        if (_alreadyDisposed)
            return;
        _context.Dispose();
        _alreadyDisposed = true;
    }

    #endregion
}