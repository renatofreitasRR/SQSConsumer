using SQSConsumerWorker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AdicionarDependencias();
builder.Services.AddCommandConsumer();
builder.Services.AddEventConsumer();

var host = builder.Build();
host.Run();
