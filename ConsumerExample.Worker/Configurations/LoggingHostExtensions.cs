using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace ConsumerExample.Worker.Configurations
{
    public static class LoggingHostExtensions
    {
        public static IHostApplicationBuilder AddInfrastructureLogging(this IHostApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();

            builder.Services.AddSerilog((services, loggerConfiguration) =>
            {
                Configure(
                    loggerConfiguration,
                    builder.Configuration
                );
            });

            return builder;
        }

        private static void Configure(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            loggerConfiguration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(new JsonFormatter() );
        }
    }
}
