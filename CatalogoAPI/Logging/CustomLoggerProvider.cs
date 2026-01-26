using System.Collections.Concurrent;

namespace CatalogoAPI.Logging;

public class CustomLoggerProvider(CustomLoggerProviderConfiguration config) : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration _config = config;
    private readonly ConcurrentDictionary<string, CustomerLogger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, _config));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}