using ComplexSQSConsumerWorker.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddConsumerConfiguration(builder.Configuration);

var host = builder.Build();
host.Run();
