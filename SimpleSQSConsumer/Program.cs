using SimpleSQSConsumer.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddConfiguration(builder.Configuration);
var host = builder.Build();
host.Run();
