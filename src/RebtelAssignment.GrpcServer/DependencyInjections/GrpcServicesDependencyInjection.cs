namespace RebtelAssignment.GrpcServer.DependencyInjections;

public static class GrpcServicesDependencyInjection
{
    public static void RegisterGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();
    }
}