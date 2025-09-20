using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RebtelAssignment.Application.Data;
using RebtelAssignment.Infrastructure.Data;

namespace RebtelAssignment.Infrastructure.DependencyInjections;

internal static class DatabaseFacadeDependencies
{
    public static void RegisterDatabaseFacade(this IServiceCollection services)
    {
        services.AddDbContext<IDbContext,ApplicationDbContext>((sp, o) =>
        {
            o.UseInMemoryDatabase("RebtelAssignmentDb", options => { options.EnableNullChecks(); });
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}