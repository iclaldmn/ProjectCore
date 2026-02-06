using Application.Common;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // ✨ BÜTÜN Validator’ları OTOMATİK yükler
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        // ✨ MediatR kayıt
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        // ✨ Validation Pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // 🔥 AUTO MAPPER (EKSİK OLAN BUYDU)
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
