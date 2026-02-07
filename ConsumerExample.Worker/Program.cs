using ConsumerExample.Application.Configurations;
using ConsumerExample.Infrastructure.Configurations;
using ConsumerExample.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddInfrastructureInjection(builder.Configuration)
    .AddApplicationInjection(builder.Configuration);

builder.Services.AddHostedService<WorkerBloqueio>();

var host = builder.Build();
host.Run();
