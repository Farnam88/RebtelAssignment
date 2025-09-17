using RebtelAssignment.GrpcServer.Helpers.Interceptors;

namespace RebtelAssignment.GrpcServer.DependencyInjections;

public static class GrpcServicesDependencyInjection
{
    public static void RegisterGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc(options => { options.Interceptors.Add<ExceptionInterceptor>(); });
    }
}