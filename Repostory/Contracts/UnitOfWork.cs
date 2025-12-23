using Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;

namespace Repository.Contracts;

public class UnitOfWork(AppDbContext context, IServiceProvider provider) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private readonly IServiceProvider _provider = provider;

    private readonly Dictionary<Type, object> _repositories = [];

    public IGenericRepository<T> Repository<T>() where T : class
    {
        if (_repositories.TryGetValue(typeof(T), out var repo))
            return (IGenericRepository<T>)repo;

        var instance = _provider.GetRequiredService<IGenericRepository<T>>();

        _repositories[typeof(T)] = instance;
        return instance;
    }

    public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
