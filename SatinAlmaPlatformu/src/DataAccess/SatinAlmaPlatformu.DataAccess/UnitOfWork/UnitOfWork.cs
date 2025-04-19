using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SatinAlmaPlatformu.Core.Entities;
using SatinAlmaPlatformu.Core.Repositories;
using SatinAlmaPlatformu.Core.UnitOfWork;
using SatinAlmaPlatformu.DataAccess.Context;
using SatinAlmaPlatformu.DataAccess.Repositories;
using System.Collections;

namespace SatinAlmaPlatformu.DataAccess.UnitOfWork;

/// <summary>
/// IUnitOfWork arayüzünün implementasyonu
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private Hashtable _repositories;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _repositories = new Hashtable();
    }

    public IRepository<T> Repository<T>() where T : class, IEntity
    {
        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (IRepository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            
            if (_transaction != null)
                await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Managed kaynakları serbest bırak
                _dbContext.Dispose();
                _transaction?.Dispose();
            }

            // Unmanaged kaynakları serbest bırak (varsa)
            _disposed = true;
        }
    }
} 