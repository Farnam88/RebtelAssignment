using RebtelAssignment.GrpcServer.DependencyInjections;
using RebtelAssignment.GrpcServer.Pipelines;

var builder = EnvironmentDependencyInjection.RegisterEnvironments(args);
builder.Services.RegisterGrpcServices();

var app = builder.Build();

app.RegisterGrpcPipeline();

// Configure the HTTP request pipeline.
app.RegisterRequestPipeline();
app.Run();