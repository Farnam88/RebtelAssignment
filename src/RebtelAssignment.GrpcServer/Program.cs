using RebtelAssignment.Application.DependencyInjections;
using RebtelAssignment.GrpcServer.DependencyInjections;
using RebtelAssignment.GrpcServer.Pipelines;
using RebtelAssignment.Infrastructure.DependencyInjections;

var builder = EnvironmentDependencyInjection.RegisterEnvironments(args);
builder.Services.RegisterGrpcServices();

builder.Services.RegisterApplicationServices();

builder.Services.RegisterInfrastructureDependencies();

var app = builder.Build();

app.RegisterGrpcPipeline();

// Configure the HTTP request pipeline.
app.RegisterRequestPipeline();
app.Run();