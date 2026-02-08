namespace ConsumerExample.Application.Services
{
    public interface ILoggerService<T>
    {
        IDisposable Enrich(string key, string value);
        IDisposable Enrich(Dictionary<string, string> keyValuePairs);
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogCritical(Exception exception, string message, params object[] args);
    }
}
