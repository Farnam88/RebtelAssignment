using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RebtelAssignment.Application.Common.PipelineBehaviors;

namespace RebtelAssignment.Application.DependencyInjections;

public static class ApplicationDependencyInjections
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyInfo).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyInfo).Assembly);
    }
}