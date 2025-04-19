using SatinAlmaPlatformu.Core.Entities;
using System.Linq.Expressions;

namespace SatinAlmaPlatformu.Core.Repositories;

/// <summary>
/// Generic Repository arayüzü
/// </summary>
/// <typeparam name="T">Entity tipi</typeparam>
public interface IRepository<T> where T : class, IEntity
{
    // Tekil entity işlemleri
    Task<T?> GetByIdAsync(int id);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    
    // Liste işlemleri
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                   string? includeString = null,
                                   bool disableTracking = true);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                   List<Expression<Func<T, object>>>? includes = null,
                                   bool disableTracking = true);
    
    // Sayfalama işlemleri
    Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize,
                                                               Expression<Func<T, bool>>? predicate = null,
                                                               Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                               List<Expression<Func<T, object>>>? includes = null,
                                                               bool disableTracking = true);
    
    // Sayma işlemleri
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    
    // Varlık kontrolü
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    
    // Ekleme işlemleri
    Task<T> AddAsync(T entity);
    Task<IReadOnlyList<T>> AddRangeAsync(IReadOnlyList<T> entities);
    
    // Güncelleme işlemleri
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IReadOnlyList<T> entities);
    
    // Silme işlemleri
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IReadOnlyList<T> entities);
    
    // Soft delete işlemleri (BaseEntity üzerinden)
    Task SoftDeleteAsync(T entity);
    Task SoftDeleteRangeAsync(IReadOnlyList<T> entities);
} 