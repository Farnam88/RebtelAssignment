using RebtelAssignment.Application.DependencyInjections;
using RebtelAssignment.GrpcServer.DependencyInjections;
using RebtelAssignment.GrpcServer.Extensions;
using RebtelAssignment.GrpcServer.Pipelines;
using RebtelAssignment.Infrastructure.DependencyInjections;

var builder = EnvironmentDependencyInjection.RegisterEnvironments(args);
builder.Services.RegisterGrpcServices();

builder.Services.RegisterMapper();
builder.Services.RegisterApplicationServices();

builder.Services.RegisterInfrastructureDependencies();

var app = builder.Build();

await app.Services.MigrateAndSeedingAsync();

app.RegisterGrpcPipeline();



// Configure the HTTP request pipeline.
app.RegisterRequestPipeline();

await app.RunAsync();