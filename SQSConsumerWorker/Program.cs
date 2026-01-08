using ComplexSQSConsumerWorker.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSqsConfig(builder.Configuration);
builder.Services.AddConsumers(builder.Configuration);
builder.Services.AddRepositories();

var host = builder.Build();
host.Run();
