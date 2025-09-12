namespace RebtelAssignment.GrpcServer.DependencyInjections;

public static class EnvironmentDependencyInjection
{
    public static WebApplicationBuilder RegisterEnvironments(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json", false, true);
        builder.Configuration.AddEnvironmentVariables();
        return builder;
    }
}