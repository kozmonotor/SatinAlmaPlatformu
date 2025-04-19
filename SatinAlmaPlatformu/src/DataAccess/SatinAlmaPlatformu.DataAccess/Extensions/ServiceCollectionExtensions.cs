using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SatinAlmaPlatformu.Core.UnitOfWork;
using SatinAlmaPlatformu.DataAccess.Context;
using SatinAlmaPlatformu.DataAccess.Repositories;
using SatinAlmaPlatformu.DataAccess.UnitOfWork;

namespace SatinAlmaPlatformu.DataAccess.Extensions;

/// <summary>
/// DataAccess katmanındaki servislerin kaydını yapan extension sınıfı
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// DataAccess katmanındaki tüm servisleri DI konteynerine kaydeder
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("SatinAlmaPlatformu.DataAccess")));

        // UnitOfWork ve Repository'leri ekleyeceğiz
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        
        return services;
    }
} 