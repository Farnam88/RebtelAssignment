using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RebtelAssignment.Application.Data;
using RebtelAssignment.Infrastructure.Data;

namespace RebtelAssignment.Infrastructure.DependencyInjections;

internal static class DatabaseFacadeDependencies
{
    public static void RegisterDatabaseFacade(this IServiceCollection services)
    {
        services.AddDbContext<IDbContext, ApplicationDbContext>((o) =>
        {
            //connection string should not be hardcoded instead azure app config can be used
            o.UseSqlite("Data Source=rebtel_library_database.db");
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}