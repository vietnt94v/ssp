using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Ssp.Cmms.Application.Common.Behaviors;

namespace Ssp.Cmms.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(
            typeof(MediatR.IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}
