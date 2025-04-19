using SatinAlmaPlatformu.Core.Entities;
using SatinAlmaPlatformu.Core.Repositories;

namespace SatinAlmaPlatformu.Core.UnitOfWork;

/// <summary>
/// Unit of Work pattern için arayüz
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repository'ye erişim sağlar
    /// </summary>
    /// <typeparam name="T">Entity tipi</typeparam>
    /// <returns>Entity için repository</returns>
    IRepository<T> Repository<T>() where T : class, IEntity;
    
    /// <summary>
    /// Değişiklikleri kaydeder
    /// </summary>
    /// <returns>Etkilenen kayıt sayısı</returns>
    Task<int> SaveChangesAsync();
    
    /// <summary>
    /// Transaction başlatır
    /// </summary>
    Task BeginTransactionAsync();
    
    /// <summary>
    /// Transaction'ı tamamlar (commit)
    /// </summary>
    Task CommitTransactionAsync();
    
    /// <summary>
    /// Transaction'ı geri alır (rollback)
    /// </summary>
    Task RollbackTransactionAsync();
} 