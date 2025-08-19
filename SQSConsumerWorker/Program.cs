using SQSConsumerWorker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AdicionarDependencias();
builder.Services.AddWorkerMovimentosEntrada();
builder.Services.AddWorkerMovimentosRespostas();

var host = builder.Build();
host.Run();
