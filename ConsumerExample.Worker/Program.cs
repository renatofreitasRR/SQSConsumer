using ConsumerExample.Application.Configurations;
using ConsumerExample.Infrastructure.Configurations;
using ConsumerExample.Worker;
using ConsumerExample.Worker.Configurations;

var builder = Host.CreateApplicationBuilder(args);

builder.AddInfrastructureLogging();

builder.Services
    .AddApplicationInjection(builder.Configuration)
    .AddInfrastructureInjection(builder.Configuration);

builder.Services.AddHostedService<WorkerBloqueio>();

var host = builder.Build();
host.Run();
