using RebTelAssignment.Domain.Models.BaseModels;

namespace RebtelAssignment.Application.Data;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntityFlag;
}