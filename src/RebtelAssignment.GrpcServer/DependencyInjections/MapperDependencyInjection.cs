using Google.Protobuf.Collections;
using Mapster;
using MapsterMapper;
using RebtelAssignment.Application;

namespace RebtelAssignment.GrpcServer.DependencyInjections;

public static class MapperDependencyInjection
{
    public static void RegisterMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(
            typeof(ApplicationAssemblyInfo).Assembly,
            typeof(Program).Assembly);
        config.Default.Config.AllowImplicitDestinationInheritance = true;
        config.Default.UseDestinationValue(member => member.SetterModifier == AccessModifier.None &&
                                                     member.Type.IsGenericType &&
                                                     member.Type.GetGenericTypeDefinition() == typeof(RepeatedField<>));

        config.Compile();
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}