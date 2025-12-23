using Repository.Interfaces;

namespace Repository.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
