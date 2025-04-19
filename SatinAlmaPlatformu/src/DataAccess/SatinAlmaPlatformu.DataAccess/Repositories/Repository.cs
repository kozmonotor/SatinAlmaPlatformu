using Microsoft.EntityFrameworkCore;
using SatinAlmaPlatformu.Core.Entities;
using SatinAlmaPlatformu.Core.Repositories;
using SatinAlmaPlatformu.DataAccess.Context;
using System.Linq.Expressions;

namespace SatinAlmaPlatformu.DataAccess.Repositories;

/// <summary>
/// IRepository arayüzünün generic implementasyonu
/// </summary>
/// <typeparam name="T">Entity tipi</typeparam>
public class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<T>();
    }

    #region Tekil Entity İşlemleri

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    #endregion

    #region Liste İşlemleri

    public virtual async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                    string? includeString = null,
                                                    bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;
        
        if (disableTracking)
            query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString))
            query = query.Include(includeString);

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                    List<Expression<Func<T, object>>>? includes = null,
                                                    bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;
        
        if (disableTracking)
            query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    #endregion

    #region Sayfalama İşlemleri

    public virtual async Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize,
                                                                                 Expression<Func<T, bool>>? predicate = null,
                                                                                 Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                                                 List<Expression<Func<T, object>>>? includes = null,
                                                                                 bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;
        
        if (disableTracking)
            query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
            query = query.Where(predicate);

        int totalCount = await query.CountAsync();

        if (orderBy != null)
            query = orderBy(query);

        var items = await query.Skip((pageIndex) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();

        return (items, totalCount);
    }

    #endregion

    #region Sayma ve Varlık Kontrolü

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate == null)
            return await _dbSet.CountAsync();
        
        return await _dbSet.CountAsync(predicate);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    #endregion

    #region Ekleme İşlemleri

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task<IReadOnlyList<T>> AddRangeAsync(IReadOnlyList<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return entities;
    }

    #endregion

    #region Güncelleme İşlemleri

    public virtual Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual Task UpdateRangeAsync(IReadOnlyList<T> entities)
    {
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    #endregion

    #region Silme İşlemleri

    public virtual Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(IReadOnlyList<T> entities)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    #endregion

    #region Soft Delete İşlemleri

    public virtual async Task SoftDeleteAsync(T entity)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;
            await UpdateAsync(entity);
        }
        else
        {
            await DeleteAsync(entity);
        }
    }

    public virtual async Task SoftDeleteRangeAsync(IReadOnlyList<T> entities)
    {
        foreach (var entity in entities)
        {
            await SoftDeleteAsync(entity);
        }
    }

    #endregion
} 