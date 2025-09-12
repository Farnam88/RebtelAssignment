using RebtelAssignment.GrpcServer.Services;

namespace RebtelAssignment.GrpcServer.Pipelines;

public static class GrpcPipelines
{
    public static void RegisterGrpcPipeline(this WebApplication app)
    {
        // app.MapGrpcService<GreeterService>();
    }
}