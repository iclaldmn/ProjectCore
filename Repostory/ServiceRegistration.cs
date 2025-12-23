using Microsoft.Extensions.DependencyInjection;
using Repository.Contracts;
using Repository.Interfaces;

namespace Repository;

public static class ServiceRegistration
{
    public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
