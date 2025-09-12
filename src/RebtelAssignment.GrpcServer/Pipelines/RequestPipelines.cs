namespace RebtelAssignment.GrpcServer.Pipelines;

public static class RequestPipelines
{
    public static void RegisterRequestPipeline(this WebApplication app)
    {
        app.MapGet("/",
            () =>
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    }
}