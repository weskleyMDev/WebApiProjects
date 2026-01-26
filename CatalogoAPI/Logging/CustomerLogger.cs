
namespace CatalogoAPI.Logging;

public class CustomerLogger(string name, CustomLoggerProviderConfiguration config) : ILogger
{
    private readonly string _name = name;
    private readonly CustomLoggerProviderConfiguration _config = config;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == _config.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";

        WriteTextToFile(message);
    }

    private static void WriteTextToFile(string message)
    {
        string filePath = @"C:\Users\weskl\OneDrive\Documentos\CatalogoAPILogs\log.txt";

        using StreamWriter writer = new(filePath, true);
        try
        {
            writer.WriteLine(message);
            writer.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }
}