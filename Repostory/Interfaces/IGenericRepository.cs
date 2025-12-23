using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IGenericRepository<T> where T : class
{
    // Basic Query
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    // Check & Count
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    // Include (EF Core 9 improved includes)
    Task<T?> GetWithIncludeAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    Task<IReadOnlyList<T>> GetAllWithIncludeAsync(
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    // Commands
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);

    // Pagination (EF Core 9 async streaming opt.)
    Task<IReadOnlyList<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default,
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        bool descending = false);

    // Raw SQL (EF Core 9)
    Task<IReadOnlyList<T>> GetWithRawSqlAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);
}
