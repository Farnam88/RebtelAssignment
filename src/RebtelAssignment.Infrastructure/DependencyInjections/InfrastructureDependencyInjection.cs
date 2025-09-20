using Microsoft.Extensions.DependencyInjection;

namespace RebtelAssignment.Infrastructure.DependencyInjections;

public static class InfrastructureDependencyInjection
{
    public static void RegisterInfrastructureDependencies(this IServiceCollection services)
    {
        services.RegisterDatabaseFacade();
    }
}