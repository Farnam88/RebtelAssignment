namespace RebtelAssignment.GrpcServer.Pipelines;

public static class GrpcPipelines
{
    public static void RegisterGrpcPipeline(this WebApplication app)
    {
        app.MapGrpcService<Services.LoaningService>();
        app.MapGrpcService<Services.InsightsService>();

        app.MapGrpcReflectionService();
    }
}