using ConsumerExample.Application.Services;
using Serilog;
using Serilog.Context;

namespace ConsumerExample.Infrastructure.Services
{
    public class LoggerService<T> : ILoggerService<T>
    {
        private readonly ILogger _logger;

        public LoggerService()
        {
            _logger = Serilog.Log.ForContext("SourceContext", typeof(T).Name);
        }


        public IDisposable Enrich(Dictionary<string, string> keyValuePairs)
        {
            var disposables = keyValuePairs
                .Select(kv => LogContext.PushProperty(kv.Key, kv.Value))
                .ToList();

            return new CompositeDisposable(disposables);
        }

        public IDisposable Enrich(string key, string value)
        {
            return LogContext.PushProperty(key, value);
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            _logger.Fatal(exception, message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.Error(exception, message, args);
        }
        public void LogError(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.Information(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.Warning(message, args);
        }
    }

    internal sealed class CompositeDisposable : IDisposable
    {
        private readonly IEnumerable<IDisposable> _disposables;

        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            _disposables = disposables;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
