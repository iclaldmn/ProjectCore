using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Linq.Expressions;

namespace Repository.Contracts;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context = context;

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Set<T>().FindAsync([id], cancellationToken);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Set<T>().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Set<T>().Where(predicate).ToListAsync(cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Set<T>().AnyAsync(predicate, cancellationToken);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        => predicate is null
            ? await _context.Set<T>().CountAsync(cancellationToken)
            : await _context.Set<T>().CountAsync(predicate, cancellationToken);

    public async Task<T?> GetWithIncludeAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();
        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllWithIncludeAsync(
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();
        foreach (var include in includes)
            query = query.Include(include);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _context.Set<T>().AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await _context.Set<T>().AddRangeAsync(entities, cancellationToken);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void UpdateRange(IEnumerable<T> entities) => _context.Set<T>().UpdateRange(entities);

    public void Remove(T entity) => _context.Set<T>().Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

    public async Task<IReadOnlyList<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default,
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        bool descending = false)
    {
        IQueryable<T> query = _context.Set<T>();

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query = descending ? query.OrderByDescending(orderBy)
                               : query.OrderBy(orderBy);

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetWithRawSqlAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters)
        => await _context.Set<T>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
    public IQueryable<T> AsQueryable()
    {
        return _context.Set<T>().AsNoTracking();
    }

}
